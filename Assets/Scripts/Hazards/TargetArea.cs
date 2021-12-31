using System;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Contains an array of all the game objects with the layer "Target" within its cube.
/// The Z scale of this game object cannot be negative or an exception will be thrown.
/// </summary>
public class TargetArea : MonoBehaviour {
    private RaycastHit[] _hitArray;
    private Vector3 _halfScale;
    private int _layerMask;

    void Start() {
        if (transform.localScale.z < 0) {
            throw new ArgumentException("Z scale cannot be negative.");
        }

        _halfScale = transform.localScale / 2f;
        _layerMask = LayerMask.GetMask("Target");
    }

    void FixedUpdate() {
        // BoxCastAll's size parameter uses halfExtents for some reason, 
        // therefore our input size is divided by 2.
        _hitArray = Physics.BoxCastAll(transform.position, _halfScale, transform.forward,
            transform.rotation, 0, _layerMask);
        print(_hitArray.Length);
    }

    public RaycastHit[] HitArray {
        get { return _hitArray; }
    }

    public Blob GetRandomBlob() {
        if (_hitArray.Length == 0) {
            return null;
        }

        int idx = Random.Range(0, _hitArray.Length);
        return _hitArray[idx].collider.gameObject.GetComponent<Blob>();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
