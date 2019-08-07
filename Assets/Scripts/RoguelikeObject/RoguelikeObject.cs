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

    [SerializeField]
    private int stackSize;
    /// <summary>
    /// This value represents the current stack size of the object. That is, when this object is in an inventory,
    /// how many copies of it are in a single inventory space. The best example is a stack of arrows.
    /// </summary>
    public int StackSize
    {
        get => stackSize;
        set
        {
            Assert.IsTrue(value <= StackSizeMax, "There was an attemp to set the stack size of a RoguelikeObject to a value greater than its stackSizeMax!");
            Assert.IsTrue(value >= 0,            "There was an attemp to set the stack size of a RoguelikeObject to a value less than 0!");
            stackSize = Mathf.Clamp(value, 0, StackSizeMax);
        }
    }

    [SerializeField]
    private int stackSizeMax;
    /// <summary>
    /// READ ONLY. The maximum stack size for this object.
    /// </summary>
    public int StackSizeMax
    {
        get => stackSizeMax;
    }

    [SerializeField]
    private int goldValue;
    /// <summary>
    /// READ ONLY. This represents the base sell/buy price of the object. The final buy/sell price will, of course, be effect by things like charisma.
    /// </summary>
    public int GoldValue
    {
        get => goldValue;
    }

    [SerializeField]
    public static int weight;
    /// <summary>
    /// READ ONLY. This represents the base sell/buy price of the object. The final buy/sell price will, of course, be effect by things like charisma.
    /// </summary>
    public int Weight
    {
        get => weight;
    }

    [SerializeField]
    private Sprite itemSprite;
    /// <summary>
    /// The current sprite we are displaying IN INVENTORIES (this is not the same as the world sprite for wall/floor/units)
    /// </summary>
    public Sprite ItemSprite
    {
        get => itemSprite;
        set => itemSprite = value;
    }

    /// <summary>
    /// PRIVATE. The sprite renderer that this object uses in the world as an ITEM. This sprite renderer is only used for showing an item lying on the ground in the world.
    /// </summary>
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    
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
            mySpriteRenderer.gameObject.SetActive(exposed);
        }
    }
}
