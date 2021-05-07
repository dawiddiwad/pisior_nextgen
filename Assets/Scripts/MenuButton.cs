using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = new Color32(81, 185, 248, 255);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = Color.white;
    }
}
