﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // All of the static controller references. These are used to reference the controllers from anywhere in the game.
    private static GameController gameC = null;
    private static FloorController floorC = null;
    private static WallController wallC = null;
    private static ItemController itemC = null;
    private static UnitController unitC = null;

    private static LogController logC = null;
    private static PopupController popupC = null;
    private static GUIController guiC = null;

    // All of the controller instances in the scene.
    [Header("Controllers")]
    public FloorController floorControllerInstance;
    public WallController wallControllerInstance;
    public ItemController itemControllerInstance;
    public UnitController unitControllerInstance;

    public LogController logControllerInstance;
    public PopupController popupControllerInstance;
    public GUIController guiControllerInstance;

    [Header("Misc")]
    public bool debug = false;

    //How many blocks wide and tall the play field is.
    public Vector2Int ScreenResInUnits = new Vector2Int(20, 20);

    /// <summary>
    /// An instance of an inventory that objects can be instantiated into before being placed into the world. When something is put in here, it should immediately be removed.
    /// </summary>
    public Inventory temporaryInventory;

    [SerializeField]
    private Floor2 floorClass;

    [SerializeField]
    private Wall2 wallClass;

    [SerializeField]
    private Unit2 unitClass;

    [SerializeField]
    private Player2 playerClass;

    // Use this for initialization of the entire game
    void Start()
    {
        gameC = this;
        floorC = floorControllerInstance;
        wallC = wallControllerInstance;
        itemC = itemControllerInstance;
        unitC = unitControllerInstance;

        logC = logControllerInstance;
        popupC = popupControllerInstance;
        guiC = guiControllerInstance;

        //Initalize the controllers that require initialization
        logC.Initialize();
        floorC.Initialize();
        wallC.Initialize();
        itemC.Initialize();
        unitC.Initialize();

        CreateTestMap();
        floorC.UpdateAllFloorSprites();
        wallC.UpdateAllWallSprites();

        Pathfinding.GenerateToPlayerMap();
        Pathfinding.GenerateToGoldMap();
        Pathfinding.GenerateFleePlayerMap();
    }

    /// <summary>
    /// Returns the mosue position on the screen.
    /// </summary>
    /// <returns></returns>
    static public Vector3 GetMousePosition()
    {
        Camera cam = Camera.main;
        
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        return cam.ScreenToWorldPoint(new Vector3(mousePos.x - 25f, mousePos.y - 25f, cam.nearClipPlane));
    }

    public static GameController GetGameController()
    {
        return gameC;
    }

    public static UnitController GetUnitController()
    {
        return unitC;
    }

    public static LogController GetLogController()
    {
        return logC;
    }

    public static GUIController GetGUIController()
    {
        return guiC;
    }

    public static ItemController GetItemController()
    {
        return itemC;
    }

    public static PopupController GetPopupController()
    {
        return popupC;
    }

    public static WallController GetWallController()
    {
        return wallC;
    }

    public static FloorController GetFloorController()
    {
        return floorC;
    }

    void Update()
    {
        //If it is the player's turn, call the player turn function otherwise, make the npcs tick.
        if (unitC.gameState == UnitController.GameStateEnum.playerTurn)
        {
            unitC.player.Turn();
        }
        else
        {
            unitC.Step();
        }
    }

    void CreateTestMap()
    {
        Vector2Int screenRes = ScreenResInUnits;
        //create all floors
        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                WorldObject.MakeAndPlaceWorldObject(floorClass, position);
            }
        }

        //create all walls
        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
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
                thisWall = GetWallController().GetWall(position);
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
                thisWall = GetWallController().GetWall(position);
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
                thisWall = GetWallController().GetWall(position);
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
                thisWall = GetWallController().GetWall(position);
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
                thisWall = GetWallController().GetWall(position);
                if (thisWall)
                {
                    thisWall.DestroyObject();
                }
            }
        }

        thisWall = GetWallController().GetWall(new Vector2Int(5, 4));
        if (thisWall)
        {
            thisWall.DestroyObject();
        }

        WorldObject.MakeAndPlaceWorldObject(unitClass, new Vector2Int(5, 15));
        WorldObject.MakeAndPlaceWorldObject(playerClass, new Vector2Int(5, 3));
    }

}
