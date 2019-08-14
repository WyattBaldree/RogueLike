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
                inventoryArray[i, j].Initialize();
            }
        }
        WorldObject yo;
        for (int i = 0; i < 1; i++)
        {
            RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5 + i, 6]);
            RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5 + i, 6]);
        }
        yo = (WorldObject)RoguelikeObject.MakeItem(itemArray[1], 1, inventoryArray[5, 6]);
        yo.Place(new Vector2Int(6, 7));
        yo.Take(inventoryArray[7, 7]);

        //MakeItem(itemArray[1], 50, inventoryArray[7, 12]);
        //MakeItem(itemArray[2], 50, inventoryArray[7, 12]);


        /*MakeItem(ironCapSource, 50, inventoryArray[7, 13]);
        MakeItem(ringmailSource, 50, inventoryArray[7, 13]);
        MakeItem(leatherGlovesSource, 50, inventoryArray[7, 13]);
        MakeItem(bootsSource, 50, inventoryArray[7, 13]);

        MakeItem(bootsSource, 1, ((Chest)(GameController.mapC.wallMapArray[3,2])).inventory);
        MakeItem(ironCapSource, 1, ((Chest)(GameController.mapC.wallMapArray[3, 2])).inventory);
        MakeItem(leatherGlovesSource, 1, ((Chest)(GameController.mapC.wallMapArray[3, 2])).inventory);
        MakeItem(ringmailSource, 1, ((Chest)(GameController.mapC.wallMapArray[3, 2])).inventory);*/
    }
}
