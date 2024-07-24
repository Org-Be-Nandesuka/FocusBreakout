using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidateButton : MonoBehaviour
{
    [SerializeField] private string _originalText;
    void OnEnable()
    {
        Debug.Log(SaveManager._path);
        if(File.Exists(SaveManager._path)) { 
            this.gameObject.GetComponent<Button>().interactable = true;
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            this.gameObject.GetComponentInChildren<TextGlitchEffect>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _originalText;
            this.gameObject.GetComponentInChildren<TextGlitchEffect>().enabled = false;
        }
    }
}
