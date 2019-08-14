using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public RoguelikeObject[] itemList = new RoguelikeObject[80];

    /// <summary>
    /// Total inventory capacity
    /// </summary>
    public int inventoryCapacity = 80;

    ///The gui we are currently hooked up to
    public InventoryGUI myInventoryGUI;

    public string inventoryName = "default inventory";

    /// <summary>
    /// Should the item be visible in the world?
    /// </summary>
    public bool external = false;

    /// <summary>
    /// A list of strings that difines what types of items are allowed.
    /// </summary>
    public List<RoguelikeObject.TagEnum> acceptableTypes = new List<RoguelikeObject.TagEnum>()
    {
        RoguelikeObject.TagEnum.item
    };

    /// <summary>
    /// Called upon being created.
    /// </summary>
    public void Initialize()
    {

    }

    /// <summary>
    /// Add an item to the inventory at the first available spot.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public bool AddItem(RoguelikeObject newItem)
    {
        int i = GetAvailableStack(newItem, newItem.UniqueID);
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
    public bool AddItem(RoguelikeObject newItem, int index)
    {
        bool acceptable = false;
        foreach (RoguelikeObject.TagEnum tag in newItem.tags)
        {
            if (acceptableTypes.Contains(tag))
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
            RoguelikeObject existingStack = itemList[index];
            if (newItem.UniqueID == existingStack.UniqueID)
            {
                //if we're droppping onto an existing stack of the same kind
                int availableSpace = existingStack.StackSizeMax - existingStack.StackSize;
                if (availableSpace <= 0)
                {
                    //if the stack is already full, return false
                    return false;
                }

                if(newItem.StackSize <= availableSpace)
                {
                    //if there is enough space for our stack, add it to the existing stack
                    existingStack.StackSize = existingStack.StackSize + newItem.StackSize;
                    Destroy(newItem.gameObject);
                    UpdateInventoryGUI(index);
                    return true;
                }
                else
                {
                    //if the stack's available space is too small to hold all of our stack, 
                    //fill the existing stack and add the remainder to the next available stack.
                    existingStack.StackSize = existingStack.StackSizeMax;
                    newItem.StackSize = newItem.StackSize - availableSpace;
                    UpdateInventoryGUI(index);
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
            newItem.Exposed = true;
        }
        else
        {
            //else the item is obtains and will not show up in the world.
            newItem.Exposed = false;
        }

        newItem.transform.position = transform.position;
        newItem.transform.SetParent(gameObject.transform);

        newItem.MyInventory = this;
        UpdateInventoryGUI(index);
        return true;
    }

    /// <summary>
    /// Returns the index of the first available non-full stack with some optional filters
    /// </summary>
    /// <param name="ignoreItem">optionally specify a specify an item stack to ignore</param>
    /// <param name="uid">optionally set a string UniqueID filter on the search</param>
    /// <returns></returns>
    public int GetAvailableStack(RoguelikeObject ignoreItem = null, string uid = "")
    {
        for( int i = 0; i < itemList.Length; i++ )
        {
            if(itemList[i] && itemList[i] != ignoreItem && (uid == "" || itemList[i].UniqueID == uid) && itemList[i].StackSize < itemList[i].StackSizeMax)
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
    public bool RemoveItem(RoguelikeObject item)
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
    /// <param name="item">The item index to remove from.</param>
    /// <returns></returns>
    public bool RemoveItem(int index)
    {
        if(itemList[index] == null)
        {
            return false;
        }

        RoguelikeObject item = itemList[index];
        itemList[index] = null;
        UpdateInventoryGUI(index);
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
        RoguelikeObject item = itemList[index];
        if(amount < 1 || amount >= item.StackSize)
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
            RoguelikeObject newItem = RoguelikeObject.MakeItem(item, amount, inv);
            if (newItem)
            {
                item.StackSize = item.StackSize - amount;
                UpdateInventoryGUI(index);
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
    public bool MoveItem(RoguelikeObject item, Inventory inv, int amount = -1)
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
        RoguelikeObject item = itemList[index1];
        if (amount < 1 || amount >= item.StackSize)
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
            RoguelikeObject newItem = RoguelikeObject.MakeItem(item, amount, inv, index2);
            if (newItem)
            {
                item.StackSize = item.StackSize - amount;
                UpdateInventoryGUI(index1);
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
    public bool MoveItem(RoguelikeObject item, Inventory inv, int index2, int amount)
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
    public RoguelikeObject GetItem(int index)
    {
        return itemList[index];
    }

    /// <summary>
    /// Get the index of the item specified.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>Returns the index or -1 if it is not found.</returns>
    public int GetItemIndex(RoguelikeObject item)
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
