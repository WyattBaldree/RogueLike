using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        inventory.Initialize();
    }

    public void Open()
    {
        opened = true;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GameController.logC.NewEntry("The " + instanceName + " is opened.");
    }

    public void Close()
    {
        opened = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        GameController.logC.NewEntry("The " + instanceName + " is closed.");
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
            GameController.popupC.containerGUI.Popup(new Vector2(), inventory);
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
