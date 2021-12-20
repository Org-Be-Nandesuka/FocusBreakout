using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BasicBlob : Blob {
    [SerializeField] private Audio[] _audioArray;

    private BlobBehavior _blobBehavior;
    private ObjectPool<BasicBlob> _blobPool;
    private Vector3 _worldDirection;
    private CharacterController _controller;
    private Coroutine _currentCoroutine;

    void Awake() {
        _controller = GetComponent<CharacterController>();
        _blobBehavior = new BlobBehavior();
        _blobBehavior.CheckData();

        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }
    }

    void FixedUpdate() {
        _controller.Move(_worldDirection * _blobBehavior.Speed * Time.fixedDeltaTime);
    }

    void OnEnable() {
        if (_blobBehavior.Lifespan != 0) {
            StartCoroutine(LifespanCoroutine());
        }

        if (_blobBehavior.DirectionChangeTime != 0) {
            _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
        }

        ResetCurrentHealth();
        _worldDirection = _blobBehavior.MoveDirection;
        // transform.rotation = Quaternion.LookRotation(...) also works. I chose transform.forward
        // because it doesn't require a method and therefore should be more efficient. However,
        // I am not entirely sure how I feel about messing with the Blob's "forward" direction...
        transform.forward = _blobBehavior.FacingDirection;
    }

    IEnumerator LifespanCoroutine() {
        yield return new WaitForSeconds(_blobBehavior.Lifespan);
        Die();
    }

    IEnumerator ReverseDirectionCoroutine() {
        yield return new WaitForSeconds(_blobBehavior.DirectionChangeTime);
        ReverseDirection();
    }

    private void ReverseDirection() {
        if (_blobBehavior.DirectionChangeTime != 0) {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
        }

        _worldDirection *= -1;
    }

    protected override void Die() {
        _blobPool.Release(this);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        ReverseDirection();
    }

    public BlobBehavior BasicBlobBehavior {
        get { return _blobBehavior; }
        set {
            value.CheckData();
            _blobBehavior = value;
        }
    }

    public ObjectPool<BasicBlob> BlobPool {
        get { return _blobPool; }
        set { _blobPool = value; }
    }

    public float WorldDirectionX {
        get { return _worldDirection.x; }
        set {
            if (value > 1) {
                _worldDirection.x = 1;
            } else if (value < -1) {
                _worldDirection.x = -1;
            } else {
                _worldDirection.x = value;
            }
        }
    }

    public float WorldDirectionY {
        get { return _worldDirection.y; }
        set {
            if (value > 1) {
                _worldDirection.y = 1;
            } else if (value < -1) {
                _worldDirection.y = -1;
            } else {
                _worldDirection.y = value;
            }
        }
    }

    public float WorldDirectionZ {
        get { return _worldDirection.z; }
        set {
            if (value > 1) {
                _worldDirection.z = 1;
            } else if (value < -1) {
                _worldDirection.z = -1;
            } else {
                _worldDirection.z = value;
            }
        }
    }

    public Vector3 WorldDirection {
        get { return _worldDirection; }
        set {
            WorldDirectionX = value.x;
            WorldDirectionY = value.y;
            WorldDirectionZ = value.z;
        }
    }
}
