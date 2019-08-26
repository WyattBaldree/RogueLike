using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGridInstance : MouseInteractive
{
    [SerializeField]
    SpriteRenderer mySpriteRenderer;

    [SerializeField]
    Sprite hoveredSprite;

    public override void CustomOnMouseEnter()
    {
        mySpriteRenderer.sprite = hoveredSprite;
    }

    public override void CustomOnMouseExit()
    {
        mySpriteRenderer.sprite = null;
    }
}
