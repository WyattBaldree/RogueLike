using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class Floor : WorldObject
{
    [Header("Floor")]
    [SerializeField]
    private bool canSupportWall;
    /// <summary>
    /// Can a wall be built on top of this floor?
    /// </summary>
    public bool CanSupportWall
    {
        get => canSupportWall;
    }

    public override Inventory GetWorldObjectInventory(Vector2Int pos)
    {
        return GetFloorController().GetFloorInventory(pos);
    }

    public override bool IsSpaceFree(Vector2Int pos)
    {
        if (!base.IsSpaceFree(pos)) return false;
        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        FloorController.floorList.Add(this);
    }

    public override void DestroyObject()
    {
        Assert.IsTrue(FloorController.floorList.Remove(this), "The roguelikeObject being destroyed was not in the RoguelikeObjectList upon being destroyed. Something is terribly wrong.");
        base.DestroyObject();
    }
}
