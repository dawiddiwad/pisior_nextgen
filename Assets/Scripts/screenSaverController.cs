using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenSaverController : MonoBehaviour
{
    //Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Application.Quit();
        }
    }
}
