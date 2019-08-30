using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GUIButton : GUIComponent
{
    public SpriteRenderer buttonSpriteRenderer = null;

    public Sprite spriteOut;
    public Sprite spriteOutHighlight;
    public Sprite spriteOutDisabled;
    public Sprite spriteIn;
    public Sprite spriteInHighlight;
    public Sprite spriteInDisabled;

    public bool toggle = false;
    public bool disabled = false;

    private bool pressed = false;
    private bool hovered = false;

    

    public UnityEvent pressEvent;
    public UnityEvent releaseEvent;

    public override void CustomOnLeftMouseDown()
    {
        if (disabled) return;
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
    
    public override void CustomOnMouseEnter()
    {
        hovered = true;
        UpdateSprite();
    }

    public override void CustomOnMouseExit()
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

    void SetDisabled(bool d)
    {
        disabled = d;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (pressed)
        {
            // down sprite
            if (spriteInDisabled && disabled)
            {
                buttonSpriteRenderer.sprite = spriteInDisabled;
            }
            else if (spriteInHighlight && hovered)
            {
                buttonSpriteRenderer.sprite = spriteInHighlight;
            }
            else
            {
                buttonSpriteRenderer.sprite = spriteIn;
            }
        }
        else
        {
            if (spriteOutDisabled && disabled)
            {
                buttonSpriteRenderer.sprite = spriteOutDisabled;
            }
            else if (spriteOutHighlight && hovered)
            {
                buttonSpriteRenderer.sprite = spriteOutHighlight;
            }
            else
            {
                buttonSpriteRenderer.sprite = spriteOut;
            }
        }

        Align();
    }

    public override Vector2 GetDimensions()
    {
        return (Vector2)buttonSpriteRenderer.bounds.size;
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
        myCollider.size = GetDimensions();
        myCollider.offset = new Vector2(myCollider.size.x / 2, -myCollider.size.y / 2) + alignment;
        buttonSpriteRenderer.transform.position = transform.position + (Vector3)alignment + new Vector3(0, -size.y, 0);
    }

    public override void UpdateGUI()
    {
        UpdateSprite();
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }
}
