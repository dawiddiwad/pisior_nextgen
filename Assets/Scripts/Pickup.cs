using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
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
            Pool.Instance.Return(gameObject);
        }
    }

    private void HandlePlayerCollison(Collision2D collision)
    {
        Pool.Instance.Return(gameObject);
    }
}
