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

    private GameObject _target;
    private LineRenderer _lineRenderer;
    private ParticleSystem _muzzleFlash;
    private float _nextTimeToFire;
    private bool _newTarget;

    void Start() {
        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;

        _muzzleFlash = transform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        _nextTimeToFire = 0f;

        _bulletTerrainHit = Instantiate(_bulletTerrainHit, transform.position, Quaternion.identity);
        _bulletTerrainHit.SetActive(false);

        _newTarget = true;
    }

    void Update() {
        _lineRenderer.SetPosition(0, transform.position);
        TargetBlob();
    }

    // Shooter will find and follow a blob (as long as it's "visible")
    private void TargetBlob() {
        Vector3 targetDirection;
        RaycastHit hit;
        Blob target = _targetArea.GetRandomBlob();

        if (target == null) {
            return;
        }

        targetDirection = GetTargetDirection(target);
        targetDirection = AimSpread(targetDirection); // Apply shooter spread

        // Check if it's "visible" by the shooter
        if (Physics.Raycast(transform.position, targetDirection, out hit, Constants.MaxMapDistance)) {
            // Follow blob until it is no longer "visible"
            if (hit.collider.CompareTag("Blob")) {
                target = hit.collider.gameObject.GetComponent<Blob>();
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, targetDirection);

                // Cast LineRenderer to location after shooter spread is applied
                /*                if (Physics.Raycast(transform.position, targetDirection, out hit, Constants.MaxMapDistance)) {
                                    _lineRenderer.SetPosition(1, hit.point);
                                    if (_newTarget) {
                                        _newTarget = false;
                                        StartCoroutine(FlashLineRendererCoroutine(_onTime, _offTime, _interations));
                                    }
                                }*/
                if (Time.time >= _nextTimeToFire) {
                    //ShootBlob(target);
                    _nextTimeToFire = Time.time + _fireRate;
                    _muzzleFlash.Play();
                    target.TakeDamage(_damage);
                }
            } else {
                print("target null");
                target = null;
            }
        }
    }

    // Imitates a gun shooting at a blob
    private void ShootBlob(Blob blob) {
        RaycastHit hit;

        _nextTimeToFire = Time.time + _fireRate;
        _muzzleFlash.Play();
        //AudioSource.PlayClipAtPoint(_audioArray[0].Clip, transform.position, _audioArray[0].Volume);

        if (Physics.Raycast(transform.position, blob.transform.position, out hit, Constants.MaxMapDistance)) {
            if (hit.collider.CompareTag("Blob")) {
                blob.TakeDamage(_damage);
                print(_damage);
            } else {
                _bulletTerrainHit.transform.position = hit.point;
                //AudioSource.PlayClipAtPoint(_audioArray[1].Clip, hit.point, _audioArray[1].Volume);
                //AudioSource.PlayClipAtPoint(_audioArray[2].Clip, hit.point, _audioArray[2].Volume);
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
