using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Wall
{
    public Sprite openedSprite;
    public Sprite closedSprite;
    public bool opened = false;

    private void Start()
    {
        Refresh();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if (opened)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Close()
    {
        opened = false;
        solid = true;
        cost = 1;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }

    public void Open()
    {
        opened = true;
        solid = false;
        cost = 0;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
    }

    public void Refresh()
    {
        if (opened)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public override void RefreshSprite()
    {

    }
}
