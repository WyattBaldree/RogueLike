using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ContainerGUI : GUIPopup
{
    public Window window;
    public Entry titleEntry;
    public InventoryGUI inventorySlots;
    public GUIButton scrollUpButton;
    public GUIButton scrollDownButton;

    public float bezelTop;
    public float bezelBottom;
    public float bezelSides;

    public Vector2 inventorySize;

    public float scrollButtonSeparation = .5f;
    public float titleSeparation = .5f;

    public override Vector2 GetDimensions()
    {
        Assert.IsNotNull(window);
        return window.GetDimensions();
    }

    public override void Align()
    {
        // base the GUI off of the child inventory GUI so update it first.
        inventorySlots.maxSize = inventorySize;
        inventorySlots.horizontalAlignment = HorizontalAlignmentEnum.left;
        inventorySlots.verticalAlignment = VerticalAlignmentEnum.top;
        inventorySlots.UpdateGUI();

        titleEntry.maxSize.x = inventorySlots.actualSize.x + scrollButtonSeparation + scrollUpButton.GetDimensions().x;
        titleEntry.transform.position = transform.position + new Vector3(bezelSides, -bezelTop, 0);
        titleEntry.horizontalAlignment = HorizontalAlignmentEnum.middle;
        titleEntry.verticalAlignment = VerticalAlignmentEnum.top;
        titleEntry.UpdateGUI();

        inventorySlots.transform.position = titleEntry.transform.position + new Vector3(0, -titleEntry.GetDimensions().y - titleSeparation, 0);

        scrollUpButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x + scrollButtonSeparation, 0, 0);
        scrollUpButton.UpdateGUI();

        scrollDownButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x + scrollButtonSeparation, -inventorySlots.GetDimensions().y + scrollDownButton.GetDimensions().y, 0);
        scrollDownButton.UpdateGUI();

        window.transform.position = transform.position;
        window.maxSize = new Vector2(inventorySlots.GetDimensions().x + scrollButtonSeparation + scrollUpButton.GetDimensions().x + (bezelSides * 2), titleEntry.GetDimensions().y + inventorySlots.GetDimensions().y + bezelTop + bezelBottom + titleSeparation);
        window.UpdateGUI();
    }

    public override void UpdateGUI()
    {
        Align();
        scrollUpButton.pressEvent.RemoveAllListeners();
        scrollUpButton.pressEvent.AddListener(inventorySlots.ScrollUp);
        scrollDownButton.pressEvent.RemoveAllListeners();
        scrollDownButton.pressEvent.AddListener(inventorySlots.ScrollDown);
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }

    public void Popup(Vector2 targetPosition, Inventory targetInventory)
    {
        titleEntry.text = targetInventory.inventoryName;
        inventorySlots.ConnectToInventory(targetInventory);
        targetInventory.myInventoryGUI = inventorySlots;
        Popup(targetPosition);
    }
}