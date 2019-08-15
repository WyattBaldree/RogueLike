using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor2 : MonoBehaviour
{
    [SerializeField]
    private bool canSupportWall;
    /// <summary>
    /// Can a wall be built on top of this floor?
    /// </summary>
    public bool CanSupportWall
    {
        get => canSupportWall;
    }
}
