using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class MouseInputController : MonoBehaviour
{
    public static List<MouseInteractive> mouseInteractives = new List<MouseInteractive>();
    List<MouseInteractive> overList = new List<MouseInteractive>();

    private bool leftMouseDown;
    private bool rightMouseDown;

    private void Awake()
    {
        mouseInteractives.Clear();
    }

    /// <summary>
    /// Returns the mosue position on the screen.
    /// </summary>
    /// <returns></returns>
    static public Vector3 GetMousePosition()
    {
        Camera cam = Camera.main;
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        return cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOverList();

        //Left Click
        bool leftClick = Input.GetMouseButton(0);

        if (leftClick)
        {
            if (!leftMouseDown)
            {
                OnLeftClick();
            }
        }
        else
        {
            if (leftMouseDown)
            {
                if (overList.Count > 0) overList[0].CustomOnLeftMouseUp();
                leftMouseDown = false;
            }
        }

        //Right Click
        bool rightClick = Input.GetMouseButton(1);

        if (rightClick)
        {
            if (!rightMouseDown)
            {
                if (overList.Count > 0) overList[0].CustomOnRightMouseDown();
                rightMouseDown = true;
            }
        }
        else
        {
            if (rightMouseDown)
            {
                if (overList.Count > 0) overList[0].CustomOnRightMouseUp();
                rightMouseDown = false;
            }
        }

    }

    void UpdateOverList()
    {
        List<MouseInteractive> newOverList = new List<MouseInteractive>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, 5f);

        foreach(RaycastHit2D hit in hits)
        {
            MouseInteractive mouseInteractive = hit.collider.gameObject.GetComponent<MouseInteractive>();
            if (mouseInteractive)
            {
                newOverList.Add(mouseInteractive);
            }
        }


        foreach (MouseInteractive existingMI in overList)
        {
            if (!newOverList.Contains(existingMI))
            {
                //The new list no longer has this entry
                //This interactive ahs been exited
                if (existingMI) existingMI.CustomOnMouseExit();
            }
        }

        foreach (MouseInteractive newMI in newOverList)
        {
            if (!overList.Contains(newMI))
            {
                //The existing list does not contain this new interactive
                //this interactive has been entered
                if (newMI) newMI.CustomOnMouseEnter();
            }
        }

        overList = newOverList;
    }

    private void OnLeftClick()
    {
        if (overList.Count > 0) overList[0].CustomOnLeftMouseDown();

        foreach(MouseInteractive mi in mouseInteractives)
        {
            if (overList.Contains(mi) == false) mi.CustomOnMouseClickedOutside();
        }

        leftMouseDown = true;
    }
}
