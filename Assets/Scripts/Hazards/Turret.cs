using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Simulates a gun firing at Blobs.
/// </summary>
public class Turret : MonoBehaviour {
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _spread; // spread applied on blob radius
    [SerializeField] private TargetArea _targetArea;
    [SerializeField] private GameObject _bulletTerrainHit;
    [SerializeField] private Audio[] _audioArray;

    // _hitEffectDur - _hitEffectDurRange should not be negative
    // the resulting float may be used in WaitForSeconds()
    private const float _hitEffectDur = 0.1f;
    private const float _hitEffectDurRange = 0.05f;

    // LineRenderer 'Flashing' Stats
    private const float _onTime = 0.02f;
    private const float _offTime = 0.04f;
    private const int _interations = 3;

    private Blob _target;
    private LineRenderer _lineRenderer;
    private ParticleSystem _muzzleFlash;
    private Vector3 _targetDirection;
    private float _nextTimeToFire;
    private bool _targetNeeded;

    void Start() {
        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;

        _muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        _nextTimeToFire = 0f;

        _bulletTerrainHit = Instantiate(_bulletTerrainHit, transform.position, Quaternion.identity);
        _bulletTerrainHit.SetActive(false);

        _targetNeeded = true;
        _targetDirection = Vector3.zero;
    }

    void Update() {
        _lineRenderer.SetPosition(0, transform.position);
        TargetBlob();
    }

    // Shooter will find and follow a blob (as long as it's "visible")
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
        if (Physics.Raycast(transform.position, targetDirection, out hit, Constants.MaxMapDistance)) {
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

    // Imitates a gun shooting at a blob
    private void ShootBlob(Vector3 targetDir) {
        Vector3 targetDirection = AimSpread(targetDir);
        RaycastHit hit;

        _nextTimeToFire = Time.time + _fireRate;
        _muzzleFlash.Play();

        if (Physics.Raycast(transform.position, targetDirection, out hit, Constants.MaxMapDistance)) {
            _lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.CompareTag("Blob")) {
                _target.TakeDamage(_damage);
                if (!_target.gameObject.activeSelf) {
                    _target = null;
                }
            } else {
                _bulletTerrainHit.transform.position = hit.point;
                StartCoroutine(TerrainHitCoroutine());
            }
        }
    }

    private Vector3 GetTargetDirection(Blob blob) {
        return blob.transform.position - transform.position;
    }

    // picks a random point inside a sphere with _spread as the radius
    private Vector3 AimSpread(Vector3 pos) {
        return Random.insideUnitSphere * _spread + pos;
    }

    IEnumerator TerrainHitCoroutine() {
        float time = Random.Range(_hitEffectDur - _hitEffectDurRange, _hitEffectDur + _hitEffectDurRange);
        _bulletTerrainHit.SetActive(true);

        yield return new WaitForSeconds(time);
        _bulletTerrainHit.SetActive(false);
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
