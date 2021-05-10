using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    ChromaticAberration chromaticAberration;

    private void adjustChromaticAberation(int healthValue)
    {
        float playerHealthPrcnt = (float)GameController.playerController.getHealthPrcnt();
        chromaticAberration.intensity.value = playerHealthPrcnt > 0 ?
            (float)(0.1m / (decimal)playerHealthPrcnt * 0.5m)
            : 0.4f;
    }
    private void OnEnable()
    {
        _ = GetComponent<Volume>().profile.TryGet(out chromaticAberration);
        PlayerController.OnHealthChange += adjustChromaticAberation;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChange -= adjustChromaticAberation;
    }
}
