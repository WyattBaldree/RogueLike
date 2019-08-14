using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wall2 : WorldObject
{
    public override Inventory GetWorldObjectInventory(Vector2Int pos)
    {
        return GameController.wallC.GetWallInventory(pos);
    }

    public override bool IsSpaceFree(Vector2Int pos)
    {
        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1;
    }
}
