using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Object
{
    public SpriteRenderer mySpriteRenderer;
    public Sprite itemSprite;

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
    
    public Inventory myInventory = null;

    public enum Rarity{normal, superior, outstanding, exceptional, superb};
    public Rarity rarity = Rarity.normal;

    public virtual void Initialize()
    {
        //mySpriteRenderer.sprite = itemSprite;
    }

    private void Start()
    {
        //mySpriteRenderer.sprite = itemSprite;
    }

    public virtual void Obtained()
    {
        dropped = false;
        mySpriteRenderer.sprite = null;
    }

    public virtual void UpdateImage()
    {
        
    }

    public virtual void Dropped()
    {
        dropped = true;
        mySpriteRenderer.sprite = itemSprite;
    }

    public void SetStackSize(int newStackSize)
    {
        if(newStackSize <= 0)
        {
            Debug.Log("WE NEED TO DESTROY THE ITEMS WHEN THEIR STACK SIZE BECOMES 0!!!!");
            //myInventory.RemoveItem(this);
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
