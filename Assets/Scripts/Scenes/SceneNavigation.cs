using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour {
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
    public void WarningNewGame()
    {
        LoadLevel(0);
        Debug.Log("TEST");
        Debug.Log(Application.persistentDataPath);
        print(Application.persistentDataPath);

        // Check if save file exists
        if (File.Exists(SaveManager._path)) {
            
            Debug.Log("Are you sure you want to start a new game?");
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
