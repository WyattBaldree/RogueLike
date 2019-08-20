using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class FloorController : MonoBehaviour
{
    public static List<Floor2> floorList = new List<Floor2>();

    [SerializeField]
    private Inventory floorInventoryClass;

    private Inventory[,] floorInventoryArray;

    /// <summary>
    /// On initialization, create all of our floor inventories and put in some debug items.
    /// </summary>
    public void Initialize()
    {
        Assert.IsNotNull(floorInventoryClass, "The Floor Inventory Class has not been set.");

        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        // create all item lists
        floorInventoryArray = new Inventory[screenRes.x, screenRes.y];

        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                floorInventoryArray[i, j] = (Inventory)Instantiate(floorInventoryClass, new Vector3(i, j), Quaternion.identity, transform);
            }
        }
    }

    public Inventory GetFloorInventory(Vector2Int pos)
    {
        return floorInventoryArray[pos.x, pos.y];
    }

    public Floor2 GetFloor(Vector2Int pos)
    {
        return (Floor2)GetFloorInventory(pos).GetItem(0);
    }

    public void UpdateAllFloorSprites()
    {
        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        //update all floors
        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                Floor2 newWall = GetFloor(position);
                if (newWall)
                {
                    newWall.UpdateRogueSpriteRenderer();
                }
            }
        }
    }
}
