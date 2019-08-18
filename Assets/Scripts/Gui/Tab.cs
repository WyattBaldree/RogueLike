using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SelectEvent : UnityEvent<int>
{
}

[System.Serializable]
public class UnselectEvent : UnityEvent<int>
{
}
/// <summary>
/// Tabs are used inside of TabGroups and allow you to create tabs that trigger specifed functions
/// </summary>
public class Tab : MonoBehaviour
{
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public SpriteRenderer spriteRenderer;

    public bool selected = false;

    public int index = 0;

    public bool unselectable = false;

    public SelectEvent selectEvent;
    public SelectEvent unselectEvent;

    //public TabGroup tabGroup;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTabSprite();
    }

    void UpdateTabSprite()
    {
        if(spriteRenderer == null)
        {
            Debug.Log("SpriteRenderer not sttached to tab", this);
            return;
        }
        if (selected)
        {
            if(selectedSprite == null)
            {
                Debug.Log("selectedSprite not set.", this);
            }
            else
            {
                spriteRenderer.sprite = selectedSprite;
            }
        }
        else
        {
            if (unselectedSprite == null)
            {
                Debug.Log("unselectedSprite not set.", this);
            }
            else
            {
                spriteRenderer.sprite = unselectedSprite;
            }
        }
    }

    private void OnValidate()
    {
        UpdateTabSprite();
    }

    private void OnMouseDown()
    {
        Toggle();
    }

    void Toggle(bool force = false)
    {
        if (!selected)
        {
            selected = true;
            selectEvent.Invoke(index);
        }
        else if(force || unselectable)
        {
            selected = false;
            unselectEvent.Invoke(index);
        }
        UpdateTabSprite();
    }

    public void SetSelected(bool value, bool triggerEvents = false)
    {
        if (value)
        {
            selected = true;

            if(triggerEvents) selectEvent.Invoke(index);
        }
        else
        {
            selected = false;
            if (triggerEvents) unselectEvent.Invoke(index);
        }
        UpdateTabSprite();
    }
}
