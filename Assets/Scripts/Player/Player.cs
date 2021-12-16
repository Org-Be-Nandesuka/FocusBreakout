using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Blob {
    [SerializeField] private float _cameraShakeStrength; 
    [SerializeField] private float _cameraShakeFrequency;
    [SerializeField] private HealthBar _healthBar; // UI representation of player's health
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; // Cinemachine camera following player
    [SerializeField] private GameObject _bulletHitPrefab; // particle effect when hit by bullet

    // _hitEffectDur - _hitEffectDurRange should not be negative
    // the resulting float may be used in WaitForSeconds()
    private const float _hitEffectDur = 0.2f;
    private const float _hitEffectDurRange = 0.05f;
    private Renderer _renderer;
    private GameObject _bulletHit;
    private int _damageTaken;

    void Start() {
        _renderer = GetComponent<Renderer>();
        _healthBar.SetMaxHealth(MaxHealth);
        _damageTaken = 0;

        _bulletHit = Instantiate(_bulletHitPrefab, transform.position, Quaternion.identity);
        _bulletHit.transform.parent = transform;
        _bulletHit.SetActive(false);
    }

    public override void TakeDamage(int dmg) {
        base.TakeDamage(dmg);
        _damageTaken += dmg;
        StartCoroutine(BulletHitCoroutine());
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
        float time = Random.Range(_hitEffectDur - _hitEffectDurRange, _hitEffectDur + _hitEffectDurRange);

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


    protected override void OnValidate() {
        base.OnValidate();
        if (_cameraShakeStrength < 0) {
            _cameraShakeStrength = 0;
        }

        if (_cameraShakeFrequency < 0) {
            _cameraShakeFrequency = 0;
        }
    }
}
