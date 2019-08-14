using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WallController : MonoBehaviour
{
    [SerializeField]
    private Inventory wallInventoryClass;

    private Inventory[,] wallInventoryArray;

    /// <summary>
    /// On initialization, create all of our ground inventories and put in some debug items.
    /// </summary>
    public void Initialize()
    {
        Assert.IsNotNull(wallInventoryClass, "The Wall Inventory Class has not been set.");

        // create all item lists
        wallInventoryArray = new Inventory[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                wallInventoryArray[i, j] = (Inventory)Instantiate(wallInventoryClass, new Vector3(i, j), Quaternion.identity, transform);
                wallInventoryArray[i, j].Initialize();
            }
        }
    }

    public Inventory GetWallInventory(Vector2Int pos)
    {
        return wallInventoryArray[pos.x, pos.y];
    }
}
