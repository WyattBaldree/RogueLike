using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class WorldGridInstance : MouseInteractive
{
    [SerializeField]
    SpriteRenderer mySpriteRenderer;

    [SerializeField]
    Interactive worldGridInteractive;

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
        GetPopupController().contextMenuGUI.Popup(MouseInputController.GetMousePosition(), worldGridInteractive);
    }
}
