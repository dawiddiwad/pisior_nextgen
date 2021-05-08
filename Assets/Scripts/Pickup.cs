using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnEnable()
    {
        SetChildsActive(false);
        SetInteractive(true);
    }

    private void OnDisable()
    {
        SetInteractive(true);
    }

    public void OnVfxFinished()
    {
        Pool.Instance.Return(gameObject);
    }

    private void SetChildsActive(bool state)
    {
        int childCount = transform.childCount;

        while (childCount-- > 0)
        {
            transform.GetChild(childCount).gameObject.SetActive(state);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject vfx = Pool.Instance.Get(Pool.GameObjectType.vfxCollisionHealthPickup);
        vfx.transform.position = transform.position;
        vfx.transform.rotation = transform.rotation;

        if (collision.gameObject.tag == "Player")
        {
            HandlePlayerCollison(collision);
        }
        else
        {
            HandlePlayerCollison(collision);
        }
    }

    private void HandlePlayerCollison(Collision2D collision)
    {
        SetInteractive(false);
        SetChildsActive(true);
    }

    private void SetInteractive(bool state)
    {
        GetComponent<SpriteRenderer>().enabled = state;
        GetComponent<BoxCollider2D>().enabled = state;
        GetComponent<Rigidbody2D>().simulated = state;
    }
}
