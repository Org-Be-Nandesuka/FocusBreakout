// Attach this script to a GameObject. Make sure it has a Collider component by clicking the Add 
// Component button. Then click Physics>Box Collider to attach a Box Collider component. This 
// script creates a BoxCast in front of the GameObject and outputs a message if another Collider 
// is hit with the Collider’s name. It also draws where the ray and BoxCast extends to. Just press
// the Gizmos button to see it in Play Mode. Make sure to have another GameObject with a Collider
// component for the BoxCast to collide with.

using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    [SerializeField] private float _scale;

    private float _maxDistance;
    private bool _hitDetect;
    private Vector3 _size;
    private Collider _collider;
    private RaycastHit _hit;

    void Start() {
        _maxDistance = Constants.MaxMapDistance;
        _collider = GetComponent<Collider>();
        _size = transform.localScale * _scale;
    }

    void FixedUpdate() {
        // Test to see if there is a hit using a BoxCast
        // Calculate using the center of the GameObject's Collider(could also just use the 
        // GameObject's position), half the GameObject's size, the direction, the GameObject's 
        // rotation, and the maximum distance as variables. Also fetch the hit data.
        _hitDetect = Physics.BoxCast(_collider.bounds.center, _size, transform.forward, out _hit, transform.rotation, _maxDistance);
        if (_hitDetect) {
            //Debug.Log("Hit : " + _hit.collider.name);
        }
    }

    // Draw the BoxCast as a gizmo to show where it currently is testing. 
    // Click the Gizmos button to see this. Use size / 2 cuz BoxCast be like dat.
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward, _size / 2);
    }

    private void OnValidate() {
        if (_scale < 1) {
            _scale = 1;
        }
    }
}
