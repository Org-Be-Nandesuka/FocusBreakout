using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float _jumpHeight;
    [SerializeField] private GameObject _moveEffectPrefab; // particle effect when moving

    private CharacterController _controller;
    private PlayerInput _playerInput;
    private InputAction _movementAction;
    private InputAction _jumpAction;
    private Transform _cameraTransform;
    private Quaternion _targetRotation;
    private Vector3 _move;
    private Vector3 _playerVelocity;
    private float _playerSpeed;
    private bool _groundedPlayer;

    private void Start() {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerSpeed = GetComponent<Player>().Speed;
        _cameraTransform = Camera.main.transform;
        _movementAction = _playerInput.actions["Movement"];
        _jumpAction = _playerInput.actions["Jump"];

        _moveEffectPrefab = Instantiate(_moveEffectPrefab, transform.position, Quaternion.identity);
        _moveEffectPrefab.transform.parent = transform;
        _moveEffectPrefab.SetActive(false);
    }

    void Update() {
        // no idea what grounded or playerVelocity does yet.
        // grounded something to do with whether or not u touch object with "ground tag"
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0) {
            _playerVelocity.y = 0f;
        }

        // Player movement input
        Vector2 input = _movementAction.ReadValue<Vector2>();
        _move = input.x * _cameraTransform.right.normalized + input.y * _cameraTransform.forward.normalized;
        _move.y = 0f;

        // Player jump input
        if (_jumpAction.triggered && _groundedPlayer) {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -16.0f * Constants.Gravity);
        }
        _playerVelocity.y += -40f * Time.deltaTime;

        // Player mouse (camera look) input
        _targetRotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
    }

    private void FixedUpdate() {
        if (_move.z != 0) {
            _moveEffectPrefab.SetActive(true);
        } else {
            _moveEffectPrefab.SetActive(false);
        }
        _controller.Move(_move * Time.fixedDeltaTime * _playerSpeed);
        _controller.Move(_playerVelocity * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _playerSpeed * Time.fixedDeltaTime);
    }

    public Vector3 Move {
        get { return _move; }
    }

    private void OnValidate() {
        if (_jumpHeight < 0) {
            _jumpHeight = 0;
        }
    }
}