using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class MouseInteractive : MonoBehaviour
{
    public virtual void CustomOnMouseEnter() { }
    public virtual void CustomOnMouseExit() { }

    public virtual void CustomOnLeftMouseDown() { }
    public virtual void CustomOnLeftMouseUp() { }

    public virtual void CustomOnRightMouseDown() { }
    public virtual void CustomOnRightMouseUp() { }
}
