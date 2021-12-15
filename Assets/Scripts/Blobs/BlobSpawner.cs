using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a 3D space where blobs can spawn. This spawner accounts for blob radius to 
/// ensure the blobs spawning near or on the face of the cube do not poke out.
/// 
/// <para>
/// Blob Spawner's scale cannot be negative and its roation must be (0, 0, 0).
/// </para>
/// 
/// <para>
/// The translucent grey cube seen in Scene View will not be seen in Game View.
/// </para>
/// </summary>
public class BlobSpawner : MonoBehaviour {
    [SerializeField] private Blob _blob;
    [SerializeField] Vector3[] _moveDirectionArray;
    [SerializeField] private int _maxBlobs;
    [Tooltip("If the Spawn Rate is ")]
    [SerializeField] private float _spawnRate;

    private Vector3 _upperBound;
    private Vector3 _lowerBound;
    private ObjectPool<Blob> _blobPool;

    void Start() {
        CheckTransform();
        Vector3 position = transform.position;
        Vector3 scale = transform.localScale;
        float blobRadius = _blob.transform.localScale.y / 2f;

        _upperBound = GetUpperBound(position, scale, blobRadius);
        _lowerBound = GetLowerBound(position, scale, blobRadius);
        _blobPool = new ObjectPool<Blob>(PoolObjectCreate, PoolObjectGet, PoolObjectRelease, 
            maxSize: _maxBlobs);

        // Starts the spawner with Max Blobs
        for (int i = 0; i < _maxBlobs; i++) {
            _blobPool.Get();
        }
        StartCoroutine(SpawnBlobCoroutine(_spawnRate));
    }

    IEnumerator SpawnBlobCoroutine(float spawnRate) {
        while (true) {
            if (_blobPool.CountInactive > 0) {
                _blobPool.Get();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    /// <summary>
    /// Gets a random Vector3 between params Lower Bound and Upper Bound.
    /// </summary>
    private Vector3 GetRandomVector3(Vector3 lowerBound, Vector3 upperBound) {
        float x = Random.Range(lowerBound.x, upperBound.x);
        float y = Random.Range(lowerBound.y, upperBound.y);
        float z = Random.Range(lowerBound.z, upperBound.z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Gets the north "upper-right" vertex.
    /// </summary>
    /// 
    /// <param name="blobRadius">
    /// Accouunts for the blob's radius so their bodies won't poke out of the spawner.
    /// </param>
    private Vector3 GetUpperBound(Vector3 pos, Vector3 scale, float blobRadius) {
        float x = pos.x + scale.x / 2f - blobRadius;
        float y = pos.y + scale.y / 2f - blobRadius;
        float z = pos.z + scale.z / 2f - blobRadius;
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Gets the south "bottom-left" vertex
    /// </summary>
    /// 
    /// <param name="blobRadius">
    /// Accouunts for the blob's radius so their bodies won't poke out of the spawner.
    /// </param>
    private Vector3 GetLowerBound(Vector3 pos, Vector3 scale, float blobRadius) {
        float x = pos.x - scale.x / 2f + blobRadius;
        float y = pos.y - scale.y / 2f + blobRadius;
        float z = pos.z - scale.z / 2f + blobRadius;
        return new Vector3(x, y, z);
    }

    private Blob PoolObjectCreate() {
        Vector3 location = GetRandomVector3(_lowerBound, _upperBound);
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


    /// <summary>
    /// Ensures proper transform values and should be trhe first method called.
    /// </summary>
    private void CheckTransform() {
        if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0) {
            throw new ArgumentException("BlobSpawn's scale cannot be negative.");
        }

        if (transform.rotation.x != 0 || transform.rotation.y != 0 || transform.rotation.z != 0) {
            throw new ArgumentException("BlobSpawn's rotation must be (0,0,0).");
        }
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
