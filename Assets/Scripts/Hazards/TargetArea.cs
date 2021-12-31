using System;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Contains an array of all the game objects with the layer "Target" within its cube.
/// Scale axes of this game object must be positive or an exception will be thrown.
/// </summary>
public class TargetArea : MonoBehaviour {
    private RaycastHit[] _hitArray;
    private Vector3 _halfScale;
    private int _layerMask;
    private Blob _selectedBlob;

    void Start() {
        if (transform.localScale.x <= 0 || 
            transform.localScale.y <= 0 || 
            transform.localScale.z <= 0) {
            throw new ArgumentException("Scale axes must be positive.");
        }

        _halfScale = transform.localScale / 2f;
        _layerMask = LayerMask.GetMask("Target");
    }

    void FixedUpdate() {
        // BoxCastAll's size parameter uses halfExtents for some reason, 
        // therefore our input size is divided by 2.
        _hitArray = Physics.BoxCastAll(transform.position, _halfScale, transform.forward,
            transform.rotation, 0, _layerMask);
    }


    public Blob GetRandomBlob() {
        if (_hitArray.Length == 0) {
            return null;
        }

        int idx = Random.Range(0, _hitArray.Length);
        return _hitArray[idx].collider.gameObject.GetComponent<Blob>();
    }

    public bool IsEmpty() {
        if (_hitArray.Length == 0) {
            return true;
        } else {
            return false;
        }
    }

/*    public Blob SelectedBlob {
        get { 
            if (Array.Exists(_hitArray, _selectedBlob)) {

            }
            return _selectedBlob; 
        }
    }*/

    public RaycastHit[] HitArray {
        get { return _hitArray; }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
