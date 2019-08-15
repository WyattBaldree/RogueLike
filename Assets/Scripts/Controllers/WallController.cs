using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WallController : MonoBehaviour
{
    [SerializeField]
    private Inventory wallInventoryClass;

    [SerializeField]
    private Wall2 wallClass;

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
            }
        }

        //create all walls
        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                WorldObject.MakeAndPlaceWorldObject(wallClass, position);
            }
        }

        Wall2 thisWall;
        //cut out rooms temporary
        for (int i = 2; i < 6; i++)
        {
            for (int j = 2; j < 4; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                thisWall = GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        for (int i = 5; i < 12; i++)
        {
            for (int j = 5; j < 9; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                thisWall = GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        for (int i = 3; i < 10; i++)
        {
            for (int j = 14; j < 17; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                thisWall = GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        for (int i = 7; i < 8; i++)
        {
            for (int j = 7; j < 15; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                thisWall = GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        for (int i = 6; i < 14; i++)
        {
            for (int j = 6; j < 14; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                thisWall = GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        thisWall = GetWall(new Vector2Int(5, 4));
        if (thisWall)
        {
            thisWall.DestroyObject();
        }

        //create all walls
        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                Wall2 newWall = GetWall(position);
                if (newWall)
                {
                    newWall.UpdateRogueSpriteRenderer();
                }
            }
        }
    }

    public Inventory GetWallInventory(Vector2Int pos)
    {
        return wallInventoryArray[pos.x, pos.y];
    }

    public Wall2 GetWall(Vector2Int pos)
    {
        return (Wall2)GetWallInventory(pos).GetItem(0);
    }
}
