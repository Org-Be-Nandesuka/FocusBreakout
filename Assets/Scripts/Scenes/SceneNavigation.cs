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
    public void WarningNewGame()
    {
        // Check if save file exists
        if (File.Exists(SaveManager._path))
        {
            Debug.Log("Are you sure you want to start a new game?");
            //GameObject popUpCanvas = GameObject.Find("PopUpCanvas");
            popUpCanvas.SetActive(true);
        } 
        else {
            LoadLevel(0);
        }
    }

    public void StartNewGame() {
        // Delete save file
        if (File.Exists(SaveManager._path)) { 
            File.Delete(SaveManager._path);
        }
        popUpCanvas.SetActive(false);
        LoadLevel(0);
    }

    public void DismissWarning() {
        popUpCanvas.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
