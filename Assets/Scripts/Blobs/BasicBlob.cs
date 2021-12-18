using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BasicBlob : Blob {
    [SerializeField] private Audio[] _audioArray;
    [SerializeField] private BlobBehavior _blobData;

    private ObjectPool<BasicBlob> _blobPool;
    private Vector3 _worldDirection;
    private CharacterController _controller;
    private Coroutine _currentCoroutine;

    void Start() {
        //_blobData = new BlobBehavior();
        _blobData.CheckData();
        _controller = GetComponent<CharacterController>();

        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }
    }

    void FixedUpdate() {
        _controller.Move(_worldDirection * _blobData.Speed * Time.fixedDeltaTime);
    }

    void OnEnable() {
        // Goes to default Blob Behavior (prefab)
        if (_blobData == null) {
            return;
        }

        if (_blobData.Lifespan != 0) {
            StartCoroutine(LifespanCoroutine());
        }

        ResetCurrentHealth();
        _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
        _worldDirection = _blobData.MoveDirection;
        transform.rotation = Quaternion.Euler(_blobData.FacingDirection);
    }

    IEnumerator LifespanCoroutine() {
        yield return new WaitForSeconds(_blobData.Lifespan);
        Die();
    }

    IEnumerator ReverseDirectionCoroutine() {
        yield return new WaitForSeconds(_blobData.DirectionChangeTime);
        ReverseDirection();
    }

    private void ReverseDirection() {
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
        _worldDirection *= -1;
    }

    protected override void Die() {
        _blobPool.Release(this);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        ReverseDirection();
    }

    public BlobBehavior BlobData {
        get { return _blobData; }
        set {
            value.CheckData();
            _blobData = value;
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
