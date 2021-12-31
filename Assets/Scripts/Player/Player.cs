using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : Blob {
    [SerializeField] private float _speed;
    [SerializeField] private float _cameraShakeStrength; 
    [SerializeField] private float _cameraShakeFrequency;
    [SerializeField] private HealthBar _healthBar; 
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; 
    [SerializeField] private GameObject _bulletHitPrefab;

    // _hitEffectDur - _hitEffectDurRange should not be negative
    // the resulting float may be used in WaitForSeconds()
    private const float _hitEffectDur = 0.2f;
    private const float _hitEffectDurRange = 0.05f;
    private Renderer _renderer;
    private GameObject _bulletHit;
    private int _damageTaken;


    void Awake() {
        _renderer = GetComponent<Renderer>();
        _healthBar.SetMaxHealth(MaxHealth);
        _damageTaken = 0;

        _bulletHit = Instantiate(_bulletHitPrefab, transform.position, Quaternion.identity);
        _bulletHit.transform.parent = transform;
        _bulletHit.SetActive(false);
    }

    public override void TakeDamage(int dmg) {
        _damageTaken += dmg;
        StartCoroutine(BulletHitCoroutine());
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

    // VFX when player is hit.
    IEnumerator BulletHitCoroutine() {
        Renderer[] rendererArray = gameObject.GetComponentsInChildren<Renderer>();
        float time = UnityEngine.Random.Range(_hitEffectDur - _hitEffectDurRange,
            _hitEffectDur + _hitEffectDurRange);

        foreach (Renderer r in rendererArray) {
            r.enabled = false;
        }
        _renderer.enabled = false;
        _bulletHit.SetActive(true);
        CameraShake(_cameraShakeStrength, _cameraShakeFrequency);

        yield return new WaitForSeconds(time);

        foreach (Renderer r in rendererArray) {
            r.enabled = true;
        }
        _renderer.enabled = true;
        _bulletHit.SetActive(false);
        CameraShake(0, 0);
    }

    private void CameraShake(float intensity, float frequency) {
        CinemachineBasicMultiChannelPerlin cinemachineBMCP = 
            _cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBMCP.m_AmplitudeGain = intensity;
        cinemachineBMCP.m_FrequencyGain = frequency;
    }

    public int DamageTaken {
        get { return _damageTaken; }
    }

    public float Speed {
        get { return _speed; }
        set {
            if (value < 0) {
                throw new ArgumentException("Speed must be at least 0.");
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
