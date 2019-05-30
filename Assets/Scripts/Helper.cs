using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static int GetX(GameObject o)
    {
        return (int)o.transform.position.x;
    }

    public static int GetY(GameObject o)
    {
        return (int)o.transform.position.y;
    }
}
