using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Wall
{
    public Sprite openedSprite;
    public Sprite closedSprite;
    public bool opened = false;

    public Inventory inventorySource;
    public Inventory inventory;

    public override void Initialize()
    {
        base.Initialize();
        Refresh();
        inventory = Instantiate<Inventory>(inventorySource, GameController.inventoryC.transform);
        inventory.Initialize();
    }

    public void Open()
    {
        opened = true;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GameController.logC.NewEntry("The " + wallName + " is opened.");
    }

    public void Close()
    {
        opened = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        GameController.logC.NewEntry("The " + wallName + " is closed.");
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
            inventory.ShowInventoryGUI();
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
