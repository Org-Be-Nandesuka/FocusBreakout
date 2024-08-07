using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the movement and behaviour of an enemy Searchlight.
/// 
/// <para>
/// Script should be attached to a GameObject with NavMeshAgent and DamagePerInterval components.
/// </para>
/// </summary>
public class SearchlightController : MonoBehaviour {
    // The child Spotlight object.
    [SerializeField] private Transform _searchlight;

    // List of positions along the Searchlight's patrol path
    [SerializeField] private Transform[] _path;

    // Number of seconds Searchlight waits at each point
    [SerializeField] private float _delay = 1f;

    private NavMeshAgent _agent;    // Agent that controls the Searchlight's movement
    private DamagePerSeconds _dps;  // Responsible for dealing damage to player when detected
    private int _destinationPoint;  // Destination represented as the corresponding index in _path
    private Light _lightEmitter;    // Light component attached to the searchlight object
    private bool _isDelayed;

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _dps = GetComponent<DamagePerSeconds>();
        _destinationPoint = 0;  // Set destination to first point in the _path
        _lightEmitter = _searchlight.GetComponent<Light>();
        ValidatePath();
    }

    void Update() {
        if (!_agent.hasPath && !_isDelayed) {
            StartCoroutine(MoveToNextPoint());
        }
    }

    private void FixedUpdate() {
        Search();
    }


    /// <summary>
    /// Send out a SphereCast to detect if the player is standing
    /// in the searchlight.
    /// </summary>
    private void Search() {
        // Cast SphereCast relative to _searchlight's position
        Vector3 origin = _searchlight.position;
        Vector3 dir = _searchlight.forward;

        // Match length of ray to dist. of light from floor
        float castLen = Math.Abs(_lightEmitter.range);
        // Calculate radius of circular light on floor (bottom of spot light's cone)
        float radius = (Mathf.Tan(_lightEmitter.spotAngle * Mathf.Deg2Rad) * castLen) - 1f;
        RaycastHit hit;

        if (Physics.SphereCast(origin, radius, dir, out hit, castLen)) {
            if (hit.collider.CompareTag("Blob")) {
                FollowPlayer(hit.transform.position);

                if (!_dps.DealingDamage) {
                    _dps.StartApplyingDamage(hit.transform.GetComponent<Player>());
                }
            } else {
                _dps.StopApplyingDamage();
            }
        }
    }

    private void FollowPlayer(Vector3 target) {
        _agent.SetDestination(target);
    }

    /// <summary>
    /// Move the Searchlight to the next point in the _path array.
    /// </summary>
    private IEnumerator MoveToNextPoint() {
        _isDelayed = true;
        yield return new WaitForSeconds(_delay);
        _isDelayed = false;

        _agent.SetDestination(_path[_destinationPoint].position);
        // Set dest to next point in path, or cycle back to the start
        _destinationPoint = (_destinationPoint + 1) % _path.Length;
    }

    private void ValidatePath() {
        if (this.enabled && _path.Length == 0) {
            _path = new Transform[] { GetComponent<Transform>() };
            throw new ArgumentException($"Path array in for {name} cannot be empty");
        }
    }
}
