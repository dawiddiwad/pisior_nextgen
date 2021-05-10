using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Explodable, IDestroyVfx
{
    void OnEnable()
    {
        PrepareDestroyVfx();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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
        TriggerDestroyVfx();
    }

    public void PrepareDestroyVfx()
    {
        SetChildsActive(false, explosionVfxTag);
    }

    public void TriggerDestroyVfx()
    {
        SetChildsActive(true, explosionVfxTag);
    }

    public void OnDestroyVfxFinished()
    {
        Pool.Instance.Return(gameObject);
    }
}
