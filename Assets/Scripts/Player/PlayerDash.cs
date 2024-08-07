using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float DashSpeed;
    public float DashTime;

    private CharacterController _characterController; //For adding velocity to the character
    private PlayerController _playerController; //For grabbing movement velocity
    private PlayerInput _playerInput; 
    private InputAction _dash; //Input action binded to Shift
    private float _dashCooldown = 3.0f;
    private float _dashCooldownTime;

    void Start() {
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();
        _playerInput = GetComponent<PlayerInput>();
        _dash = _playerInput.actions["Dash"];
    }

    void Update() {
        if (_dash.triggered && Time.time > _dashCooldownTime) {
            _dashCooldownTime = Time.time + _dashCooldown;
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash() {
        float startTime = Time.time;

        while (Time.time < startTime + DashTime) { 
            _characterController.Move(_playerController.Move * DashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
