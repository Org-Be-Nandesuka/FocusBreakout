using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelButtons;
    
    void OnEnable()
    {
        int levelsUnlocked = SaveManager.GetMainLevelsCompleted();

        for(int i = 0; i < levelsUnlocked; i++) {
            levelButtons[i].SetActive(true);
        }
    }
}
