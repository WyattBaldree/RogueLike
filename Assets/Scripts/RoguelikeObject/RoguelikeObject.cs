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

    [Header("Roguelike Object")]

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
            UpdateRogueSpriteRenderer();
        }
    }

    /// <summary>
    /// PRIVATE. The sprite renderer that this object uses in the world. This sprite renderer is used for showing an item lying on the ground, a wall placed in the world, or a floor placed on the ground, ect.
    /// </summary>
    [SerializeField]
    protected RogueSpriteRenderer myRogueSpriteRenderer;


    public enum TagEnum { item }

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

    [SerializeField]
    private int stackSizeMax = 1;
    /// <summary>
    /// READ ONLY. The maximum stack size for this object.
    /// </summary>
    public int StackSizeMax
    {
        get => stackSizeMax;
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
    private int goldValue = 1;
    /// <summary>
    /// READ ONLY. This represents the base sell/buy price of the object. The final buy/sell price will, of course, be effect by things like charisma.
    /// </summary>
    public int GoldValue
    {
        get => goldValue;
    }

    [SerializeField]
    private int healthMax = 10;
    /// <summary>
    /// READ ONLY. The maximum health of this object.
    /// </summary>
    public int HealthMax
    {
        get => healthMax;
    }

    private int health;
    /// <summary>
    /// The current healt of this object. When this value reaches 0 decrement the stackSize by 1 (health only represents the hitpoints of the item on top of the stack).
    /// </summary>
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if(health <= 0)
            {
                health = HealthMax;
                StackSize--;
            }
        }
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
            UpdateRogueSpriteRenderer();
        }
    }

    /// <summary>
    /// Called when the item is first created.
    /// </summary>
    public virtual void Initialize()
    {
        ItemSprite = itemSprite;
        Health = HealthMax;
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
    /// when this function is called, MyRogueSpriteRenderer is updated based on the objects current values (GetCurrentSprite(), Exposed, ect.)
    /// </summary>
    public void UpdateRogueSpriteRenderer()
    {
        myRogueSpriteRenderer.gameObject.SetActive(Exposed);
        myRogueSpriteRenderer.MySprite = GetCurrentSprite();
        myRogueSpriteRenderer.StackSize = StackSize;
    }

    /// <summary>
    /// Returns the current sprite that should be shown by this object based on the values.
    /// </summary>
    /// <returns>The Sprite that should be renderered.</returns>
    public virtual Sprite GetCurrentSprite()
    {
        return ItemSprite;
    }

    /// <summary>
    /// Cleanly Destroy the item by removing it from all lists, updating all graphics, then destroying it.
    /// </summary>
    public virtual void DestroyObject()
    {
        //If we ever try to delete a roguelikeObject and it is not in "MyInventory" something is terribly wrong.
        Assert.IsTrue(MyInventory.RemoveItem(this), "The roguelikeObject is not in it's own MyInventory upon attempting to delete itself.");
        Destroy(this.gameObject);
    }

    /// <summary>
    /// A function that is used to create a new RoguelikeObject.
    /// </summary>
    /// <param name="source">The source of the RoguelikeObject being made.</param>
    /// <param name="amount">The stack size of the new item.</param>
    /// <param name="destination">The inventory the item is going to begin in.</param>
    /// <param name="index">The inventory index to put the new item in. -1 for the first available spot.</param>
    /// <returns>Returns a reference to the newly created RoguelikeObject</returns>
    public static RoguelikeObject MakeRoguelikeObject(RoguelikeObject source, int amount, Inventory destination, int index = -1)
    {
        RoguelikeObject newItem = Instantiate<RoguelikeObject>(source, destination.transform.position, Quaternion.identity, destination.transform);
        newItem.Initialize();

        newItem.StackSize = amount;

        //Attempt to add the new RoguelikeObject to the destination inventory
        bool wasAdded;
        if (index > -1)
        {
            wasAdded = destination.AddItem(newItem, index);
        }
        else
        {
            wasAdded = destination.AddItem(newItem);
        }

        //If we were able to add the item to the destination inventory, return it. Otherwise, destroy it and return null.
        if (wasAdded)
        {
            return newItem;
        }
        else
        {
            Destroy(newItem.gameObject);
            return null;
        }
    }
}
