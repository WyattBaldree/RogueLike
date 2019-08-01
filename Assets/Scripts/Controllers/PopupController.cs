using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public Vector2 popupBounds = new Vector2(10, 10);

    private List<GUIPopup> popupStack = new List<GUIPopup>();

    public ContainerGUI containerGUI;

    public void PopupOpened(GUIPopup popup)
    {
        popupStack.Add(popup);
    }

    public void PopupClosed(GUIPopup popup)
    {
        popupStack.Remove(popup);
    }

    void OnDrawGizmosSelected()
    {
        // Draws a box around our text box.
        Vector2 dim = popupBounds;
        float boxWidth = dim.x;
        float boxHeight = dim.y;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(boxWidth, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, -boxHeight, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(boxWidth, 0, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
    }
}
