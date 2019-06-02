using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite hoveredSprite;

    private SpriteRenderer spriteRenderer;

    public bool down = false;

    public bool toggle = false;

    private bool hovered = false;

    public UnityEvent downEvent;
    public UnityEvent upEvent;

    public bool unselectable = false;

    private void OnValidate()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        UpdateButtonSprite();
    }

    void UpdateButtonSprite()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("SpriteRenderer not sttached to button", this);
            return;
        }
        if (down)
        {
            if (downSprite == null)
            {
                Debug.Log("downSprite not set.", this);
            }
            else
            {
                spriteRenderer.sprite = downSprite;
            }
        }
        else
        {
            if (hovered)
            {
                if (hoveredSprite == null)
                {
                    Debug.Log("upSprite not set.", this);
                }
                else
                {
                    spriteRenderer.sprite = hoveredSprite;
                }
            }
            else
            {
                if (upSprite == null)
                {
                    Debug.Log("upSprite not set.", this);
                }
                else
                {
                    spriteRenderer.sprite = upSprite;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (toggle)
        {
            Toggle();
        }
        else
        {
            SetButton(true, !down);
        }
    }

    private void OnMouseUp()
    {
        if (toggle)
        {
            return;
        }
        else
        {
            SetButton(false, down);
        }
    }

    private void OnMouseExit()
    {
        hovered = false;
        if(toggle)
        {
            return;
        }
        else
        {
            SetButton(false, down);
        }
        UpdateButtonSprite();
    }

    private void OnMouseEnter()
    {
        hovered = true;
        UpdateButtonSprite();
    }

    void Toggle(bool force = false)
    {
        if(!force && unselectable)
        {
            return;
        }

        if (!down)
        {
            down = true;
            downEvent.Invoke();
        }
        else
        {
            down = false;
            upEvent.Invoke();
        }
        UpdateButtonSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonSprite();
    }

    public void SetButton(bool buttonDown, bool triggerEvents = false)
    {
        if (buttonDown)
        {
            down = true;

            if (triggerEvents) downEvent.Invoke();
        }
        else
        {
            down = false;
            if (triggerEvents) upEvent.Invoke();
        }
        UpdateButtonSprite();
    }

}
