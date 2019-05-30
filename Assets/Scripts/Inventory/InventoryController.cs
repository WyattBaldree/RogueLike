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
    /// An enum where each entry is associated with an inventoryGUI in inventoryGUIs. GetInventoryGUI returns the associated InventoryGUI.
    /// </summary>
    public enum inventoryEnum {none ,main, single, ground, weapon, helmet, chest, greaves, boots, gloves, container};

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
    /// Get the InventoryGUI associated with the supplied inventoryEnum.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public InventoryGUI getInventoryGUI(inventoryEnum i)
    {
        return inventoryGUIs[(int)i];
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
