using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wall2 : WorldObject
{
    [Header("Wall")]
    [SerializeField]
    private bool blockUnits = true;
    /// <summary>
    /// This bool determins if units can move through this wall or not.
    /// </summary>
    public bool BlockUnits
    {
        get => blockUnits;
        set => blockUnits = value;
    }

    [SerializeField]
    private bool influenceFloors = true;
    /// <summary>
    /// This bool determines if floor sprites are effected by this wall's presence. (A brickwall will effect floors while a door will not.
    /// The floor will appear to run under the door.)
    /// </summary>
    public bool InfluenceFloors
    {
        get => influenceFloors;
    }

    /// <summary>
    /// Returns the inventory at the supplied position in the WallController.wallInventoryArray[,]
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public override Inventory GetWorldObjectInventory(Vector2Int pos)
    {
        return GameController.wallC.GetWallInventory(pos);
    }

    /// <summary>
    /// Returns true if the supplied position does not already have a wall in it.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public override bool IsSpaceFree(Vector2Int pos)
    {
        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1;
    }
    
}
