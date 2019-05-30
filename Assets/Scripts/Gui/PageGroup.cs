using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A group of gameobjects that can be collective set to active or deactive with the SetActive function.
/// </summary>
public class PageGroup : MonoBehaviour
{
    public bool active = true;
    public GameObject[] objects;

    public void SetActive(bool a)
    {
        active = a;
        foreach (GameObject o in objects)
        {
            o.SetActive(active);
        }
    }

    private void OnValidate()
    {
        SetActive(active);
    }
}


