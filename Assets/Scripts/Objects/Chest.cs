using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class Chest : Wall
{
    public Sprite openedSprite;
    public Sprite closedSprite;
    public bool opened = false;
    public Inventory inventory;

    public override void Initialize()
    {
        base.Initialize();
        Refresh();
        Assert.IsNotNull(inventory);
    }

    public void Open()
    {
        opened = true;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        if (GetLogController()) GetLogController().NewEntry("The " + instanceName + " is opened.");
    }

    public void Close()
    {
        opened = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        if(GetLogController()) GetLogController().NewEntry("The " + instanceName + " is closed.");
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if (!opened)
        {
            Open();
        }
        else
        {
            GetPopupController().containerGUI.Popup(new Vector2(), inventory);
        }

        GetComponent<Interactive>().ShowMenu();
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
