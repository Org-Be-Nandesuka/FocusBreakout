using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
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
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "CONTINUE";
            this.gameObject.GetComponentInChildren<TextGlitchEffect>().enabled = false;
        }
    }
}
