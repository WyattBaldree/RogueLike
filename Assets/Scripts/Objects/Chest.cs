using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Wall
{
    public Sprite openedSprite;
    public Sprite closedSprite;
    public bool opened = false;

    public Inventory inventorySource;
    private Inventory inventory;

    private void Start()
    {
        Refresh();
        inventory = Instantiate<Inventory>(inventorySource, GameController.inventoryC.transform);
        inventory.Initialize();
    }

    public void Open()
    {
        opened = true;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
    }

    public void Close()
    {
        opened = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
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
