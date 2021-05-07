using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : ListMenuController
{
    public delegate void SelectDifficultyMenu();
    public static event SelectDifficultyMenu OnOpenDifficultyMenu;

    private void OnEnable()
    {
        setDefaultFocusOn(GameObject.Find("Resume"));
    }

    public void ExitGame()
    {
        GameController.ExitGame();
    }

    public void OpenDifficultyMenu()
    {
        if (OnOpenDifficultyMenu == null)
        {
            Debug.Log("no subscribers on event OnOpenDifficultyMenu");
        }
        else
        {
            OnOpenDifficultyMenu();
        }
    }
}
