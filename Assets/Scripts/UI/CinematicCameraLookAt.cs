using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraLookAt : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField] bool _active = true;
    [SerializeField] float _rotationSpeed = 1.0f;

    // Update is called once per frame
    void FixedUpdate() {
        if (_active) {
            Vector3 direction = _target.transform.position - transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
