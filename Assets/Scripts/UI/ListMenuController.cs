using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListMenuController : MonoBehaviour
{
    protected EventSystem eventSystem;

    public void setDefaultFocusOn(GameObject menuItem)
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.firstSelectedGameObject = menuItem;
        eventSystem.SetSelectedGameObject(menuItem);
    }

    public void FocusSelection()
    {
        eventSystem.SetSelectedGameObject(gameObject);
    }

    public void ResetSelection()
    {
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void ResumeGame()
    {
        _ = GameController.GamePauseToggle(forceUnpause: false);
    }
}
