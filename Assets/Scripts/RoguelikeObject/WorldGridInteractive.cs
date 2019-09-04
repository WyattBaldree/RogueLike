using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameController;

public class WorldGridInteractive : Interactive
{
    public override List<Interaction> GetInteractions()
    {
        List<Interaction> newInteractionList = new List<Interaction>();

        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        Wall targetWall = GetWallController().GetWall(targetDestination);
        if (targetWall) newInteractionList.Add(new Interaction(targetWall.GetFullName(), "Opens the wall interaction menu.", OpenWall));

        Floor targetFloor = GetFloorController().GetFloor(targetDestination);
        if (targetFloor) newInteractionList.Add(new Interaction(targetFloor.GetFullName(), "Opens the floor interaction menu.", OpenFloor));

        Unit targetUnit = GetUnitController().GetUnit(targetDestination);
        if (targetUnit) newInteractionList.Add(new Interaction(targetUnit.GetFullName(), "Opens the unit interaction menu.", OpenUnit));

        newInteractionList.Add(new Interaction("Ground Inventory", "Opens the ground inventory.", OpenGroundInventory));

        return newInteractionList;
    }

    private void OpenWall()
    {
        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Wall targetWall = GetWallController().GetWall(targetDestination);
        ContextMenuGUI contextMenu = GetPopupController().contextMenuGUI;
        if (targetWall) contextMenu.Popup(contextMenu.transform.position, targetWall);
    }

    private void OpenFloor()
    {
        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Floor targetFloor = GetFloorController().GetFloor(targetDestination);
        ContextMenuGUI contextMenu = GetPopupController().contextMenuGUI;
        if (targetFloor) contextMenu.Popup(contextMenu.transform.position, targetFloor);
    }

    private void OpenUnit()
    {
        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Unit targetUnit = GetUnitController().GetUnit(targetDestination);
        ContextMenuGUI contextMenu = GetPopupController().contextMenuGUI;
        if (targetUnit) contextMenu.Popup(contextMenu.transform.position, targetUnit);
    }

    private void OpenGroundInventory()
    {
        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        GetPopupController().containerGUI.Popup(targetDestination, GetItemController().GetInventory(targetDestination));
    }

    public override string GetInteractiveName()
    {
        return "World";
    }
}
