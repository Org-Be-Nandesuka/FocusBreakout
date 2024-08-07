using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextWriter : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _timePerChar;
    [SerializeField] private float _timeUntilText;
    [SerializeField] private float _timeUntilBSOD;
    [SerializeField] private float _timeUntilMenu;

    private GameObject _bsodImg;
    private int charIndex;
    private string originalText;
    private float _highScore;
    private float _currScore;

    private void Awake() {
        if (_textMesh == null) {
            throw new ArgumentException("TextMesh component is null");
        }

        _bsodImg = GameObject.Find("BSODImage");
        _bsodImg.SetActive(false); ;

        LevelData[] levelData = SaveManager.LoadData();
        _highScore = levelData[DataManager.CurrentLevel].HighScore;
        _currScore = DataManager.CurrentScore;

        if (_currScore > _highScore) {
            levelData[DataManager.CurrentLevel].HighScore = _currScore;
            SaveManager.SaveData(levelData);
            _highScore = _currScore;
        }

        string preMsg = "Level Completed!\nScore:" + _currScore + "\nHighScore: " + _highScore + "\n";

        charIndex = 0;
        originalText = preMsg + _textMesh.text;
        _textMesh.text = "";
        StartCoroutine(WriteCoroutine());
    }

    IEnumerator WriteCoroutine() {
        AudioManager.Instance.Stop("BackgroundMusic");
        yield return new WaitForSeconds(_timeUntilText);
        AudioManager.Instance.PlayDelayed("TextTyping", _timePerChar);

        while (charIndex < originalText.Length) {
            yield return new WaitForSeconds(_timePerChar);
            charIndex++;
            _textMesh.text = originalText.Substring(0, charIndex);

            // There is a tiny gap before the BSOD audio so I need to player it a tad bit earlier
            if (charIndex == originalText.Length - 3) {
                AudioManager.Instance.Play("BSOD");
                AudioManager.Instance.Stop("TextTyping");
            }
        }

        yield return new WaitForSeconds(_timeUntilBSOD);
        _bsodImg.SetActive(true);

        yield return new WaitForSeconds(_timeUntilMenu);
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.Play("BackgroundMusic");
        AudioManager.Instance.Stop("BSOD");
    }

    private void OnValidate() {
        if (_timePerChar < 0) {
            _timePerChar = 0;
        }

        if (_timeUntilText < 0) {
            _timeUntilText = 0;
        }

        if (_timeUntilBSOD < 0) {
            _timeUntilBSOD = 0;
        }

        if (_timeUntilMenu < 0) {
            _timeUntilMenu = 0;
        }
    }
}

