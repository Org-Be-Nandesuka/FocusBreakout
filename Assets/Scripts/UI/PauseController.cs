using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {
    [SerializeField] private GameObject _pauseMenu;

    private PlayerInput _playerInput;
    private InputAction _pauseAction;
    private bool _isPaused;

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        _pauseAction = _playerInput.actions["Pause"];
        _isPaused = false;
    }

    private void Update() {
        if (_pauseAction.triggered) {
            if (_isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void Resume() {
        Cursor.lockState = CursorLockMode.Locked;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f; // resume the game
        _isPaused = false;
    }

    private void Pause() {
        Cursor.lockState = CursorLockMode.Confined;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f; // pause the game
        _isPaused = true;
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
    }
}
