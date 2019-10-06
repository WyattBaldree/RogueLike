using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    /// <summary>
    /// all this class does is hold a reference to all GUIs for other objects to access.
    /// </summary>

    public InventoryGUI mainInventoryGUI;
    public InventoryGUI equipmentGUIHead;
    public InventoryGUI equipmentGUITorso;
    public InventoryGUI equipmentGUILeftHand;
    public InventoryGUI equipmentGUILeftArm;
    public InventoryGUI equipmentGUILeftLeg;
    public InventoryGUI equipmentGUIRightHand;
    public InventoryGUI equipmentGUIRightArm;
    public InventoryGUI equipmentGUIRightLeg;
}
