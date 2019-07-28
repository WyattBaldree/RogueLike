using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : GUIComponent
{
    public SpriteRenderer backgroundSpriteRenderer = null;

    public Vector2 borderSize = new Vector2(0,0);
    private Vector2 actualSize = new Vector2(0, 0);

    public void SetDimension(Vector2 size)
    {
        actualSize = size;
        backgroundSpriteRenderer.size = new Vector2(size.x / backgroundSpriteRenderer.transform.localScale.x, size.y / backgroundSpriteRenderer.transform.localScale.y);
        Align();
    }

    public void SetDimensionX(float x)
    {
        SetDimension(new Vector2(x, backgroundSpriteRenderer.size.y));
    }

    public void SetDimensionY(float y)
    {
        SetDimension(new Vector2(backgroundSpriteRenderer.size.x, y));
    }

    public override Vector2 GetDimensions()
    {
        return actualSize;
    }

    public override void Align()
    {
        backgroundSpriteRenderer.transform.position = transform.position - new Vector3(0, actualSize.y, 0);
    }

    public override void UpdateGUI()
    {
        SetDimension(maxSize);
    }
}