using System;
using UnityEngine;

/// <summary>
/// Represents the movement behavior of a Blob. made to be used in conjunction with
/// Blob Spawner for an array of different behaviors that a blob can be spawned with.
/// </summary>
[Serializable]
public class BlobBehavior {
    // How long until automatic death, 0 implies they won't die based on time.
    [SerializeField] [Min(0)] private float _lifespan; 
    // 0 implies no movement.
    [SerializeField] [Min(0)] private float _speed; 
    // Howe long until they change their movement direction, 
    // 0 implies they only change direction from collisions.
    [SerializeField] [Min(0)] private float _directionChangeTime;
    // (0, 0, 0) implies no movement
    [SerializeField] [Min(-1)] private Vector3 _moveDirection;
    // (0,0,0) will be automatically changed to (0, 0, 1) ie: "forward".
    [SerializeField] [Min(-1)] private Vector3 _facingDirection;

    public BlobBehavior(float lifespan, float speed, float dirChangeTime, Vector3 moveDir, Vector3 faceDir) {
        _lifespan = lifespan;
        _speed = speed;
        _directionChangeTime = dirChangeTime;
        _moveDirection = moveDir;
        _facingDirection = faceDir;

        CheckData();
    }

    public BlobBehavior() {
        _lifespan = 0.01f;
        _speed = 0;
        _directionChangeTime = 0;
        _moveDirection = Vector3.zero;
        _facingDirection = Vector3.forward;
    }

    /// <summary>
    /// TODO: OnValidate for non-monobehavior classes
    /// </summary>
    public void CheckData() {
        if (_lifespan < 0) {
            throw new ArgumentException("Lifespan cannot be less than 0.");
        } else if (_speed < 0) {
            throw new ArgumentException("Speed cannot be less than 0.");
        } else if (_directionChangeTime < 0) {
            throw new ArgumentException("Direction Change Time cannot be less than 0.");
        } else if (!CheckVector3(_moveDirection)) {
            throw new ArgumentException(
                "Move Direction values must be between -1 and 1 (both inclusive)");
        } else if (!CheckVector3(_facingDirection)) {
            throw new ArgumentException(
               "Facing Direction values must be between -1 and 1 (both inclusive)");
        } else if (_facingDirection == Vector3.zero) {
            _facingDirection = Vector3.forward;
        }
    }

    private bool CheckVector3(Vector3 v, float min = -1, float max = 1) {
        if (v.x < min || v.x > max || 
            v.y < min || v.y > max ||
            v.z < min || v.z > max) {
            return false;
        } else {
            return true;
        }
    }

    public float Lifespan {
        get { return _lifespan; }
    }

    public float Speed {
        get { return _speed; }
    }

    public float DirectionChangeTime {
        get { return _directionChangeTime; }
    }

    public Vector3 MoveDirection {
        get { return _moveDirection; }
    }

    public Vector3 FacingDirection {
        get { return _facingDirection; }
    }
}
