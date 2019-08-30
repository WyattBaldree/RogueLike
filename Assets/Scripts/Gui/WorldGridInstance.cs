using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class WorldGridInstance : MouseInteractive
{
    [SerializeField]
    SpriteRenderer mySpriteRenderer;

    [SerializeField]
    Sprite hoveredSprite;

    public override void CustomOnMouseEnter()
    {
        mySpriteRenderer.sprite = hoveredSprite;
    }

    public override void CustomOnMouseExit()
    {
        mySpriteRenderer.sprite = null;
    }

    public override void CustomOnRightMouseDown()
    {
        Vector2Int targetDestination = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        WorldObject targetObject = GetUnitController().GetUnit(targetDestination);
        if (!targetObject) targetObject = GetWallController().GetWall(targetDestination);
        if (!targetObject) targetObject = GetFloorController().GetFloor(targetDestination);

        GetPopupController().contextMenuGUI.Popup(targetDestination, targetObject);
    }
}
