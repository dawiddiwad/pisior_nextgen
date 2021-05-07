using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreDisplayController : MonoBehaviour
{
    public string scoreTemplateText = "SCORE";
    public string delimiter = " : ";

    private Text textCmp;
    private int newScoreValue = -1;

    // Start is called before the first frame update
    public void setScoreText(int score)
    {
        textCmp.text = $"{scoreTemplateText}{delimiter}{score}";
    }

    public void enableScoreUpdate(int newScoreValue)
    {
        this.newScoreValue = newScoreValue;
    }

    void Start()
    {
        textCmp = GetComponent<Text>();
        enableScoreUpdate(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (newScoreValue != -1)
        {
            setScoreText(newScoreValue);
            newScoreValue = -1;
        }
    }

    private void OnEnable()
    {
        GameController.onScoreChange += enableScoreUpdate;
    }

    private void OnDisable()
    {
        GameController.onScoreChange -= setScoreText;
    }
}
