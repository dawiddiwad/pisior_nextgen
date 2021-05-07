using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUG_fps : MonoBehaviour
{
    void Start()
    {
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = $"HZ: {Screen.currentResolution.refreshRate}\nFPS:{1.0f / Time.smoothDeltaTime}";
    }
}
