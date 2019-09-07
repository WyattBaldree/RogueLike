using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public abstract class RoguelikeObject : Interactive
{
    /* The RoguelikeObject class represents any object that can be interacted with in the world.
     * All RoguelikeObjects can be added to an inventory including the player's inventory, containers, and any ground space.
     * Since all interactable objects will be children of this object, this object also contains information that all objects
     * share like: a name, a description, weight, ect.
     */

    public static List<RoguelikeObject> roguelikeObjectList = new List<RoguelikeObject>();

    /// <summary>
    /// The max amount of "on fire" possible
    /// </summary>
    private static int fireAmountMax = 3;

    public enum TagEnum { item }

    [Header("Roguelike Object")]
    
    ///A list of tags that apply to this object.
    public List<TagEnum> tags = new List<TagEnum>() { TagEnum.item };

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

    /// <summary>
    /// PRIVATE. The sprite renderer that this object uses in the world. This sprite renderer is used for showing an item lying on the ground, a wall placed in the world, or a floor placed on the ground, ect.
    /// </summary>
    [SerializeField]
    public RogueSpriteRenderer myRogueSpriteRenderer;

    [SerializeField]
    private Sprite itemSprite1;
    /// <summary>
    /// The sprite we display IN INVENTORIES when SpriteToggle is false (this is not the same as the world sprite for wall/floor/units)
    /// </summary>
    public Sprite ItemSprite1
    {
        get => itemSprite1;
        set
        {
            itemSprite1 = value;
            UpdateRogueSpriteRenderer();
        }
    }

    [SerializeField]
    private Sprite itemSprite2;
    /// <summary>
    /// The sprite we display IN INVENTORIES when SpriteToggle is true (this is not the same as the world sprite for wall/floor/units)
    /// </summary>
    public Sprite ItemSprite2
    {
        get => itemSprite2;
        set
        {
            itemSprite2 = value;
            UpdateRogueSpriteRenderer();
        }
    }

    private bool spriteToggle = false;
    /// <summary>
    /// When this value is toggled, it swaps the objectect from sprite 1 to sprite 2 or visa-versa
    /// </summary>
    public bool SpriteToggle
    {
        get => spriteToggle;
        set
        {
            spriteToggle = value;
            UpdateRogueSpriteRenderer();
        }
    }

    [SerializeField]
    private float animationSpeed = .7f;
    /// <summary>
    /// This float controlls how fast the object toggles between it's 2 sprites. The lower this value, the faster the toggling.
    /// </summary>
    public float AnimationSpeed
    {
        get => animationSpeed;
        set => animationSpeed = value;
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
                Die();
            }
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
            UpdateRogueSpriteRenderer();
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
    
    private Inventory parentInventory;
    /// <summary>
    /// The inventory we are currently located in.
    /// </summary>
    public Inventory ParentInventory
    {
        get => parentInventory;
        set => parentInventory = value;
    }

    [SerializeField]
    private Inventory myTnventory;
    /// <summary>
    /// The internal inventory of this object.
    /// </summary>
    public Inventory MyInventory { get => myTnventory; }

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
    /// A function that is used to create a new RoguelikeObject and immediately place it into an inventory.
    /// </summary>
    /// <param name="source">The source of the RoguelikeObject being made.</param>
    /// <param name="amount">The stack size of the new item.</param>
    /// <param name="destination">The inventory the item is going to begin in.</param>
    /// <param name="index">The inventory index to put the new item in. -1 for the first available spot.</param>
    /// <returns>Returns a reference to the newly created RoguelikeObject</returns>
    public static RoguelikeObject MakeRoguelikeObject(RoguelikeObject source, int amount, Inventory destination, int index = -1)
    {
        //Create a new rogulikeObject and immdiately add it to the temporary inventory
        RoguelikeObject newItem = MakeRoguelikeObjectTemporary(source, amount);
        Inventory tempInv = GetGameController().temporaryInventory;
        tempInv.AddItem(newItem);

        //Attempt to move all of the item from the temporary inventory to the real inventory and get the final stack we end on.
        RoguelikeObject finalStack;
        if (index > -1)
        {
            finalStack = tempInv.MoveItem(newItem, destination, index, newItem.StackSize);
        }
        else
        {
            finalStack = tempInv.MoveItem(newItem, destination);
        }

        //If we were unable to add the entire stack to the game, remove the rest of the item from the temporary inventory.
        if (finalStack == null)
        {
            newItem.StackSize = 0;
            Assert.IsNull(tempInv.GetItem(0), "The temporary inventory is not empty at the end of MakeRoguelikeObject. Something is VERY wrong!");
            return null;
        }
        else
        {
            Assert.IsNull(tempInv.GetItem(0), "The temporary inventory is not empty at the end of MakeRoguelikeObject. Something is VERY wrong!");
            return finalStack;
        }
    }

    /// <summary>
    /// Thsi function creates and initializes a new item but does not add it to an inventory. This should only be used right before adding the item to an inventory.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static RoguelikeObject MakeRoguelikeObjectTemporary(RoguelikeObject source, int amount)
    {
        RoguelikeObject newItem = Instantiate<RoguelikeObject>(source, GetGameController().temporaryInventory.transform.position, Quaternion.identity, GetGameController().temporaryInventory.transform);
        newItem.Initialize();
        newItem.StackSize = amount;
        newItem.OnCreate();
        return newItem;
    }

    /// <summary>
    /// A coroutine that toggles the between the object's srites.
    /// </summary>
    /// <returns></returns>
    IEnumerator ToggleRogulikeObjectSprites()
    {
        while (true)
        {
            SpriteToggle = !SpriteToggle;
            yield return new WaitForSeconds(AnimationSpeed);
        }
    }
    
    /// <summary>
    /// Get the current position of this object.
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetPosition()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    /// <summary>
    /// Get the current position plus the supplied offset.
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public Vector2Int GetPositionOffset(Vector2Int offset)
    {
        return GetPosition() + offset;
    }

    /// <summary>
    /// Deal damage to the roguelikeObject
    /// </summary>
    /// <param name="source"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(RoguelikeObject source, int damage)
    {
        bool didKill = false;
        if (damage >= Health)
        {
            didKill = true;
        }

        if (source)
        {
            Vector2Int positionDelta = GetPosition() - source.GetPosition();
            myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.BounceAnimation, 7, positionDelta.x, positionDelta.y, .4f);
        }

        Health -= damage;
        return didKill;
    }

    /// <summary>
    /// Attempt to open the internal inventory of this object if it has one.
    /// </summary>
    public virtual void OpenInventory()
    {
        if (MyInventory)
        {
            GetPopupController().containerGUI.Popup(GetPosition(), myTnventory);
        }
    }

    /// <summary>
    /// Called when the item is first created.
    /// </summary>
    public virtual void Initialize()
    {
        ItemSprite1 = itemSprite1;
        Health = HealthMax;
    }

    /// <summary>
    /// when this function is called, MyRogueSpriteRenderer is updated based on the objects current values (GetCurrentSprite(), Exposed, ect.)
    /// </summary>
    public virtual void UpdateRogueSpriteRenderer()
    {
        myRogueSpriteRenderer.gameObject.SetActive(Exposed);
        myRogueSpriteRenderer.MySprite = GetCurrentSprite();
        myRogueSpriteRenderer.StackSize = StackSize;
        if (ParentInventory) ParentInventory.UpdateInventoryGUI(ParentInventory.GetItemIndex(this));
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
    /// Returns the current sprite that should be shown by this object based on the values.
    /// </summary>
    /// <returns>The Sprite that should be renderered.</returns>
    public virtual Sprite GetCurrentSprite()
    {
        if (SpriteToggle && ItemSprite2)
        {
            return ItemSprite2;
        }
        else
        {
            return ItemSprite1;
        }
    }

    /// <summary>
    /// Called once per game step
    /// </summary>
    public virtual void Step()
    {

    }

    /// <summary>
    /// Called when the object's health = 0
    /// </summary>
    public virtual void Die()
    {
        health = HealthMax;
        StackSize--;
    }

    /// <summary>
    /// Attempt to dodge an attack. Return true if we were able to dodge.
    /// </summary>
    /// <returns></returns>
    public virtual bool AttemptDodge(RoguelikeObject source)
    {
        return false;
    }

    /// <summary>
    /// Dodge the attack from the source object.
    /// </summary>
    /// <param name="source"></param>
    public virtual void Dodge(RoguelikeObject source)
    {
        if (source)
        {
            Vector2Int positionDelta = GetPosition() - source.GetPosition();
            myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.BounceAnimation, 7, positionDelta.y, positionDelta.x, .7f);
            GetLogController().NewEntry("<d>The " + GetFullName() + "<d> dodges the attack of the " + source.GetFullName() + "<d>.");
        }
    }

    /// <summary>
    /// Called when this RoguelikeObject is created.
    /// </summary>
    public virtual void OnCreate()
    {
        roguelikeObjectList.Add(this);
        StartCoroutine("ToggleRogulikeObjectSprites");
    }

    /// <summary>
    /// Cleanly Destroy the item by removing it from all lists then destroying it.
    /// </summary>
    public virtual void DestroyObject()
    {
        //If we ever try to delete a roguelikeObject and it is not in "MyInventory" something is terribly wrong.
        Assert.IsTrue(ParentInventory.RemoveItem(this), "The roguelikeObject is not in it's own MyInventory upon attempting to delete itself.");
        Assert.IsTrue(roguelikeObjectList.Remove(this), "The roguelikeObject being destroyed was not in the RoguelikeObjectList upon being destroyed. Something is terribly wrong.");
        Destroy(this.gameObject);
    }

    public override List<Interaction> GetInteractions()
    {
        List<Interaction> newInteractionList = new List<Interaction>();

        newInteractionList.Add(new Interaction("<color.red>Kill", "Kill the object.", Die));
        if(MyInventory) newInteractionList.Add(new Interaction("Open Inventory", "Opens the object's inventory.", OpenInventory));

        return newInteractionList;
    }

    public override string GetInteractiveName()
    {
        return GetFullName();
    }
}
