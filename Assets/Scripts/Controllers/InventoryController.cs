using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    /// <summary>
    /// A collection of all of the inventoryGUIs. This is set in the inspector.
    /// </summary>
    public List<InventoryGUI> inventoryGUIs;

    /// <summary>
    /// Initialize all of the InventoryGUIs
    /// </summary>
    public void Initialize()
    {
        foreach(InventoryGUI gui in inventoryGUIs)
        {
            if(gui) gui.Initialize();
        }
    }

    /// <summary>
    /// Initialize the pickupDrop itemSprites
    /// </summary>
    public void InitializePickupDrops()
    {

        foreach (InventoryGUI gui in inventoryGUIs)
        {
            if (gui) gui.UpdateFrameContent();
        }
        
    }
}
