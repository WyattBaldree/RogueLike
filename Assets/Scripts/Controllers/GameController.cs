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
    public static PopupController popupC = null;
    public static WallController wallC = null;

    /// <summary>
    /// The InteractiveGUI class we use for context menues;
    /// </summary>
    public static InteractiveGUI interactiveGUI;

    [Header("Controllers")]
    public MapController mapControllerInstance;
    public UnitController unitControllerInstance;
    public LogController logControllerInstance;
    public ItemController itemControllerInstance;
    public WallController wallControllerInstance;

    public InventoryController inventoryControllerInstance;
    public PopupController popupControllerInstance;

    [Header("Misc")]
    public bool debug = false;

    //How many blocks wide and tall the play field is.
    public Vector2Int ScreenResInUnits = new Vector2Int(20, 20);
    [System.NonSerialized]
    public Vector2 ScreenResInPixels;

    [SerializeField]
    private float spriteToggleTime = .1f;

    /// <summary>
    /// An instance of an inventory that objects can be instantiated into before being placed into the world. When something is put in here, it should immediately be removed.
    /// </summary>
    public Inventory temporaryInventory;

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
        //mapC = mapControllerInstance;
        wallC = wallControllerInstance;
        itemC = itemControllerInstance;
        unitC = unitControllerInstance;
        logC = logControllerInstance;

        inventoryC = inventoryControllerInstance;
        popupC = popupControllerInstance;

        logC.Initialize();
        wallC.Initialize();
        //mapC.Initialize(Camera.main);
        inventoryC.Initialize();
        itemC.Initialize();
        unitC.Initialize();

        inventoryC.InitializePickupDrops();

        Pathfinding.GenerateToPlayerMap();
        Pathfinding.GenerateToGoldMap();
        Pathfinding.GenerateFleePlayerMap();
        StartCoroutine("ToggleRogulikeObjectSprites");
    }

    IEnumerator ToggleRogulikeObjectSprites()
    {
        while (true)
        {
            foreach (RoguelikeObject rlObject in RoguelikeObject.roguelikeObjectList)
            {
                rlObject.SpriteToggle = !rlObject.SpriteToggle;
            }
            yield return new WaitForSeconds(spriteToggleTime);
        }
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
