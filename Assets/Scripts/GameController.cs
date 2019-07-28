using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController gameC = null;
    public static MapController mapC = null;
    public static UnitController unitC = null;
    public static LogController logC = null;
    public static InventoryController inventoryC = null;
    public static ItemController itemC = null;

    /// <summary>
    /// The InteractiveGUI class we use for context menues;
    /// </summary>
    public static InteractiveGUI interactiveGUI;

    public MapController mapControllerClass;
    public UnitController unitControllerClass;
    public LogController logControllerClass;
    public ItemController itemControllerClass;

    public InventoryController inventoryControllerInstance;
    public InteractiveGUI interactiveGUIInstance;

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
        mapC = (MapController)Instantiate(mapControllerClass);
        itemC = (ItemController)Instantiate(itemControllerClass);
        unitC = (UnitController)Instantiate(unitControllerClass);
        logC = (LogController)Instantiate(logControllerClass);

        inventoryC = inventoryControllerInstance;
        interactiveGUI = interactiveGUIInstance;

        logC.Initialize();
        mapC.Initialize(Camera.main);
        inventoryC.Initialize();
        itemC.Initialize();
        unitC.Initialize();


        inventoryC.InitializePickupDrops();



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

    // Update is called once per frame
    void Update()
    {

    }

}
