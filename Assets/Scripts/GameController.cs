using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController gameC;
    public static MapController mapC;
    public static UnitController unitC;
    public static LogController logC;
    public static InventoryController inventoryC;
    public static ItemController itemC;

    public MapController mapControllerClass;
    public UnitController unitControllerClass;
    public LogController logControllerClass;
    public ItemController itemControllerClass;

    public InventoryController inventoryControllerInstance;

    public bool debug = false;

    //How many blocks wide and tall the play field is.
    public Vector2 ScreenResInUnits = new Vector2(20, 20);
    [System.NonSerialized]
    public Vector2 ScreenResInPixels;


    public static void UpdateGUId()
    {
        //GameController.inventoryC.UpdateFrameSprites();
    }

    // Use this for initialization
    void Start()
    {
        //Set our Screen res in pixels then adjust our camera accordingly
        ScreenResInPixels = new Vector2(ScreenResInUnits.x, ScreenResInUnits.y);

        gameC = this;

        inventoryC = inventoryControllerInstance;
        inventoryC.Initialize();

        mapC = (MapController)Instantiate(mapControllerClass);
        mapC.Initialize(Camera.main);

        itemC = (ItemController)Instantiate(itemControllerClass);
        itemC.Initialize();

        unitC = (UnitController)Instantiate(unitControllerClass);
        unitC.Initialize();

        logC = (LogController)Instantiate(logControllerClass);
        logC.Initialize();


        inventoryC.InitializePickupDrops();



        Pathfinding.GenerateToPlayerMap();
        Pathfinding.GenerateToGoldMap();
        Pathfinding.GenerateFleePlayerMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
