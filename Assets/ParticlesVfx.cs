using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesVfx : MonoBehaviour
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
            Pool.Instance.Return(gameObject);
        }
        else
        {
            parent?.GetComponent<Pickup>().OnVfxFinished();
        }
    }
}
