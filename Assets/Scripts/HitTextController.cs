using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitTextController : MonoBehaviour
{
    private bool initialized = false;
    private float textSpeed = 10;
    private float textFadeSpeed = 1;
    private Vector3 startPosition = new Vector3(0,0,0);
    private Text textCmp;

    public enum UNIT
    {
        HP,
        SCORE,
        EMPTY
    }

    private void setupParent()
    {
        if (GameController.stats != null)
        {
            transform.SetParent(GameController.stats.transform, false);
        }
    }
    private void setupTextValue(int value, UNIT _unit)
    {
        string symbol;
        string unit;

        switch (_unit)
        {
            case UNIT.HP:
                unit = "HP";
                break;
            case UNIT.SCORE:
            case UNIT.EMPTY:
            default:
                unit = "";
                break;
        }
 
        if (value >= 0)
        {
            symbol = "+";
            GetComponent<Text>().color = Color.green;
        }
        else
        {
            symbol = "";
            GetComponent<Text>().color = Color.red;
        }
        textCmp.text = $"{symbol}{value}{unit}";
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        initialized = false;
        textCmp = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            transform.Translate(0, Time.deltaTime * textSpeed, 0, Space.World);
        }
    }

    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(textFadeSpeed);
        ResetProperties();
        Pool.Instance.Return(gameObject);
    }

    private void ResetProperties()
    {
        initialized = false;
        textCmp.text = "";
        transform.position = startPosition;
    }

    public void Setup(int value, Vector3 position, UNIT unit)
    {
        setupParent();
        setupTextValue(value, unit);
        transform.position = position;
        textCmp.CrossFadeAlpha(0, textFadeSpeed, false);
        _ = StartCoroutine(Recycle());
        initialized = true;
    }
}
