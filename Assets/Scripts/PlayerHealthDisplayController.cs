using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDisplayController : MonoBehaviour
{
    public string healthTemplateText = "HEALTH";
    public string delimiter = " ";

    private AudioSource audioSourceGameControl;
    private Color defaultColor;
    private Text textCmp;
    private int newHealthValue = -1;

    // Start is called before the first frame update
    public void setHealth(int health)
    {
        GetComponent<Text>().text = $"{healthTemplateText}{delimiter}{health}";
    }

    // Update is called once per frame
    void Update()
    {
        if (newHealthValue != -1)
        {
            setHealth(newHealthValue);
            newHealthValue = -1;
        }
    }

    private void OnEnable()
    {
        intialize();
        PlayerController.OnHealthChange += adjustHealthAndThemePitch;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChange -= adjustHealthAndThemePitch;
    }

    private void intialize()
    {
        textCmp = GetComponent<Text>();
        defaultColor = textCmp.color;
        audioSourceGameControl = GameObject.Find("GameControl").GetComponent<AudioSource>();
    }

    private void adjustHealthAndThemePitch(int healthValue)
    {
        if (healthValue == 0)
        {
            textCmp.color = Color.red;
            audioSourceGameControl.pitch = 0.6f;
        }
        else if (healthValue <= 25)
        {
            textCmp.color = Color.red;
            audioSourceGameControl.pitch = 1.04f;
        }
        else if (healthValue <= 50)
        {
            textCmp.color = Color.red;
            audioSourceGameControl.pitch = 1.02f;
        }
        else if (healthValue <= 75)
        {
            textCmp.color = Color.yellow;
            audioSourceGameControl.pitch = 1.01f;
        }
        else
        {
            textCmp.color = defaultColor;
            audioSourceGameControl.pitch = 1.00f;
        }
        newHealthValue = healthValue;
    }
}
