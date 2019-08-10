using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class RoguelikeObject : MonoBehaviour
{
    /* The RoguelikeObject class represents any object that can be interacted with in the world.
     * All RoguelikeObjects can be added to an inventory including the player's inventory, containers, and any ground space.
     * Since all interactable objects will be children of this object, this object also contains information that all objects
     * share like: a name, a description, weight, ect.
     */
    public enum TagEnum { item }

    [Header("Roguelike Object")]

    public List<TagEnum> tags = new List<TagEnum>(){TagEnum.item};

    [SerializeField]
    private string uniqueID;
    /// <summary>
    /// READ ONLY. The objectName uniqueID. This ID is used for differentiiating between object that may have the same name. Used for stacking items.
    /// </summary>
    public string UniqueID
    {
        get => uniqueID;
    }

    [SerializeField]
    private string objectName;
    /// <summary>
    /// READ ONLY. The objectName property represents just that, the name of whatever the object is. For example, the name of an arrow would just be "Arrow"
    /// </summary>
    public string ObjectName
    {
        get => objectName;
    }

    [SerializeField]
    private string description;
    /// <summary>
    /// READ ONLY. The description property is a... description of the object. For example, an arrow might have the description:
    /// "A sharp piece of metal, a wooden shaft, and feathers expertly fletched together. Can be fired out of a bow."
    /// </summary>
    public string Description
    {
        get => description;
    }
    
    private Inventory myInventory;
    /// <summary>
    /// READ ONLY. The description property is a... description of the object. For example, an arrow might have the description:
    /// "A sharp piece of metal, a wooden shaft, and feathers expertly fletched together. Can be fired out of a bow."
    /// </summary>
    public Inventory MyInventory
    {
        get => myInventory;
        set => myInventory = value;
    }

    private int stackSize = 0;
    /// <summary>
    /// This value represents the current stack size of the object. That is, when this object is in an inventory,
    /// how many copies of it are in a single inventory space. The best example is a stack of arrows.
    /// </summary>
    public int StackSize
    {
        get => stackSize;
        set
        {
            Assert.IsTrue(value <= StackSizeMax, "There was an attempt to set the stack size of a RoguelikeObject to a value greater than its stackSizeMax!");
            Assert.IsTrue(value >= 0,            "There was an attempt to set the stack size of a RoguelikeObject to a value less than 0!");

            //[NEEDS WORK]
            if (value == 0)
            {
                DestroyObject();
                return;
            }
            stackSize = Mathf.Clamp(value, 0, StackSizeMax);
        }
    }

    [SerializeField]
    private int stackSizeMax = 1;
    /// <summary>
    /// READ ONLY. The maximum stack size for this object.
    /// </summary>
    public int StackSizeMax
    {
        get => stackSizeMax;
    }

    [SerializeField]
    private int goldValue = 1;
    /// <summary>
    /// READ ONLY. This represents the base sell/buy price of the object. The final buy/sell price will, of course, be effect by things like charisma.
    /// </summary>
    public int GoldValue
    {
        get => goldValue;
    }

    [SerializeField]
    public static int weight = 1;
    /// <summary>
    /// READ ONLY. This represents the base sell/buy price of the object. The final buy/sell price will, of course, be effect by things like charisma.
    /// </summary>
    public int Weight
    {
        get => weight;
    }

    [SerializeField]
    private bool flammable = false;
    /// <summary>
    /// Can this object catch on fire?
    /// </summary>
    public bool Flammable
    {
        get => flammable;
    }

    /// <summary>
    /// The max amount of "on fire" possible
    /// </summary>
    private static int fireAmountMax = 3;
    private int fireAmount = 0;
    /// <summary>
    /// How "on fire" is this thing? from
    /// 0 - not on fire
    /// 1 - catching on fire
    /// 2 - on fire
    /// 3 - engulfed in flames
    /// </summary>
    public int FireAmount
    {
        get => fireAmount;
        set
        {
            fireAmount = (int)Mathf.Clamp(value, 0, fireAmountMax);
        }
    }

    [SerializeField]
    private bool explosive = false;
    /// <summary>
    /// When this object catches on fire does it explode?
    /// </summary>
    public bool Explosive
    {
        get => explosive;
        set => explosive = value;
    }
    
    private bool exposed = false;
    /// <summary>
    /// This boolean represents wether this object is currently in an external or internal inventory.
    /// When an object is in an external inventory, it is visible in the world.
    /// The most obvious external inventory is a ground inventory where all of the items are visible on the ground.
    /// </summary>
    public bool Exposed
    {
        get => exposed;
        set
        {
            exposed = value;
            myItemSpriteRenderer.gameObject.SetActive(exposed);
        }
    }

    [SerializeField]
    private Sprite itemSprite;
    /// <summary>
    /// The current sprite we are displaying IN INVENTORIES (this is not the same as the world sprite for wall/floor/units)
    /// </summary>
    public Sprite ItemSprite
    {
        get => itemSprite;
        set
        {
            itemSprite = value;
            myItemSpriteRenderer.ItemSprite = itemSprite;
        }
    }

    /// <summary>
    /// PRIVATE. The sprite renderer that this object uses in the world as an ITEM. This sprite renderer is only used for showing an item lying on the ground in the world.
    /// </summary>
    [SerializeField]
    private ItemSpriteRenderer myItemSpriteRenderer;

    public void Initialize()
    {
        ItemSprite = itemSprite;
    }

    /// <summary>
    /// This function is used to get the name of an object as it appears in game. This will be generated differently depending on the child object.
    /// For Example, a weapon may return its objectName appended with +<echantmentLevel> to indicate that it is enchanted. "Iron Sword +2"
    /// </summary>
    /// <returns>The name of the RoguelikeObject as it appears in game.</returns>
    public virtual string GetFullName()
    {
        //return the plural version if applicable.
        if(stackSize > 1)
        {
            return (StackSize) + " " + ObjectName + "s";
        }
        return ObjectName;
    }

    /// <summary>
    /// Cleanly Destroy the item by removing it from all lists, updating all graphics, then destroying it.
    /// </summary>
    public virtual void DestroyObject()
    {
        Debug.Log("WE NEED TO DESTROY THE ITEMS WHEN THEIR STACK SIZE BECOMES 0!!!!");
        MyInventory.RemoveItem(this);
        Destroy(this.gameObject);
    }
}
