﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RogueSpriteRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Entry stackNumberEntry;

    [SerializeField]
    private SortingGroup sortingGroup;

    [SerializeField]
    /// <summary>
    /// This sprite is used by RoguelikeSpriteRenderers when no other sprite is available
    /// </summary>
    private Sprite debugSprite;

    public int StackSize
    {
        set
        {
            stackNumberEntry.maxSize.x = mySpriteRenderer.bounds.size.x *.9f;
            if(value <= 1)
            {
                stackNumberEntry.gameObject.SetActive(false);
            }
            else
            {
                stackNumberEntry.gameObject.SetActive(true);
                stackNumberEntry.SetText(value.ToString());
            }
            stackNumberEntry.gameObject.transform.position = this.transform.position + new Vector3(0, stackNumberEntry.GetDimensions().y + mySpriteRenderer.bounds.size.y * .1f, 0);
        }
    }

    public Sprite MySprite
    {
        set
        {
            if(value != null)
            {
                mySpriteRenderer.sprite = value;
            }
            else
            {
                mySpriteRenderer.sprite = debugSprite;
            }
        }
    }

    public int SortingOrder
    {
        get => sortingGroup.sortingOrder;
        set => sortingGroup.sortingOrder = value;
    }
}
