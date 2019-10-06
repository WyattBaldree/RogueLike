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
    public List<InventoryGUI> myInventoryGUIs = new List<InventoryGUI>();

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
    /// Add an item to the inventory at the first available spot.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public RoguelikeObject AddItem(RoguelikeObject sourceItem, int amount = -1)
    {
        if (amount == -1) amount = sourceItem.StackSize;

        int i = GetAvailableStack(sourceItem, sourceItem.UniqueID);
        if (i >= 0)
        {
            //add this new item to the existing stack
            return AddItem(sourceItem, amount, i);
        }

        return AddItem(sourceItem, amount, GetFirstAvailableSlot());
    }

    /// <summary>
    /// Add an item to a specific index in the inventory.
    /// </summary>
    /// <returns>The stack that our add ended on or null if we were unable to add all of our item.</returns>
    public RoguelikeObject AddItem(RoguelikeObject sourceItem, int amount, int index)
    {
        bool acceptable = false;
        foreach (RoguelikeObject.TagEnum tag in sourceItem.tags)
        {
            if (acceptableTypes.Contains(tag))
            {
                acceptable = true;
                break;
            }
        }
        if (!acceptable) return null;

        if (index == -1)
        {
            index = GetAvailableStack(sourceItem, sourceItem.UniqueID);
        }

        if (index == -1)
        {
            index = GetFirstAvailableSlot();
        }

        if (index < 0 || index >= inventoryCapacity) return null;

        if (!sourceItem) return null;

        //if there is already a stack here...
        if (itemList[index] != null)
        {
            RoguelikeObject existingStack = itemList[index];
            if (sourceItem.UniqueID == existingStack.UniqueID)
            {
                //if we're droppping onto an existing stack of the same kind
                int availableSpace = existingStack.StackSizeMax - existingStack.StackSize;
                if (availableSpace <= 0)
                {
                    //if the stack is already full, return false
                    return null;
                }

                if(amount <= availableSpace)
                {
                    //if there is enough space for our stack, add it to the existing stack
                    existingStack.StackSize += amount;
                    sourceItem.StackSize -= amount;
                    UpdateInventoryGUI(index);
                    return existingStack;
                }
                else
                {
                    //if the stack's available space is too small to hold all of our stack, 
                    //fill the existing stack and add the remainder to the next available stack.
                    existingStack.StackSize = existingStack.StackSizeMax;
                    sourceItem.StackSize -= availableSpace;
                    UpdateInventoryGUI(index);
                    return AddItem(sourceItem, amount - availableSpace);
                }
            }
            else
            {
                return null;
            }
        }

        //If the amount to move is equal to the source item, just move over the source item. Otherwise, make a new object and add it instead.
        RoguelikeObject newStack;
        if (amount >= sourceItem.StackSize)
        {
            newStack = sourceItem;
            if(newStack.ParentInventory) newStack.ParentInventory.RemoveItem(newStack);
        }
        else
        {
            newStack = RoguelikeObject.MakeRoguelikeObjectTemporary(sourceItem, amount);
            sourceItem.StackSize -= amount;
        }

        //else add the item to the inv index
        itemList[index] = newStack;

        if (external)
        {
            //if this is an external inv, the item is dropped meaning, among other things, it will be visible in the world and located at the external inv's location.
            newStack.Exposed = true;
        }
        else
        {
            //else the item is obtains and will not show up in the world.
            newStack.Exposed = false;
        }

        newStack.transform.position = transform.position;
        newStack.transform.SetParent(gameObject.transform);

        newStack.ParentInventory = this;
        UpdateInventoryGUI(index);
        return newStack;
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
    public RoguelikeObject MoveItem(int index, Inventory inv, int amount)
    {
        RoguelikeObject item = itemList[index];
        if (amount < 1 || amount >= item.StackSize)
        {
            amount = item.StackSize;
        }

        RoguelikeObject finalStack = inv.AddItem(item, amount);
        UpdateInventoryGUI(index);

        return finalStack;
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public RoguelikeObject MoveItem(RoguelikeObject item, Inventory inv, int amount = -1)
    {
        int index = GetItemIndex(item);
        if (index >= 0) return MoveItem(index, inv, amount);
        return null;
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="index1">The index in this inventory we are moving from.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="index2">The index to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public RoguelikeObject MoveItem(int index1, Inventory inv, int index2, int amount = -1)
    {
        RoguelikeObject item = itemList[index1];
        if (amount < 1 || amount >= item.StackSize)
        {
            amount = item.StackSize;
        }

        RoguelikeObject finalStack = inv.AddItem(item, amount, index2);
        UpdateInventoryGUI(index1);

        return finalStack;
    }

    /// <summary>
    /// Move an item from this inventory to another. Optionally specify an amount to move.
    /// </summary>
    /// <param name="item">The item to move.</param>
    /// <param name="inv">The inventory to move to.</param>
    /// <param name="index2">The index to move to.</param>
    /// <param name="amount">The amount to move. -1 for the whole stack.</param>
    /// <returns></returns>
    public RoguelikeObject MoveItem(RoguelikeObject item, Inventory inv, int index2, int amount)
    {
        int index = GetItemIndex(item);
        if (index >= 0) return MoveItem(index, inv, index2, amount);
        return null;
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
    /// returns true if the inventory is empty
    /// </summary>
    public bool isEmpty()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (itemList[i] != null) return false;
        }
        return true;
    }

    /// <summary>
    /// Update our inventoryGUI.
    /// </summary>
    public void UpdateInventoryGUI(int index = -1)
    {
        foreach(InventoryGUI gui in myInventoryGUIs)
        {
            gui.UpdateFrameContent(index);
        }
    }
}
