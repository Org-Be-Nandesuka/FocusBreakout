using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

// Represents a 3D space where blobs can spawn.
// BlobSpawn's scale cannot be negative and its rotation must be (0,0,0).
// Please keep in mind that parts of a blob may spawn outside of these bounds
// becuase a blob's spawn point does not account for its radius. 
public class BlobSpawnManager : MonoBehaviour {
    [Header("Blob Prefabs")]
    [SerializeField] private Blob _blob;
    // Each element in array has an equal chance for the blob to move in said direction
    [SerializeField] Vector3[] _moveDirectionArray;
    [SerializeField] private int _maxBlobs;
    [SerializeField] private float _spawnRate;

    public int active;
    public int inactive;
    public int all;

    private float _volume;
    private Vector3 _position;
    private Vector3 _scale;
    private Vector3 _upperBound;
    private Vector3 _lowerBound;
    private int _blobCount;
    private ObjectPool<Blob> _blobPool;

    void Start() {
        CheckTransform();
        _position = transform.position;
        _scale = transform.localScale;
        _volume = _scale.x * _scale.y * _scale.z;
        _blobCount = 1; // Accounts for player
        _upperBound = GetUpperBound();
        _lowerBound = GetLowerBound();

        _blobPool = new ObjectPool<Blob>(PoolObjectCreate, PoolObjectGet, PoolObjectRelease, maxSize: _maxBlobs);
        for (int i = 0; i < _maxBlobs; i++) {
            _blobPool.Get();
        }
        StartCoroutine(SpawnBlobCoroutine(_spawnRate));
    }

    private void Update() {
        active = _blobPool.CountActive;
        inactive = _blobPool.CountInactive;
        all = _blobPool.CountAll;
    }

    IEnumerator SpawnBlobCoroutine(float spawnRate) {
        while (true) {
            if (_blobPool.CountInactive > 0) {
                _blobPool.Get();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    // Should be called during Start()/Awake()
    // Gets the north "upper-right" vertex
    private Vector3 GetUpperBound() {
        float x = _position.x + _scale.x / 2f;
        float y = _position.y + _scale.y / 2f;
        float z = _position.z + _scale.z / 2f;
        return new Vector3(x, y, z);
    }

    // Should be called during Start()/Awake()
    // Gets the south "bottom-left" vertex
    private Vector3 GetLowerBound() {
        float x = _position.x - _scale.x / 2f;
        float y = _position.y - _scale.y / 2f;
        float z = _position.z - _scale.z / 2f;
        return new Vector3(x, y, z);
    }

    private Vector3 GetRandomVector3(Vector3 lowerBound, Vector3 upperBound) {
        float x = Random.Range(lowerBound.x, upperBound.x);
        float y = Random.Range(lowerBound.y, upperBound.y);
        float z = Random.Range(lowerBound.z, upperBound.z);
        return new Vector3(x, y, z);
    }

    private Blob PoolObjectCreate() {
        Vector3 location = GetRandomVector3(_lowerBound, _upperBound);
        int num = Random.Range(0, 2);
        Blob blob = Instantiate(_blob, location, Quaternion.identity);
        blob.BlobPool = _blobPool;
        return blob;
    }

    private void PoolObjectGet(Blob blob) {
        blob.gameObject.SetActive(true);
        Vector3 location = GetRandomVector3(_lowerBound, _upperBound);
        blob.transform.position = location;
    }

    private void PoolObjectRelease(Blob blob) {
        blob.gameObject.SetActive(false);
    }

    // Should be the first method that's called in this class
    // Ensures proper transform values
    private void CheckTransform() {
        if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0) {
            throw new ArgumentException("BlobSpawn's scale cannot be negative.");
        }

        if (transform.rotation.x != 0 || transform.rotation.y != 0 || transform.rotation.z != 0) {
            throw new ArgumentException("BlobSpawn's rotation must be (0,0,0).");
        }
    }

    public Vector3 Position {
        get { return _position; }
    }

    public Vector3 Scale {
        get { return _scale; }
    }

    public float Volume {
        get { return _volume; }
    }

    public Vector3 UpperBound {
        get { return _upperBound; }
    }

    public Vector3 LowerBound {
        get { return _lowerBound; }
    }

    private void OnValidate() {
        if (_maxBlobs < 1) {
            _maxBlobs = 1;
        }

        if (_spawnRate < 0) {
            _spawnRate = 0;
        }
    }
}
