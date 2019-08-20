using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class Wall2 : WorldObject
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
        return GetWallController().GetWallInventory(pos);
    }

    /// <summary>
    /// Returns true if the supplied position does not already have a wall in it.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public override bool IsSpaceFree(Vector2Int pos)
    {
        if (!base.IsSpaceFree(pos)) return false;
        Floor2 floor = GetFloorController().GetFloor(pos);
        bool supportedByFloor = floor && floor.CanSupportWall;

        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1 && supportedByFloor;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        WallController.wallList.Add(this);
    }

    public override void DestroyObject()
    {
        Assert.IsTrue(WallController.wallList.Remove(this), "The roguelikeObject being destroyed was not in the RoguelikeObjectList upon being destroyed. Something is terribly wrong.");
        base.DestroyObject();
    }
}
