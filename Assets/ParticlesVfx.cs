using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesVfx : MonoBehaviour
{
   public void OnParticleSystemStopped()
    {
        Pool.Instance.Return(gameObject);
    }
}
