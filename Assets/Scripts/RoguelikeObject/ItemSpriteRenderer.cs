using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpriteRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Entry stackNumberEntry;

    public int StackSize
    {
        set
        {
            if(value <= 1)
            {
                stackNumberEntry.gameObject.SetActive(false);
            }
            else
            {
                stackNumberEntry.gameObject.SetActive(true);
                stackNumberEntry.SetText(value.ToString());
            }
        }
    }

    public Sprite ItemSprite
    {
        set
        {
            mySpriteRenderer.sprite = value;
        }
    }
}
