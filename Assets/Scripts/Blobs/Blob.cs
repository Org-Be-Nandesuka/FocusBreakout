using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Blob : MonoBehaviour {
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifespan;
    [SerializeField] private Audio[] _audioArray;

    private int _currentHealth;
    private ObjectPool<Blob> _blobPool;
    private const float MinPositiveFloat = 0.00001f;

    void Awake() {
        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
        }

        _currentHealth = _maxHealth;
    }

    void OnEnable() {
        StartCoroutine(LifespanCoroutine());
    }

    IEnumerator LifespanCoroutine() {
        yield return new WaitForSeconds(_lifespan);
        Die();
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

    public int CurrentHealth {
        get { return _currentHealth; }
    }

    public int MaxHealth {
        get { return _maxHealth; }
    }

    public float Speed {
        get { return _speed; }
    }

    public float Lifespan {
        get { return _lifespan; }
        set { 
            if (value < MinPositiveFloat) {
                throw new ArgumentException("Blob lifespan cannot be less than " + MinPositiveFloat);
            } else {
                _lifespan = value;
            }
        }
    }

    public ObjectPool<Blob> BlobPool {
        get { return _blobPool; }
        set { _blobPool = value; }
    }

    protected virtual void OnValidate() {
        if (_maxHealth < 1) {
            _maxHealth = 1;
        }

        if (_speed < 0) {
            _speed = 0;
        }

        if(_lifespan < MinPositiveFloat) {
            _lifespan = MinPositiveFloat;
        }
    }
}
