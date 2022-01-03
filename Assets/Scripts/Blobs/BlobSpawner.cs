using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a 3D space where blobs can spawn. This spawner accounts for blob radius to 
/// ensure the blobs spawning near or on the face of the cube do not have their body poke out.
/// Only exception is if the Blob's scale is larger than that of the cube on any of the axes.
/// 
/// <para>
/// Blob Spawner's scale cannot be negative and its rotation must be (0, 0, 0).
/// </para>
/// 
/// <para>
/// (Blob's Lifespan) / (Spawn Rate) should be at least Max Blobs.
/// </para>
/// 
/// <para>
/// The translucent blue cube seen in Scene View will not be seen in Game View.
/// </para>
/// </summary>
public class BlobSpawner : MonoBehaviour {
    [SerializeField] private BasicBlob _blob;
    [SerializeField] private int _maxBlobs;
    [SerializeField] private float _spawnRate;
    [SerializeField] private BlobBehavior[] _blobBehaviorArray;

    private Vector3 _upperBound;
    private Vector3 _lowerBound;
    private ObjectPool<BasicBlob> _blobPool;

    void Start() {
        CheckTransform();
        if (_blobBehaviorArray.Length == 0) {
            throw new ArgumentException("Blob Behavior Array cannot be empty.");
        }

        Vector3 position = transform.position;
        Vector3 scale = transform.localScale;
        float blobRadius = _blob.transform.localScale.y / 2f;

        _upperBound = GetUpperBound(position, scale, blobRadius);
        _lowerBound = GetLowerBound(position, scale, blobRadius);
        _blobPool = new ObjectPool<BasicBlob>(PoolObjectCreate, PoolObjectGet, PoolObjectRelease,
            maxSize: _maxBlobs);

        // Initiates Blobs
        for (int i = 0; i < _maxBlobs; i++) {
            _blobPool.Get();
        }
        StartCoroutine(SpawnBlobCoroutine(_spawnRate));
    }

    IEnumerator SpawnBlobCoroutine(float spawnRate) {
        while (true) {
            yield return new WaitForSeconds(spawnRate);

            if (_blobPool.CountInactive > 0) {
                _blobPool.Get();
            }
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

    /// <summary>
    /// When a game object is first instantiated so it can be placed in a pool.
    /// </summary>
    private BasicBlob PoolObjectCreate() {
        BasicBlob blob = Instantiate(_blob);
        blob.BlobPool = _blobPool;
        return blob;
    }

    /// <summary>
    /// When a game object is returned back to its pool.
    /// </summary>
    private void PoolObjectRelease(BasicBlob blob) {
        blob.gameObject.SetActive(false);
    }

    /// <summary>
    /// When a game object is taken from its pool.
    /// </summary>
    private void PoolObjectGet(BasicBlob blob) {
        blob = SetUpBlob(blob);
        blob.gameObject.SetActive(true);
    }

    /// <summary>
    /// Gives a blob a new random location and behavior.
    /// </summary>
    private BasicBlob SetUpBlob(BasicBlob blob) {
        Vector3 location = GetRandomVector3(_lowerBound, _upperBound);
        int idx = Random.Range(0, _blobBehaviorArray.Length);

        blob.transform.position = location;
        blob.BasicBlobBehavior = _blobBehaviorArray[idx];
        return blob;
    }

    /// <summary>
    /// Ensures proper transform values and should be trhe first method called.
    /// </summary>
    private void CheckTransform() {
        if (transform.localScale.x < 0 || 
            transform.localScale.y < 0 || 
            transform.localScale.z < 0) {
            throw new ArgumentException("BlobSpawner's scale cannot be negative.");
        }

        if (transform.rotation.x != 0 || 
            transform.rotation.y != 0 || 
            transform.rotation.z != 0) {
            throw new ArgumentException("BlobSpawner's rotation must be (0,0,0).");
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
