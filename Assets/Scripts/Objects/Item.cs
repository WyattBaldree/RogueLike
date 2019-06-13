using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Object
{
    public SpriteController mySpriteController;

    List<Sprites> mySprites;

    public string uniqueId = "item";

    public List<string> typeList = new List<string>()
    {
        "item"
    };

    public int value = 1;
    public int stackSize = 1;
    public int maxStackSize = 20;

    bool autoPickup = false;

    public bool dropped = true;

    Inventory myInventory = null;

    public enum Rarity{normal, superior, outstanding, exceptional, superb};
    public Rarity rarity = Rarity.normal;

    public virtual void Initialize()
    {
        mySprites = mySpriteController.GetMySpriteRenderers();
    }

    public virtual void Obtained(Inventory newInventory)
    {
        dropped = false;
        foreach (Sprites s in mySprites)
        {
            s.GetComponent<SpriteRenderer>().enabled = false;
        }
        myInventory = newInventory;
    }

    public virtual void UpdateImage()
    {
        
    }

    public virtual void Dropped()
    {
        dropped = true;
        foreach (Sprites s in mySprites)
        {
            s.GetComponent<SpriteRenderer>().enabled = true;
        }
        myInventory = null;
    }

    public void SetStackSize(int newStackSize)
    {
        if(newStackSize <= 0)
        {
            Debug.Log("WE NEED TO DESTROY THE ITEMS WHEN THEIR STACK SIZE BECOMES 0!!!!");
            myInventory.RemoveItem(this);
            Destroy(gameObject);
            return;
        }

        stackSize = newStackSize;
        gameObject.GetComponent<Item>().UpdateImage();

        if (myInventory && myInventory.myInventoryGUI)
        {
            myInventory.myInventoryGUI.UpdateFrameContent();
        }

        UpdateImage();
    }
}
