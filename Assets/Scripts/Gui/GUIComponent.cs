using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class GUIComponent : MonoBehaviour
{
    public enum HorizontalAlignmentEnum { left, middle, right };
    public enum VerticalAlignmentEnum { bottom, middle, top };

    [Header("GUI Component")]
    public HorizontalAlignmentEnum horizontalAlignment = HorizontalAlignmentEnum.left;
    public VerticalAlignmentEnum verticalAlignment = VerticalAlignmentEnum.top;

    public Vector2 maxSize = new Vector2();
    public Vector2 minSize = new Vector2();

    public abstract void Align();

    public abstract Vector2 GetDimensions();

    void OnDrawGizmosSelected()
    {
        // Draws a box around our text box.
        Vector2 dim = GetDimensions();
        float boxWidth = dim.x;
        float boxHeight = dim.y;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(boxWidth, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, -boxHeight, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(boxWidth, 0, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(boxWidth / 2, 0, 0), transform.position + new Vector3(boxWidth / 2, -boxHeight, 0));

        dim = minSize;
        boxWidth = dim.x;
        boxHeight = dim.y;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(boxWidth, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, -boxHeight, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(boxWidth, 0, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));

        dim = maxSize;
        boxWidth = dim.x;
        boxHeight = dim.y;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(boxWidth, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(0, -boxHeight, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
        Gizmos.DrawLine(transform.position + new Vector3(boxWidth, 0, 0), transform.position + new Vector3(boxWidth, -boxHeight, 0));
    }

    public abstract void UpdateGUI();
}