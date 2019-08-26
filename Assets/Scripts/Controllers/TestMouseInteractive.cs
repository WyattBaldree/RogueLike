using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseInteractive : MouseInteractive
{
    public override void CustomOnMouseEnter()
    {
        Debug.Log("Mouse Entered " + name);
    }

    public override void CustomOnMouseExit()
    {
        Debug.Log("Mouse Exited " + name);
    }

    public override void CustomOnLeftMouseDown()
    {
        Debug.Log("Mouse Left Down " + name);
    }

    public override void CustomOnLeftMouseUp()
    {
        Debug.Log("Mouse Left Up " + name);
    }

    public override void CustomOnRightMouseDown()
    {
        Debug.Log("Mouse Right Down " + name);
    }

    public override void CustomOnRightMouseUp()
    {
        Debug.Log("Mouse Right Down " + name);
    }
}
