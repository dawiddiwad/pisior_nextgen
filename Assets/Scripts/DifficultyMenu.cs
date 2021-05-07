using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyMenu : ListMenuController
{
    public delegate void GoBackToPauseMenu();
    public static event GoBackToPauseMenu OnGoBackToPauseMenu;
    private void OnEnable()
    {
        setDefaultFocusOn(GameObject.Find("Pussy"));
    }

    public void ShowPauseMenu()
    {
        if (OnGoBackToPauseMenu == null)
        {
            Debug.Log("no subscribers on event OnGoBackToPauseMenu");
        }
        else
        {
            OnGoBackToPauseMenu();
        }
    }

    public void SetDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                GameController.SetDifficulty(GameController.Difficulty.Pussy);
                break;
            case 2:
                GameController.SetDifficulty(GameController.Difficulty.VeryHard);
                break;
            case 3:
                GameController.SetDifficulty(GameController.Difficulty.Jedi);
                break;
            default:
                throw new UnityException($"unable to map {difficulty} to any of difficulty levels");

        }
        _ = StartCoroutine(GameController.ReloadCurrentScene(0));
    }
}
