using UnityEngine;
using System.Collections;

public abstract class Explodable : MonoBehaviour
{
    protected const string explosionVfxTag = "explosion_vfx";

    void OnEnable()
    {
        SetInteractive(true);
    }

    protected void SetInteractive(bool state)
    {
        GetComponent<SpriteRenderer>().enabled = state;
        GetComponent<Collider2D>().enabled = state;
        GetComponent<Rigidbody2D>().simulated = state;
    }

    protected void SetChildsActive(bool state)
    {
        int childCount = transform.childCount;

        while (childCount-- > 0)
        {
            transform.GetChild(childCount).gameObject.SetActive(state);
        }
    }

    protected void SetChildsActive(bool state, string tag)
    {
        if (tag == null)
        {
            SetChildsActive(state);
        }
        else
        {
            int childCount = transform.childCount;
            GameObject gameObject;

            while (childCount-- > 0)
            {
                gameObject = transform.GetChild(childCount).gameObject;
                if (gameObject.tag == tag)
                {
                    transform.GetChild(childCount).gameObject.SetActive(state);
                }
            }
        }
    }
}
