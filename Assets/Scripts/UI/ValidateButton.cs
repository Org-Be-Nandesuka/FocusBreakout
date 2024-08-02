using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidateButton : MonoBehaviour {
    [SerializeField] private string _originalText;

    void OnEnable() {
        if(File.Exists(SaveManager._path)) { 
            GetComponent<Button>().interactable = true;
            GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            GetComponentInChildren<TextGlitchEffect>().enabled = true;
        } else {
            GetComponent<Button>().interactable = false;
            GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            GetComponentInChildren<TextMeshProUGUI>().text = _originalText;
            GetComponentInChildren<TextGlitchEffect>().enabled = false;
        }
    }
}
