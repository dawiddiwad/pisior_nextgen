using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeader : MonoBehaviour
{
    void hanldeGamePause()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }
    void hanldeGameResume()
    {
        this.gameObject.GetComponent<Canvas>().enabled = true;
    }

    void OnEnable()
    {
        GameController.onGamePuase += hanldeGamePause;
        GameController.onGameResume += hanldeGameResume;
    }

    private void OnDisable()
    {
        GameController.onGamePuase -= hanldeGamePause;
        GameController.onGameResume -= hanldeGameResume;
    }
}
