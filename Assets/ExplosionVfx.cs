using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVfx : MonoBehaviour
{
    Transform parent;

    private void OnEnable()
    {
        parent = transform.parent;
    }

    public void OnParticleSystemStopped()
    {
        if (parent == null)
        {
            Destroy(gameObject);
        }
        else
        {
            parent?.GetComponent<IDestroyVfx>()?.OnDestroyVfxFinished();
        }
    }
}
