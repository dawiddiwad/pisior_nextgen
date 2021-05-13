using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSummary : MonoBehaviour
{
    GameObject result;
    GameObject score;
    GameObject star1;
    GameObject star2;
    GameObject star3;
    // Start is called before the first frame update
    void OnEnable()
    {
        result = GameObject.Find("Result");
        score = GameObject.Find("LevelScore");
        star1 = GameObject.Find("Star1");
        star2 = GameObject.Find("Star2");
        star3 = GameObject.Find("Star3");
        UpdateStars();
        UpdateScore();
    }

    private void UpdateScore()
    {
        score.GetComponent<Text>().text = $"SCORED {GameController.getScore()}";
    }

    private void UpdateStars()
    {
        GameLevels.stars star = GameLevels.stars.none;
        int score = GameController.getScore();
        foreach (var treshold in GameLevels.getScoreTresholdsFor((GameController.Difficulty)GameController.getDifficultyFactor()))
        {
            if (score > treshold.Value)
            {
                star = treshold.Key;
            }
        }

        switch (star)
        {
            case GameLevels.stars.first:
                result.GetComponent<Text>().text = "BONER!";
                MarkStarActive(star1);
                break;
            case GameLevels.stars.second:
                result.GetComponent<Text>().text = "NICE!";
                MarkStarActive(star1);
                MarkStarActive(star2);
                break;
            case GameLevels.stars.third:
                result.GetComponent<Text>().text = "AWESOME!";
                MarkStarActive(star1);
                MarkStarActive(star2);
                MarkStarActive(star3);
                break;
            default:
                result.GetComponent<Text>().text = "LAME!";
                break;
        }
    }

    private void MarkStarActive(GameObject star)
    {
        star.GetComponent<Animator>().SetTrigger("GrantStar");
    }
}
