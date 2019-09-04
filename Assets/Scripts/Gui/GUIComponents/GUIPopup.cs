using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class GUIPopup : GUIComponent
{
    //The GUI popup abstract class should do everything the GUIComponent abstract class does except it will also have the ability to "pop up" and "hide"
    public PopupController popupControllerInstance;

    [System.NonSerialized]
    public bool opened = false;

    public virtual void Popup(Vector2 targetPosition, bool constrainToWorldSpace = true)
    {
        UpdateGUI();
        gameObject.SetActive(true);
        opened = true;
        if (constrainToWorldSpace)
        {
            PositionWithinPopupBounds(targetPosition, popupControllerInstance.popupBoundsWorld);
        }
        else
        {
            PositionWithinPopupBounds(targetPosition, popupControllerInstance.popupBoundsScreen);
        }
        
        popupControllerInstance.PopupOpened(this);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        opened = false;
        popupControllerInstance.PopupClosed(this);
    }

    private bool PositionWithinPopupBounds(Vector2 targetPosition, Vector2 popupBounds)
    {
        // The purpose of this function is to position
        Vector2 size = GetDimensions();

        //If there is a target position and the size of our popup is smaller than the size of the popup area
        if (targetPosition != null && size.x <= popupBounds.x && size.y <= popupBounds.y)
        {
            // position the new popup but make sure it is within the correct bounds. Create a PopupController Class that has the bounds built in and has a stack of popups.
            float maxHorizontalPosition = popupControllerInstance.transform.position.x + popupBounds.x - size.x;
            float minVerticalPosition = popupControllerInstance.transform.position.y - popupBounds.y + size.y;

            transform.position = new Vector3(Mathf.Clamp(targetPosition.x, popupControllerInstance.transform.position.x, maxHorizontalPosition), 
                                             Mathf.Clamp(targetPosition.y, minVerticalPosition, popupControllerInstance.transform.position.y), 
                                             transform.position.z);
            return true;
        }
        else
        {
            // else we cannot place the poup properly. Just place it in the center of the popup area.
            transform.position = new Vector3(popupControllerInstance.transform.position.x + (popupBounds.x/2) - (size.x/2),
                                             popupControllerInstance.transform.position.y - (popupBounds.y/2) + (size.y/2), 
                                             transform.position.z);
            return false;
        }
    }
}
