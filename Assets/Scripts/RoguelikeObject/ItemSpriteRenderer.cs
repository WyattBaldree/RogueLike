using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemSpriteRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Entry stackNumberEntry;

    [SerializeField]
    private SortingGroup sortingGroup;

    public int StackSize
    {
        set
        {
            stackNumberEntry.maxSize.x = mySpriteRenderer.bounds.size.x;
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

    public int SortingOrder
    {
        get => sortingGroup.sortingOrder;
        set => sortingGroup.sortingOrder = value;
    }
}
