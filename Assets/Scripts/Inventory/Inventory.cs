using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public Item[] itemList = new Item[80];

    /// <summary>
    /// Total inventory capacity
    /// </summary>
    public int inventoryCapacity = 80;

    ///The gui we are currently hooked up to
    public InventoryGUI myInventoryGUI;

    //public InventoryController.inventoryEnum myInventoryGUIEnum = InventoryController.inventoryEnum.none;

    public string inventoryName = "default inventory";

    /// <summary>
    /// Should the item be visible in the world?
    /// </summary>
    public bool external = false;

    /// <summary>
    /// Can this inventory be opened?
    /// </summary>
    public bool openable = false;

    /// <summary>
    /// An event that is emited when one of the items in the inventory is changed.
    /// </summary>
    public UnityEvent itemChangedEvent;

    /// <summary>
    /// A list of strings that difines what types of items are allowed.
    /// </summary>
    public List<string> acceptableTypes = new List<string>()
    {
        "item"
    };



    /*/// <summary>
    /// Shows the inventory gui and moves its position to the position supplied.
    /// </summary>
    /// <param name="position">Where the inventory will open on the screen.</param>
    /// <returns></returns>
    public bool Open(Vector3 position)
    {
        if(openable == false)
        {
            Debug.Log("unable to open inventory, openable is false");
            return false;
        }
        myInventoryGUI.transform.position = new Vector2(Mathf.Clamp(position.x-3.13f, 0, GameController.gameC.ScreenResInUnits.x - 7.45f), Mathf.Clamp(position.y+3f, 0, GameController.gameC.ScreenResInUnits.y - 1.41f));
        ShowInventoryGUI();
        
        return true;
    }
    
    /// <summary>
    /// Hides the inventory gui.
    /// </summary>
    /// <returns></returns>
    public bool Close()
    {
        if (openable == false)
        {
            Debug.Log("unable to close inventory, openable is false");
            return false;
        }
        HideInventoryGUI();
        return true;
    }*/

    /// <summary>
    /// Called upon being created. Currently updates myInventoryGUI based on what our myInventoryGUIEnum
    /// </summary>
    public void Initialize()
    {
        //ChangeInventoryGUI(myInventoryGUIEnum);
    }

    /// <summary>
    /// Add an item to the inventory at the first available spot.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public bool AddItem(Item newItem)
    {
        int i = GetAvailableStack(newItem, newItem.uniqueId);
        if (i >= 0)
        {
            //add this new item to the existing stack
            return AddItem(newItem, i);
        }

        return AddItem(newItem, GetFirstAvailableSlot());
    }

    /// <summary>
    /// Add an item to a specific index in the inventory.
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool AddItem(Item newItem, int index)
    {
        bool acceptable = false;
        foreach (string s in newItem.typeList)
        {
            if (acceptableTypes.Contains(s))
            {
                acceptable = true;
                break;
            }
        }
        if (!acceptable) return false;

        if (index >= inventoryCapacity) return false;

        if (!newItem) return false;

        //if there is already a stack here...
        if (itemList[index] != null)
        {
            Item existingStack = itemList[index];
            if (newItem.uniqueId == existingStack.uniqueId)
            {
                //if we're droppping onto an existing stack of the same kind
                int availableSpace = existingStack.maxStackSize - existingStack.stackSize;
                if (availableSpace <= 0)
                {
                    //if the stack is already full, return false
                    return false;
                }

                if(newItem.stackSize <= availableSpace)
                {
                    //if there is enough space for our stack, add it to the existing stack
                    existingStack.SetStackSize(existingStack.stackSize + newItem.stackSize);
                    Destroy(newItem.gameObject);
                    UpdateInventoryGUI(index);
                    itemChangedEvent.Invoke();
                    return true;
                }
                else
                {
                    //if the stack's available space is too small to hold all of our stack, 
                    //fill the existing stack and add the remainder to the next available stack.
                    existingStack.SetStackSize(existingStack.maxStackSize);
                    newItem.SetStackSize(newItem.stackSize - availableSpace);
                    itemChangedEvent.Invoke();
                    return AddItem(newItem);
                }
            }
            else
            {
                return false;
            }
        }
        //else add the item to the inv index
        itemList[index] = newItem;

        if (external)
        {
            //if this is an external inv, the item is dropped meaning, among other things, it will be visible in the world and located at the external inv's location.
            newItem.Dropped();
            newItem.transform.position = transform.position;
        }
        else
        {
            //else the item is obtains and will not show up in the world.
            newItem.Obtained(this);
        }

        UpdateInventoryGUI(index);
        itemChangedEvent.Invoke();
        return true;
    }

    /// <summary>
    /// Returns the index of the first available non-full stack with some optional filters
    /// </summary>
    /// <param name="ignoreItem">optionally specify a specify an item stack to ignore</param>
    /// <param name="uid">optionally set a string uid filter on the search</param>
    /// <returns></returns>
    public int GetAvailableStack(Item ignoreItem = null, string uid = "")
    {
        for( int i = 0; i < itemList.Length; i++ )
        {
            if(itemList[i] && itemList[i] != ignoreItem && (uid == "" || itemList[i].uniqueId == uid) && itemList[i].stackSize < itemList[i].maxStackSize)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Remove an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns></returns>
    public bool RemoveItem(Item item)
    {
        int index = GetItemIndex(item);
        if (index >= 0)
        {
            return RemoveItem(index);
        }
        return false;
    }

    /// <summary>
    /// Remove an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns></returns>
    public bool RemoveItem(int index)
    {
        if(itemList[index] == null)
        {
            return false;
        }

        Item item = itemList[index];
        itemList[index] = null;
        UpdateInventoryGUI(index);
        itemChangedEvent.Invoke();
        return true;
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="index">The index of the item to move.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public bool MoveItem(int index, Inventory inv, int amount)
    {
        Item item = itemList[index];
        if(amount < 1 || amount >= item.stackSize)
        {
            RemoveItem(index);
            if (!inv.AddItem(item))
            {
                AddItem(item, index);
                return false;
            }
            return true;
        }
        else
        {
            Item newItem = ItemController.MakeItem(item, amount, inv);
            if (newItem)
            {
                item.SetStackSize(item.stackSize - amount);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public bool MoveItem(Item item, Inventory inv, int amount = -1)
    {
        int index = GetItemIndex(item);
        if (index >= 0) return MoveItem(index, inv, amount);
        return false;
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="index1">The index in this inventory we are moving from.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="index2">The index to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public bool MoveItem(int index1, Inventory inv, int index2, int amount = -1)
    {
        Item item = itemList[index1];
        if (amount < 1 || amount >= item.stackSize)
        {
            if (!RemoveItem(index1)) return false;
            if (!inv.AddItem(item, index2))
            {
                AddItem(item, index1);
                return false;
            }
            return true;
        }
        else
        {
            Item newItem = ItemController.MakeItem(item, amount, inv, index2);
            if (newItem)
            {
                item.SetStackSize(item.stackSize - amount);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="index2">The index to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public bool MoveItem(Item item, Inventory inv, int index2, int amount)
    {
        int index = GetItemIndex(item);
        if (index >= 0) return MoveItem(index, inv, index2, amount);
        return false;
    }

    /// <summary>
    /// Get the item at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item GetItem(int index)
    {
        return itemList[index];
    }

    /// <summary>
    /// Get the index of the item specified.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>Returns the index or -1 if it is not found.</returns>
    public int GetItemIndex(Item item)
    {
        for(int i = 0; i < inventoryCapacity; i++)
        {
            if (itemList[i] == item) return i;
        }

        return -1;
    }

    /// <summary>
    /// Get the index of the first empty slot.
    /// </summary>
    /// <returns></returns>
    public int GetFirstAvailableSlot()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (itemList[i] == null) return i;
        }

        return -1;
    }

    /*////////////////////////////////////////Inventory GUI stuff
    /// <summary>
    /// Change the inventory gui of this inventory.
    /// </summary>
    /// <param name="i"></param>
    public void ChangeInventoryGUI(InventoryController.inventoryEnum i)
    {
        if ((int)i == 0) return;

        if (myInventoryGUI)
        {
            if (myInventoryGUI.targetInventory == this)
            {
                myInventoryGUI.targetInventory = null;
            }
        }
        myInventoryGUI = GameController.inventoryC.getInventoryGUI(i);
        myInventoryGUI.targetInventory = this;
    }

    /// <summary>
    /// Show the inventoryGUI in myInventoryGUI.
    /// </summary>
    public void ShowInventoryGUI()
    {
        myInventoryGUI.targetInventory = this;
        myInventoryGUI.gameObject.SetActive(true);
        Entry title = myInventoryGUI.GetComponentInChildren<Entry>();
        if (title)
        {
            title.EntryInitialize(inventoryName);
        }
        UpdateInventoryGUI();
    }

    /// <summary>
    /// Hide our inventoryGUI.
    /// </summary>
    public void HideInventoryGUI()
    {
        myInventoryGUI.gameObject.SetActive(false);
    }
    */

    /// <summary>
    /// Update our inventoryGUI.
    /// </summary>
    private void UpdateInventoryGUI(int index = -1)
    {
        if (myInventoryGUI)
        {
            myInventoryGUI.UpdateFrameContent(index);
        }
    }
}
