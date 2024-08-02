using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour {

    [SerializeField] GameObject popUpCanvas;
    // Play button
    public void LoadHighestLevel() {
        LoadLevel(SaveManager.GetMainLevelsCompleted());
    }

    // Next level button
    public void LoadNextLevel() {
        LoadLevel(DataManager.CurrentLevel + 1);
    }

    // Play again button
    public void LoadCurrentLevel() {
        LoadLevel(DataManager.CurrentLevel);
    }

    private void LoadLevel(int level) {
        if (level >= Constants.NumOfMainLevels) {
            level = Constants.NumOfMainLevels - 1;
        }
        LoadScene("Level" + level);
    }

    public void LoadScene(string name) {
        StopAllCoroutines();
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(name);
    }

    // Give a warning before starting a New Game
    public void WarningNewGame(Animator dollyCartAnim)
    {
        // Check if save file exists
        if (File.Exists(SaveManager._path)) {
            popUpCanvas.SetActive(true);
        } else {
            // Disable main menu canvas
            var mainMenuCanvas = GameObject.Find("MainMenuCanvas");
            mainMenuCanvas.SetActive(false);

            dollyCartAnim.SetTrigger("StartNewGame"); // Play animation!
        }
    }

    public void StartNewGame(Animator dollyCartAnim) {
        // Delete save file
        if (File.Exists(SaveManager._path)) { 
            File.Delete(SaveManager._path);
        }

        popUpCanvas.SetActive(false);

        // Disable main menu canvas
        var mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        mainMenuCanvas.SetActive(false);

        dollyCartAnim.SetTrigger("StartNewGame"); // Play animation!
    }

    public void DismissWarning() {
        popUpCanvas.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
