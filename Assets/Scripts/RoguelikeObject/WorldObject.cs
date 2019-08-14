using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : RoguelikeObject
{
    [Header("World Object")]
    [SerializeField]
    private Sprite worldSprite;
    /// <summary>
    /// The current sprite we are displaying IN THE WORLD (this is not the same as the item sprite)
    /// </summary>
    public Sprite WorldSprite
    {
        get => worldSprite;
        set
        {
            worldSprite = value;
            UpdateRogueSpriteRenderer();
        }
    }

    private bool placed = false;
    /// <summary>
    /// This value is true while the world object is placed in the world. So if a unit is in the world, a wall is built, or a floor is built this will
    /// be true.
    /// </summary>
    public bool Placed
    {
        get => placed;
        set
        {
            placed = value;
            UpdateRogueSpriteRenderer();
        }
    }

    public override Sprite GetCurrentSprite()
    {
        if (Placed)
        {
            return WorldSprite;
        }
        else
        {
            return ItemSprite;
        }
    }

    /// <summary>
    /// This function returns whether a given position is available to be moved into by this object. For walls, this function will typically check if 
    /// there is an existing wall while for a unit, this function will check if there is an existing unit ect.
    /// </summary>
    /// <returns>Returns true if the space is available for this object to move to.</returns>
    public abstract bool IsSpaceFree(Vector2Int pos);

    /// <summary>
    /// Return the world inventory at the supplied position for this object type. For walls, this will check the wallController.GetWallInventory(pos);
    /// </summary>
    /// <returns></returns>
    public abstract Inventory GetWorldObjectInventory(Vector2Int pos);

    /// <summary>
    /// This function checks if the supplied position is available for the world object (which depends on the object's implementation of IsSpaceFree),
    /// sets Placed to True, then calls the implementation of PlaceInWorld (which is implemented by the child).
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public virtual bool Place(Vector2Int pos)
    {
        //If the space we are trying to place at is free (as determined by the implemtation).
        if (IsSpaceFree(pos))
        {
            //We are moving the item into the world so set Placed to true;
            Placed = true;
            //Get the inventory that we will be moving the object into (as determined by the implementation).
            Inventory targetInventory = GetWorldObjectInventory(pos);
            //Move the item into that inventory. This will automatically move the WorldObject to that inventory's location.
            MyInventory.MoveItem(this, targetInventory, 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This function moves the world object from a worldInventory to an inventory (and sets Placed to false)
    /// </summary>
    /// <param name="targetInventory">The inventory to move to.</param>
    /// <param name="index">Optionally specify the inventory index to move to.</param>
    /// <returns></returns>
    public virtual bool Take(Inventory targetInventory, int index = -1)
    {
        //Were we able to add to the inventory?
        bool wasAddedToInventory = false;

        //Attempt to move the worldObject
        if (index < 0)
        {
            wasAddedToInventory = MyInventory.MoveItem(this, targetInventory);
        }
        else
        {
            wasAddedToInventory = MyInventory.MoveItem(this, targetInventory, index, -1);
        }

        //Update placed
        Placed = !wasAddedToInventory;
        return wasAddedToInventory;
    }
}
