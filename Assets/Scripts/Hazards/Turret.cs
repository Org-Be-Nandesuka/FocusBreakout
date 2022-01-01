using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Simulates a gun firing at Blobs.
/// </summary>
public class Turret : MonoBehaviour {
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    // Represents where the bullet may land when firing at a Blob.
    // Spread that is equal or less than the Blob's radius results in perfect accuracy.
    [SerializeField] private float _spread;
    [SerializeField] private TargetArea _targetArea;
    [SerializeField] private GameObject _turretTerrainHit;

    private const float _minHitEffectDur = 0.05f;
    private const float _maxHitEffectDur = 0.15f;
    private const float _maxDistance = Constants.MaxMapDistance;

    // LineRenderer 'Flashing' Stats
    private const float _onTime = 0.02f;
    private const float _offTime = 0.04f;
    private const int _interations = 3;

    private Blob _target;
    private LineRenderer _lineRenderer;
    private ParticleSystem _muzzleFlash;
    private AudioSource _audioSource;
    private AudioClip _audioClip;
    private float _nextTimeToFire;

    void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;

        _muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = AudioManager.Instance.GetAudioClip("BulletTerrainHit");
        _nextTimeToFire = 0f;

        _turretTerrainHit = Instantiate(_turretTerrainHit, transform);
        _turretTerrainHit.SetActive(false);
    }

    void Update() {
        _lineRenderer.SetPosition(0, transform.position);
        TargetBlob();
    }

    /// <summary>
    /// Shooter will find and follow a blob (as long as it's "visible")
    /// </summary>
    private void TargetBlob() {
        Vector3 targetDirection;
        RaycastHit hit;

        // Find blob
        if (_target == null) {
            // returns null if there are no Blobs in Target Area
            _target = _targetArea.GetRandomBlob();

            if (_target == null) {
                return;
            }
        }

        targetDirection = GetTargetDirection(_target);

        // Check if it's "visible" by the shooter
        if (Physics.Raycast(transform.position, targetDirection, out hit, _maxDistance)) {
            // Follow blob until it is no longer "visible"
            if (hit.collider.CompareTag("Blob")) {
                _target = hit.collider.gameObject.GetComponent<Blob>();
                _lineRenderer.SetPosition(1, hit.point);
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, targetDirection);

                if (Time.time >= _nextTimeToFire) {
                    ShootBlob(targetDirection);
                }
            } else {
                _target = null;
            }
        }
    }

    /// <summary>
    /// Imitates a gun shooting at a Blob. This uses _spread as its accuracy.
    /// </summary>
    /// <param name="targetDir">Location to shoot at</param>
    private void ShootBlob(Vector3 targetDir) {
        Vector3 targetDirection = AimSpread(targetDir);
        RaycastHit hit;

        _nextTimeToFire = Time.time + _fireRate;
        _muzzleFlash.Play();
        _audioSource.Play();

        if (Physics.Raycast(transform.position, targetDirection, out hit, _maxDistance)) {
            _lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Blob")) {
                _target.TakeDamage(_damage);
                if (!_target.gameObject.activeSelf) {
                    _target = null;
                }
            } else {
                _turretTerrainHit.transform.position = hit.point;
                StartCoroutine(TerrainHitCoroutine());
                AudioSource.PlayClipAtPoint(_audioClip, hit.point, 0.1f);
            }
        }
    }

    private Vector3 GetTargetDirection(Blob blob) {
        return blob.transform.position - transform.position;
    }

    /// <summary>
    /// picks a random point inside a sphere with _spread as the radius
    /// </summary>
    private Vector3 AimSpread(Vector3 pos) {
        return Random.insideUnitSphere * _spread + pos;
    }

    IEnumerator TerrainHitCoroutine() {
        float time = Random.Range(_minHitEffectDur, _maxHitEffectDur);
        _turretTerrainHit.SetActive(true);

        yield return new WaitForSeconds(time);
        _turretTerrainHit.SetActive(false);
    }

    IEnumerator FlashLineRendererCoroutine(float onTime, float offTime, int num) {
        for (int i = 0; i < num; i++) {
            _lineRenderer.enabled = true;
            yield return new WaitForSeconds(onTime);
            _lineRenderer.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }

    private void OnValidate() {
        if (_fireRate < 0) {
            _fireRate = 0;
        }

        if (_damage < 0) {
            _damage = 0;
        }

        if (_spread < 0) {
            _spread = 0;
        }
    }
}
