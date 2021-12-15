using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Blob : MonoBehaviour {
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _lifespan;
    [SerializeField] private float _speed;
    [SerializeField] private float _directionChangeTime;
    [Tooltip("Each axis is between 0 and 360 degrees")]
    [SerializeField] private Vector3 __facingDirection;

    [SerializeField] private Audio[] _audioArray;

    private int _currentHealth;
    private ObjectPool<Blob> _blobPool;


    public Vector3 _worldDirection;
    private CharacterController _controller;
    private Coroutine _currentCoroutine;

    void Awake() {
        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }

        _currentHealth = _maxHealth;
        transform.rotation = Quaternion.Euler(__facingDirection);

        _controller = GetComponent<CharacterController>();
        _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
    }

    void FixedUpdate() {
        _controller.Move(_worldDirection * _speed * Time.fixedDeltaTime);
    }

    void OnEnable() {
        if (_lifespan != 0) {
            StartCoroutine(LifespanCoroutine());
        }
    }

    IEnumerator LifespanCoroutine() {
        yield return new WaitForSeconds(_lifespan);
        Die();
    }

    private void ReverseDirection() {
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ReverseDirectionCoroutine());
        _worldDirection *= -1;
    }

    IEnumerator ReverseDirectionCoroutine() {
        yield return new WaitForSeconds(_directionChangeTime);
        ReverseDirection();
    }

    public virtual void TakeDamage(int dmg) {
        if (dmg <= 0) {
            throw new ArgumentException("Can't deal " + dmg + " damage, needs to be at least 0");
        }
        AudioSource.PlayClipAtPoint(_audioArray[1].Clip, transform.position, _audioArray[1].Volume);
        _currentHealth -= dmg;
        if (_currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Heal(int num) {
        if (num <= 0) {
            throw new ArgumentException("Can't heal " + num + ", needs to be at least 0");
        }
        _currentHealth += num;
        if (_currentHealth > _maxHealth) {
            _currentHealth = _maxHealth;
        }
    }

    protected virtual void Die() {
        if (Random.Range(0, 420) == 0) {
            AudioSource.PlayClipAtPoint(_audioArray[0].Clip, transform.position, _audioArray[0].Volume);
        }
        _blobPool.Release(this);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        ReverseDirection();
    }

    public int CurrentHealth {
        get { return _currentHealth; }
    }

    public int MaxHealth {
        get { return _maxHealth; }
    }

    public float Lifespan {
        get { return _lifespan; }
        set { 
            if (value < 0) {
                throw new ArgumentException("Blob lifespan cannot be less than 0");
            } else {
                _lifespan = value;
            }
        }
    }

    public Vector3 FacingDirection {
        get { return __facingDirection; }
        set { __facingDirection = value; }
    }

    public ObjectPool<Blob> BlobPool {
        get { return _blobPool; }
        set { _blobPool = value; }
    }

    public float Speed {
        get { return _speed; }
        set {
            if (value < 0) {
                throw new ArgumentException("MovingBlob speed cannot be negative.");
            }
            _speed = value;
        }
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

    protected virtual void OnValidate() {
        if (_maxHealth < 1) {
            _maxHealth = 1;
        }

        if(_lifespan < 0) {
            _lifespan = 0;
        }

        if (_directionChangeTime < 0) {
            _directionChangeTime = 0;
        }

        if (_speed < 0) {
            _speed = 0;
        }
    }
}
