using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : Blob {
    [SerializeField] private float _speed;
    [SerializeField] private float _cameraShakeStrength; 
    [SerializeField] private float _cameraShakeFrequency;
    [SerializeField] private HealthBar _healthBar; 
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; 
    [SerializeField] private GameObject _bulletHitPrefab;

    private const float _minHitEffectDur = 0.15f;
    private const float _maxHitEffectDur = 0.25f;
    private const float _spawnTimeIncrement = 0.025f;
    private const float _spawnStepIncrement = 0.1f;
    private const float _spawnScaleOvershoot = 1.25f;
    private Renderer _renderer;
    private Renderer[] _rendererArray;
    private GameObject _bulletHit;
    private int _damageTaken;

    void Start() {
        _renderer = GetComponent<Renderer>();
        _rendererArray = gameObject.GetComponentsInChildren<Renderer>();
        _healthBar.SetMaxHealth(MaxHealth);
        _damageTaken = 0;

        _bulletHit = Instantiate(_bulletHitPrefab, transform.position, Quaternion.identity);
        _bulletHit.transform.parent = transform;
        _bulletHit.SetActive(false);

        StartCoroutine(SpawnCoroutine());
    }

    public override void TakeDamage(int dmg) {
        _damageTaken += dmg;
        StartCoroutine(BulletHitCoroutine());
        AudioManager.Instance.Play("BulletBlobHit");
        base.TakeDamage(dmg);
        _healthBar.SetHealth(CurrentHealth);
    }

    public override void Heal(int num) {
        base.Heal(num);
        _healthBar.SetHealth(CurrentHealth);
    }

    protected override void Die() {
        DataManager.GameOverMessage = "Get Rekt' Scrub";
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// VFX when player gets damanged.
    /// </summary>
    IEnumerator BulletHitCoroutine() {
        float time = Random.Range(_minHitEffectDur, _maxHitEffectDur);

        _bulletHit.SetActive(true);
        CameraShake(_cameraShakeStrength, _cameraShakeFrequency);

        yield return new WaitForSeconds(time);

        _bulletHit.SetActive(false);
        CameraShake(0, 0);
    }

    /// <summary>
    /// Lerp, Slerp, and SmoothStep are not used becuase they are unable
    /// to start with a scale of 0 due to the short timeframe of the effect.
    /// </summary>
    /// <param name="time">How long to wait before changing the step value.</param>
    /// <param name="step">How much to increment/decrement the step value.</param>
    IEnumerator SpawnCoroutine() {
        float scale = 0;

        Renderer(false);
        AudioManager.Instance.Play("PlayerSpawn");
        _bulletHit.SetActive(true);
        _bulletHit.transform.localScale = Vector3.zero;

        // Increase bullet hit animation size
        while (scale < _spawnScaleOvershoot) {
            yield return new WaitForSeconds(_spawnTimeIncrement);
            scale += _spawnStepIncrement;
            _bulletHit.transform.localScale = new Vector3(scale, scale, scale);
        }

        Renderer(true);

        // Decrease bullet hit animation size
        while (scale > 1) {
            yield return new WaitForSeconds(_spawnTimeIncrement);
            scale -= _spawnStepIncrement;
            _bulletHit.transform.localScale = new Vector3(scale, scale, scale);
        }

        AudioManager.Instance.Stop("PlayerSpawn");
        _bulletHit.transform.localScale = Vector3.one;
        _bulletHit.SetActive(false);
    }

    /// <summary>
    /// Turns on or off the Renderer component of this game object and its children.
    /// </summary>
    private void Renderer(bool b) {
        foreach (Renderer r in _rendererArray) {
            r.enabled = b;
        }
        _renderer.enabled = b;
    }

    private void CameraShake(float intensity, float frequency) {
        CinemachineBasicMultiChannelPerlin cinemachineBMCP = 
            _cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // can prob get rid of this once transition to new blob/turret is done
        if (cinemachineBMCP == null) {
            throw new NullReferenceException("Make sure PlayerCamera -> CinemachineVirtualCamera" +
                " -> Noise is set to 'Basic Multi Channel Perlin'. Check PlayerCamera prefab for" +
                "default values.");
        }

        cinemachineBMCP.m_AmplitudeGain = intensity;
        cinemachineBMCP.m_FrequencyGain = frequency;
    }

    public int DamageTaken {
        get { return _damageTaken; }
    }

    public float Speed {
        get { return _speed; }
        set {
            if (value <= 0) {
                throw new ArgumentException("Speed must be positive.");
            } else {
                _speed = value;
            }
        }
    }

    protected override void OnValidate() {
        base.OnValidate();
        if (_speed < 0) {
            _speed = 0;
        }

        if (_cameraShakeStrength < 0) {
            _cameraShakeStrength = 0;
        }

        if (_cameraShakeFrequency < 0) {
            _cameraShakeFrequency = 0;
        }
    }
}
