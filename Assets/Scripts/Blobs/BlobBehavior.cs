using System;
using UnityEngine;

/// <summary>
/// Represents the movement behavior of a Blob. Mainly used in conjunction with
/// Blob Spawner for an array of different behaviors that a blob can be spawned with.
/// </summary>
[Serializable]
public class BlobBehavior {
    [SerializeField] private float _lifespan;
    [SerializeField] private float _speed;
    [SerializeField] private float _directionChangeTime;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private Vector3 _facingDirection;

    public BlobBehavior() {
        _lifespan = 1f;
        _speed = 0;
        _directionChangeTime = 0;
        _moveDirection = Vector3.zero;
        _facingDirection = Vector3.zero;
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
