using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlobManager : MonoBehaviour {
/*    [Header("Blob Prefabs")]
    [SerializeField] private BlinkingBlob _blinkingBlob;
    [SerializeField] private MovingBlob _movingBlob;

    [Header("Blob Spawn Settings")]
    [SerializeField] private int _maxBlobs;
    // Will spawn blobs based on their BlobSpawn size or give
    // each BlobSpawn equal probability regardless of size.
    [SerializeField] private bool _spawnProbabilityBySize;
    [SerializeField] private BlobSpawn[] _spawnArray;

    [Header("Moving Blob Settings")]
    // True if blob has a 50% chance of moving in the opposite direction when spawned
    [SerializeField] bool _isMoveDirectionReversible;
    // Each element in array has an equal chance for the blob to move in said direction
    [SerializeField] Vector3[] _moveDirectionArray;

    private static List<GameObject> _blobList;
    // # of differnt blob types that can be spawned
    private const int _numDifferentBlobs = 2;
    private float _totalSpawnVolume;

    void Start() {
        CheckBlobSpawns();
        _blobList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Blob"));
        foreach (BlobSpawn spawn in _spawnArray) {
            _totalSpawnVolume += spawn.Volume;
        }
    }

    private void Update() {
        // Spawn blobs if possible
        if (_blobList.Count < _maxBlobs) {
            int num = Random.Range(0, _numDifferentBlobs);
            Vector3 location;

            if (_spawnProbabilityBySize) {
                location = GetRandomLocationBySize();
            } else {
                location = GetRandomLocationByCount();
            }

            switch (num) {
                case 0:
                    _blobList.Add(Instantiate(_blinkingBlob, location, Quaternion.identity).gameObject);
                    break;
                case 1:
                    MovingBlob blob = Instantiate(_movingBlob, location, Quaternion.identity);
                    int numDir = Random.Range(0, _moveDirectionArray.Length);
                    Vector3 moveDir = _moveDirectionArray[numDir];

                    if (_isMoveDirectionReversible && Random.Range(0, 2) == 0) {
                        moveDir *= -1;
                    }

                    blob.WorldDirection = moveDir;
                    _blobList.Add(blob.gameObject);
                    break;
                default:
                    Debug.LogError("BlobManager spawn error. Random int(" + num + ") outside of switch statement.");
                    break;
            }
        }
    }

    // Gives a random location where the probability of a blob spawn
    // is based on total volume of blob spawns
    private Vector3 GetRandomLocationBySize() {
        float num = Random.Range(0, _totalSpawnVolume);
        float volume = 0;
        BlobSpawn blobSpawn = null;

        foreach (BlobSpawn spawn in _spawnArray) {
            volume += spawn.Volume;

            if (num <= volume) {
                blobSpawn = spawn;
                break;
            }
        }

        if (blobSpawn == null) {
            throw new ArgumentException("Exception in BlobManager's GetRandomLocationBySize()\n" +
                "Random volume number: " + num + "\n" +
                "Total volume: " + _totalSpawnVolume);
        } else {
            return GetRandomVector3(blobSpawn.LowerBound, blobSpawn.UpperBound);
        }
    }

    // Gives a random location where every 'BlobSpawn' has an equal
    // chance regardless of size
    private Vector3 GetRandomLocationByCount() {
        int idx = Random.Range(0, _spawnArray.Length);
        Vector3 upperBound = _spawnArray[idx].UpperBound;
        Vector3 lowerBound = _spawnArray[idx].LowerBound;

        return GetRandomVector3(lowerBound, upperBound);
    }

    private Vector3 GetRandomVector3(Vector3 lowerBound, Vector3 upperBound) {
        float x = Random.Range(lowerBound.x, upperBound.x);
        float y = Random.Range(lowerBound.y, upperBound.y);
        float z = Random.Range(lowerBound.z, upperBound.z);
        return new Vector3(x, y, z);
    }

    public static int GetBlobCount() {
        return _blobList.Count;
    }

    public static void AddBlob(GameObject blob) {
        _blobList.Add(blob);
    }

    public static void RemoveBlob(Blob blob) {
        _blobList.Remove(blob.gameObject);
    }

    // Ensures that SpawnArray and MoveDirectionArray isn't empty,
    // otherwise throws an excpetion.
    private void CheckBlobSpawns() {
        if (_spawnArray.Length == 0) {
            throw new ArgumentException("SpawnArray in BlobManager is empty.\n" +
                "Please use a BlobSpawn prefab to specify where blobs can spawn.");
        } else if (_moveDirectionArray.Length == 0) {
            throw new ArgumentException("MoveDirectionArray in BlobManager is empty.\n" +
                "Please specify Vector3 directions for spawning moving blobs.");
        }
    }

    // Beta
    IEnumerator Run() {
        yield return new WaitForSeconds(5);
        print("run");
        _maxBlobs = 5000;
    }

    // Ensures Vector3 MoveDirection is between -1 and 1.
    private void OnValidate() {
        for (int i = 0; i < _moveDirectionArray.Length; i++) {
            if (_moveDirectionArray[i].x < -1) {
                _moveDirectionArray[i].x = -1;
            } else if (_moveDirectionArray[i].y < -1) {
                _moveDirectionArray[i].y = -1;
            } else if (_moveDirectionArray[i].z < -1) {
                _moveDirectionArray[i].z = -1;
            } else if (_moveDirectionArray[i].x > 1) {
                _moveDirectionArray[i].x = 1;
            } else if (_moveDirectionArray[i].y > 1) {
                _moveDirectionArray[i].y = 1;
            } else if (_moveDirectionArray[i].z > 1) {
                _moveDirectionArray[i].z = 1;
            }
        }
    }*/

    public static GameObject GetRandomBlob() {
        //int index = Random.Range(0, _blobList.Count);
        //return _blobList[index];
        return null;
    }
}
