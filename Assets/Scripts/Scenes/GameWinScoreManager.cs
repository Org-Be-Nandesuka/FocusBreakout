using TMPro;
using UnityEngine;

public class GameWinScoreManager : MonoBehaviour {
    [SerializeField] private bool _isCurrentScore;

    public void UpdateText() {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        LevelData[] levelData = SaveManager.LoadData();
        float _highScore = levelData[DataManager.CurrentLevel].HighScore;
        float _currentScore = DataManager.CurrentScore;

        if (_currentScore > _highScore) {
            levelData[DataManager.CurrentLevel].HighScore = _currentScore;
            SaveManager.SaveData(levelData);
            _highScore = _currentScore;
        }

        if (_isCurrentScore) {
            text.text = "Score: " + _currentScore;
        } else {
            text.text = "High Score: " + _highScore;
        }
    }
}
