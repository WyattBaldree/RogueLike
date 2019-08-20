using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class UnitController : MonoBehaviour
{
    public static List<Unit2> unitList = new List<Unit2>();

    public enum GameStateEnum { enemyTurn, playerTurn }
    public GameStateEnum gameState = GameStateEnum.playerTurn;

    public Player2 player;

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

    public Unit2 GetUnit(Vector2Int pos)
    {
        return (Unit2)GetUnitInventory(pos).GetItem(0);
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
                Unit2 newUnit = GetUnit(position);
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
        foreach (Unit2 u in UnitController.unitList)
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
        Unit2[] list = UnitController.unitList.ToArray();

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

/*public class UnitController : MonoBehaviour
{
    //list of all characters currently in the map
    public List<Unit> unitList;


    public Player myPlayer;

    public enum GameStateEnum {enemyTurn, playerTurn}
    public GameStateEnum gameState = GameStateEnum.playerTurn;

    public Player playerSource;
    public Unit demonSource;

    public void Initialize()
    {
        // the unit list holds all characters currently in the game including the player
        unitList = new List<Unit>();

        //Create a player
        Player player = (Player)Instantiate(playerSource, new Vector3(5, 3), Quaternion.identity, transform);
        player.Initialize();
        unitList.Add(player);
        myPlayer = player;

        //Create a unit
        Unit demon = demonSource;// load the default unit
        
        Unit unit = (Unit)Instantiate(demon, new Vector3(5, 15), Quaternion.identity, transform);
        unit.Initialize();
        unitList.Add(unit);

        unit = (Unit)Instantiate(demon, new Vector3(5, 14), Quaternion.identity, transform);
        unit.Initialize();
        unitList.Add(unit);
    }

    void Update()
    {
        //If it is the player's turn, call the player turn function otherwise, make the npcs tick.
        if (gameState == GameStateEnum.playerTurn)
        {
            myPlayer.Turn();
        }
        else
        {
            Step();
        }
    }

    public void Step()
    {
        //Every step: each unit will get to take it's turn which will involve:
        //reducing their speed counter and potentially taking an action
        
        //When it is the player's turn, the step loop is broken. At the end of 
        //the player's turn, FinishStep is called and the remaining list is processed.
        foreach (Unit u in GetUnitController().unitList)
        {
            if(u is Player)
            {
                ((Player)u).Step();
            }
            else
            {
                u.Step();
            }
        }
    }

    //When it is the player's turn, the step loop is broken. At the end of 
    //the player's turn, FinishStep is called and the remaining list is processed.
    public void FinishStep()
    {
        gameState = GameStateEnum.enemyTurn;

        int playerIndex = unitList.IndexOf(myPlayer);
        Unit[] list = GetUnitController().unitList.ToArray();

        //Finish the step loop where we left off.
        for(int i = playerIndex + 1; i < list.Length; i++)
        {
            if (list[i])
            {
                list[i].Step();
            }
        }

    }
}*/
