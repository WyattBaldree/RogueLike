using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIButton : GUIComponent
{
    public SpriteRenderer buttonSpriteRenderer = null;
    public BoxCollider2D collider = null;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite hoverSprite;

    public bool toggle = false;

    private bool pressed = false;
    private bool hovered = false;

    public UnityEvent pressEvent;
    public UnityEvent releaseEvent;

    private void OnMouseDown()
    {
        if (toggle)
        {
            Toggle();
        }
        else
        {
            Press();
        }
    }

    private void OnMouseUp()
    {
        if(!toggle)
        {
            Release();
        }
    }

    private void OnMouseEnter()
    {
        hovered = true;
        UpdateSprite();
    }

    private void OnMouseExit()
    {
        hovered = false;
        UpdateSprite();
    }

    public void Press()
    {
        pressed = true;
        pressEvent.Invoke();
        UpdateSprite();
    }

    public void Release()
    {
        pressed = false;
        releaseEvent.Invoke();
        UpdateSprite();
    }

    public void Toggle()
    {
        if (pressed) Release();
        else Press();
    }

    public void UpdateSprite()
    {
        if (pressed)
        {
            // down sprite
            buttonSpriteRenderer.sprite = downSprite;
        }
        else
        {
            if (hovered)
            {
                // hovered sprite
                buttonSpriteRenderer.sprite = hoverSprite;
            }
            else
            {
                // up sprite
                buttonSpriteRenderer.sprite = upSprite;
            }
        }

        Align();
    }

    public override Vector2 GetDimensions()
    {
        return buttonSpriteRenderer.size;
    }

    public override void Align()
    {
        Vector2 size = GetDimensions();

        // Vertical Alignment
        float xAlign;
        if (horizontalAlignment == HorizontalAlignmentEnum.left)
        {
            xAlign = 0;
        }
        else if (horizontalAlignment == HorizontalAlignmentEnum.middle)
        {
            xAlign = (Mathf.Max(minSize.x, maxSize.x) / 2) - (size.x / 2);
        }
        else
        {
            xAlign = Mathf.Max(minSize.x, maxSize.x) - (size.x);
        }

        // Vertical Alignment
        float yAlign;
        if (verticalAlignment == VerticalAlignmentEnum.top)
        {
            yAlign = 0;
        }
        else if (verticalAlignment == VerticalAlignmentEnum.middle)
        {
            yAlign = (Mathf.Max(minSize.y, maxSize.y) / 2) - (size.y / 2);
        }
        else
        {
            yAlign = Mathf.Max(minSize.y, maxSize.y) - (size.y);
        }

        Vector2 alignment = new Vector2(xAlign, -yAlign);
        collider.offset = new Vector2(0.5f, -0.5f) + alignment;
        buttonSpriteRenderer.transform.position = transform.position + (Vector3)alignment + new Vector3(0, -size.y, 0);
    }

    public override void UpdateGUI()
    {
        UpdateSprite();
    }
}
