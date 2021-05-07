using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public static void playAnimation()
    {
        new Animator().SetBool("Granted", true);
    }

}