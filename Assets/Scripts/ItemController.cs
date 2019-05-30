using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item[] itemArray;

    [System.NonSerialized]

    ///An array of all the floor inventories.
    public Inventory[,] inventoryArray;
    
    ///The ground Inventory source
    public Inventory groundInventorySource;

    /// sources for several items that can be added to the game DEBUG
    public Armor ironCapSource;
    public Armor ringmailSource;
    public Armor leatherGlovesSource;
    public Armor bootsSource;
    
    /// <summary>
    /// On initialization, create all of our ground inventories and put in some debug items.
    /// </summary>
    public void Initialize()
    {
        // create all item lists
        inventoryArray = new Inventory[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                //Item item = (Item)Instantiate(item, new Vector3(i, j), Quaternion.identity);
                inventoryArray[i, j] = (Inventory)Instantiate(groundInventorySource, new Vector3(i, j), Quaternion.identity, transform);
                inventoryArray[i, j].Initialize();
            }
        }

        for (int i = 0; i < 1; i++)
        {
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
            MakeItem(itemArray[0], 50, inventoryArray[5 + i, 6]);
        }


        MakeItem(itemArray[1], 50, inventoryArray[7, 12]);
        MakeItem(itemArray[2], 50, inventoryArray[7, 12]);


        MakeItem(ironCapSource, 50, inventoryArray[7, 13]);
        MakeItem(ringmailSource, 50, inventoryArray[7, 13]);
        MakeItem(leatherGlovesSource, 50, inventoryArray[7, 13]);
        MakeItem(bootsSource, 50, inventoryArray[7, 13]);
    }

    /// <summary>
    /// A function that is used to create a new item.
    /// </summary>
    /// <param name="source">The source of the item being made.</param>
    /// <param name="amount">The stack size of the new item.</param>
    /// <param name="destination">The inventory the item is going to begin in.</param>
    /// <param name="index">The inventory index to put the new item in. -1 for the first available spot.</param>
    /// <returns></returns>
    public static Item MakeItem(Item source, int amount, Inventory destination, int index = -1)
    {
        Item newItem = Instantiate<Item>(source, destination.transform.position, Quaternion.identity, destination.transform);
        newItem.Initialize();

        if(amount < 0)
        {
            newItem.SetStackSize(1);
        }
        else if (amount <= newItem.maxStackSize)
        {
            newItem.SetStackSize(amount);
        }
        else
        {
            newItem.SetStackSize(newItem.maxStackSize);
        }

        bool wasAdded;
        if(index > -1)
        {
            wasAdded = destination.AddItem(newItem, index);
        }
        else
        {
            wasAdded = destination.AddItem(newItem);
        }

        if (wasAdded)
        {
            return newItem;
        }
        else
        {
            Destroy(newItem.gameObject);
            return null;
        }
    }
}
