using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public RoguelikeObject[] itemArray;

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
            }
        }
        WorldObject yo;
        for (int i = 0; i < 1; i++)
        {
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[0], 32, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[0], 32, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[0], 32, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeRoguelikeObject(itemArray[2], 1, inventoryArray[5 + i, 6]);
        }
    }

    public Inventory GetInventory(Vector2Int pos)
    {
        return inventoryArray[pos.x, pos.y];
    }
}
