using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour {

    public GameManager GameManager;
    public GameObject CollectKeyText;

    public GameObject Timer;

    private float _timeToAppear = 4f;
    private float _timeWhenDisappear;
    private Animator _animator;

    private void OnTriggerEnter(Collider other) {
        if (GameManager.KeyCollected) {
            Win();
        } else {
            EnableText(); //Enable UI text prompting to collect key
        }
    }

    public void EnableText() {
        CollectKeyText.SetActive(true);
        _timeWhenDisappear = Time.time + _timeToAppear; // Calulcate Time when message should disappear
        _animator = CollectKeyText.GetComponent<Animator>();
        _animator.SetTrigger("Fade");
    }

    private void Win() {
        StopAllCoroutines();
        GameManager.SaveLevelData();
        DataManager.CurrentScore = GameManager.GetScore();

        Cursor.lockState = CursorLockMode.Confined;
        
        if (DataManager.CurrentLevel == 3) {
            SceneManager.LoadScene("Ending");
        } else {
            SceneManager.LoadScene("GameWin"); //Switch to GameWinScene
        }
    }
}
