using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private bool gameWon = false;

#if (UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL)
    private void showMobilePauseButton()
    {
        findChildGameObjectBy(name: "MobilePause").SetActive(true);
    }
#endif

    private void showDifficultyMenu()
    {
        findChildGameObjectBy(name: "PauseMenu").SetActive(false);
        findChildGameObjectBy(name: "DifficultyMenu").SetActive(true);
    }

    private void hideDifficultyMenu()
    {
        findChildGameObjectBy(name: "PauseMenu").SetActive(true);
        findChildGameObjectBy(name: "DifficultyMenu").SetActive(false);
    }

    private GameObject findChildGameObjectBy(string name)
    {
        foreach (Component component in GetComponentsInChildren<Component>(true))
        {
            if (component.gameObject.name == name)
            {
                return component.gameObject;
            }
        }
        throw new UnityException($"There is no child element: {name} inside: {gameObject.name}");
    }
    private void processGameEnd(GameController.GameResult result)
    {
        switch (result)
        {
            case GameController.GameResult.Won:
                //findChildGameObjectBy(name: "Winner").SetActive(true);
                gameWon = true;
                findChildGameObjectBy(name: "Stats").SetActive(false);
                findChildGameObjectBy(name: "LevelSummary").SetActive(true);
                break;
            case GameController.GameResult.Lost:
                findChildGameObjectBy(name: "GameOver").SetActive(true);
                break;
        }
    }

    private void processGamePause()
    {
        findChildGameObjectBy(name: "PauseMenu").SetActive(true);
        findChildGameObjectBy(name: "LevelSummary").SetActive(false);
    }

    private void processGameResume()
    {
        findChildGameObjectBy(name: "PauseMenu").SetActive(false);
        findChildGameObjectBy(name: "DifficultyMenu").SetActive(false);
        if (gameWon)
        {
            findChildGameObjectBy(name: "LevelSummary").SetActive(true);
        }
    }

    void OnEnable()
    {
        GameController.OnGameEnd += processGameEnd;
        GameController.onGamePuase += processGamePause;
        GameController.onGameResume += processGameResume;
        PauseMenu.OnOpenDifficultyMenu += showDifficultyMenu;
        DifficultyMenu.OnGoBackToPauseMenu += hideDifficultyMenu;
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL)
        showMobilePauseButton();
#endif
    }

    private void OnDisable()
    {
        GameController.OnGameEnd -= processGameEnd;
        GameController.onGamePuase -= processGamePause;
        GameController.onGameResume -= processGameResume;
        PauseMenu.OnOpenDifficultyMenu -= showDifficultyMenu;
        DifficultyMenu.OnGoBackToPauseMenu -= hideDifficultyMenu;
    }
}
