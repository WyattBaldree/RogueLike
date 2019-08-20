using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class UnitController : MonoBehaviour
{
    public static List<Unit> unitList = new List<Unit>();

    public enum GameStateEnum { enemyTurn, playerTurn }
    public GameStateEnum gameState = GameStateEnum.playerTurn;

    public Player player;

    [SerializeField]
    private Inventory unitInventoryClass;

    private Inventory[,] unitInventoryArray;

    /// <summary>
    /// On initialization, create all of our unit inventories.
    /// </summary>
    public void Initialize()
    {
        Assert.IsNotNull(unitInventoryClass, "The Unit Inventory Class has not been set.");

        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        // create all item lists
        unitInventoryArray = new Inventory[screenRes.x, screenRes.y];

        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                unitInventoryArray[i, j] = (Inventory)Instantiate(unitInventoryClass, new Vector3(i, j), Quaternion.identity, transform);
            }
        }
    }

    public Inventory GetUnitInventory(Vector2Int pos)
    {
        return unitInventoryArray[pos.x, pos.y];
    }

    public Unit GetUnit(Vector2Int pos)
    {
        return (Unit)GetUnitInventory(pos).GetItem(0);
    }

    public void UpdateAllUnitSprites()
    {
        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        //update all units
        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                Unit newUnit = GetUnit(position);
                if (newUnit)
                {
                    newUnit.UpdateRogueSpriteRenderer();
                }
            }
        }
    }

    public void Step()
    {
        //Every step: each unit will get to take it's turn which will involve:
        //reducing their speed counter and potentially taking an action

        //When it is the player's turn, the step loop is broken. At the end of 
        //the player's turn, FinishStep is called and the remaining list is processed.
        foreach (Unit u in UnitController.unitList)
        {
            u.Step();
        }
    }

    //When it is the player's turn, the step loop is broken. At the end of 
    //the player's turn, FinishStep is called and the remaining list is processed.
    public void FinishStep()
    {
        gameState = GameStateEnum.enemyTurn;

        int playerIndex = unitList.IndexOf(player);
        Unit[] list = UnitController.unitList.ToArray();

        //Finish the step loop where we left off.
        for (int i = playerIndex + 1; i < list.Length; i++)
        {
            if (list[i])
            {
                list[i].Step();
            }
        }

    }
}