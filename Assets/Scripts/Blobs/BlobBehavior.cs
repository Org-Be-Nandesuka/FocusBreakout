using System;
using UnityEngine;

/// <summary>
/// Represents the movement behavior of a Blob. made to be used in conjunction with
/// Blob Spawner for an array of different behaviors that a blob can be spawned with.
/// </summary>
[Serializable]
public class BlobBehavior : ISerializationCallbackReceiver {
    // Time until automatic death, 0 = they won't die based on time.
    [SerializeField] private float _lifespan; 
    // 0 = no movement.
    [SerializeField] private float _speed; 
    // Time until they flip their movement direction, 0 = only change direction from collisions.
    [SerializeField] private float _directionChangeTime;
    // (0, 0, 0) = no movement
    [SerializeField] private Vector3 _moveDirection;
    // Overrides Facing Direction and will face the direction they are moving in.
    [SerializeField] private bool _faceMoveDirection;
    // (0, 0, 0) will be changed to (0, 0, 1)
    [SerializeField] private Vector3 _facingDirection;

    // min/max values for normalized Vector
    private const float _minVecVal = -1;
    private const float _maxVecVal = 1;

    #region PossibleFutureImplementation
    // Creating custom BlobBehaviors inside a script
    /*    public BlobBehavior(float lifespan, float speed, float dirChangeTime,
            Vector3 moveDir, bool faceMoveDir, Vector3 faceDir) {
            _lifespan = lifespan;
            _speed = speed;
            _directionChangeTime = dirChangeTime;
            _moveDirection = moveDir;
            _faceMoveDirection = faceMoveDir;
            _facingDirection = faceDir;

            CheckData();
        }

    // Validate data when making a new instance
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
        }*/
    #endregion

    public BlobBehavior() {
        _lifespan = 0.01f;
        _speed = 0;
        _directionChangeTime = 0;
        _moveDirection = Vector3.zero;
        _faceMoveDirection = false;
        _facingDirection = Vector3.forward;
    }

    private bool CheckVector3(Vector3 v, float min = _minVecVal, float max = _maxVecVal) {
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

    public bool FaceMoveDirection {
        get { return _faceMoveDirection; }
    }

    public Vector3 FacingDirection {
        get { 
            if (_facingDirection == Vector3.zero) {
                _facingDirection = Vector3.forward;
            }
            return _facingDirection; 
        }
    }

    public void OnBeforeSerialize() {
        OnValidate();
    }

    public void OnAfterDeserialize() { }

    public void OnValidate() {
        if (_lifespan < 0) {
            _lifespan = 0;
        } else if (_speed < 0) {
            _speed = 0;
        } else if (_directionChangeTime < 0) {
            _directionChangeTime = 0;
        // Validate Move Direction
        } else if (_moveDirection.x < _minVecVal || _moveDirection.x > _maxVecVal) {
            _moveDirection.x = Mathf.Clamp(_moveDirection.x, _minVecVal, _maxVecVal);
        } else if (_moveDirection.y < _minVecVal || _moveDirection.y > _maxVecVal) {
            _moveDirection.y = Mathf.Clamp(_moveDirection.y, _minVecVal, _maxVecVal);
        } else if (_moveDirection.z < _minVecVal || _moveDirection.z > _maxVecVal) {
            _moveDirection.z = Mathf.Clamp(_moveDirection.z, _minVecVal, _maxVecVal);
        // Validate Facing Direction
        } else if (_facingDirection.x < _minVecVal || _facingDirection.x > _maxVecVal) {
            _facingDirection.x = Mathf.Clamp(_facingDirection.x, _minVecVal, _maxVecVal);
        } else if (_facingDirection.y < _minVecVal || _facingDirection.y > _maxVecVal) {
            _facingDirection.y = Mathf.Clamp(_facingDirection.y, _minVecVal, _maxVecVal);
        } else if (_facingDirection.z < _minVecVal || _facingDirection.z > _maxVecVal) {
            _facingDirection.z = Mathf.Clamp(_facingDirection.z, _minVecVal, _maxVecVal);
        }
    }
}
