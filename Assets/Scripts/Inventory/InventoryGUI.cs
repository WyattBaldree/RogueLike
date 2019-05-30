using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    private List<PickupDrop> pickupDropList;

    /// <summary>
    /// The inventory this is currently attached to.
    /// </summary>
    public Inventory targetInventory;

    public Vector2 offset = new Vector2(0, 0);
    public Vector2 separation = new Vector2(0.8f,0.8f);
    public float scale = 1f;
    public int columns = 5;
    public int numFrames = 40;

    /// <summary>
    /// The source pickupDrop object.
    /// </summary>
    public PickupDrop pickupDropSource;
    
    /// <summary>
    /// When we are initialized, create each of the PickupDrop objects then initialize our frames and update the location of the pickupDrop objects.
    /// </summary>
    public void Initialize()
    {
        pickupDropList = new List<PickupDrop>();

        for (int i = 0; i < numFrames; i++)
        {
            PickupDrop pd = Instantiate<PickupDrop>(pickupDropSource,transform);
            pd.Initialize();
            pd.myIndex = i;
            pd.transform.localScale = new Vector3(scale, scale, 1);
            pd.myInventoryGUI = this;
            pickupDropList.Add(pd);
        }
        UpdateFrameLocations();
        UpdateFrameContent();
        Debug.Log(name + " initialized");
    }

    /// <summary>
    /// Update the locations of the frames based on the offset and separations variables.
    /// </summary>
    void UpdateFrameLocations()
    {
        for (int i = 0; i < pickupDropList.Count; i++)
        {
            pickupDropList[i].transform.localPosition = new Vector2((i % columns) * separation.x + offset.x, offset.y - (float)Math.Floor(((float)i) / columns) * separation.y);
        }
    }

    /// <summary>
    /// Update the sprites of the pickupDrop objects based on the object in the inventory.
    /// </summary>
    public void UpdateFrameContent(int index = -1)
    {
        if(targetInventory)
        {
            if(index < 0)
            {
                for (int i = 0; i < pickupDropList.Count; i++)
                {
                    pickupDropList[i].UpdateSprite();
                }
            }
            else
            {
                pickupDropList[index].UpdateSprite();
            }
        }
    }
}
