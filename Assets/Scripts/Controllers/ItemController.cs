using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class ItemController : MonoBehaviour
{
    public RoguelikeObject[] itemArray;

    [System.NonSerialized]

    ///An array of all the floor inventories.
    public Inventory[,] inventoryArray;
    
    ///The ground Inventory source
    public Inventory groundInventorySource;
    
    /// <summary>
    /// On initialization, create all of our ground inventories and put in some debug items.
    /// </summary>
    public void Initialize()
    {
        Vector2Int screenRes = GetGameController().ScreenResInUnits;
        // create all item lists
        inventoryArray = new Inventory[screenRes.x, screenRes.y];

        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
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
