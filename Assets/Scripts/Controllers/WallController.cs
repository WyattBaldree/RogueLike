using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class WallController : MonoBehaviour
{
    public static List<Wall> wallList = new List<Wall>();

    [SerializeField]
    private Inventory wallInventoryClass;

    private Inventory[,] wallInventoryArray;

    /// <summary>
    /// On initialization, create all of our wall inventories and put in some debug items.
    /// </summary>
    public void Initialize()
    {
        Assert.IsNotNull(wallInventoryClass, "The Wall Inventory Class has not been set.");

        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        // create all item lists
        wallInventoryArray = new Inventory[screenRes.x, screenRes.y];

        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                wallInventoryArray[i, j] = (Inventory)Instantiate(wallInventoryClass, new Vector3(i, j), Quaternion.identity, transform);
            }
        }
    }

    public Inventory GetWallInventory(Vector2Int pos)
    {
        return wallInventoryArray[pos.x, pos.y];
    }

    public Wall GetWall(Vector2Int pos)
    {
        return (Wall)GetWallInventory(pos).GetItem(0);
    }

    public void UpdateAllWallSprites()
    {
        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        //update all walls
        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                Wall newWall = GetWall(position);
                if (newWall)
                {
                    newWall.UpdateRogueSpriteRenderer();
                }
            }
        }
    }
}
