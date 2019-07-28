using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TestGUI : GUIComponent
{
    public Window myWindow;
    public Entry titleEntry;
    public InventoryGUI inventorySlots;
    public GUIButton scrollUpButton;
    public GUIButton scrollDownButton;

    public float bezelTop;
    public float bezelBottom;
    public float bezelSides;

    public Vector2 windowSize = new Vector2();

    private void UpdateDimensions()
    {

    }

    public override Vector2 GetDimensions()
    {
        Assert.IsNotNull(myWindow);
        return myWindow.GetDimensions();
    }

    public override void Align()
    {
        // position all of the inner objects
        myWindow.transform.position = transform.position;
        myWindow.SetDimension(windowSize);

        titleEntry.transform.position = transform.position + new Vector3(bezelSides, -bezelTop, 0);
        titleEntry.maxSize.x = windowSize.x - (2 * bezelSides);
        titleEntry.horizontalAlignment = HorizontalAlignmentEnum.middle;
        titleEntry.verticalAlignment = VerticalAlignmentEnum.top;

        inventorySlots.transform.position = transform.position + new Vector3(bezelSides, -bezelTop - titleEntry.GetDimensions().y, 0);
        inventorySlots.maxSize = new Vector2(windowSize.x - (2 * bezelSides) - scrollUpButton.GetDimensions().x, windowSize.y - bezelTop - bezelBottom - titleEntry.GetDimensions().y);
        inventorySlots.horizontalAlignment = HorizontalAlignmentEnum.middle;
        inventorySlots.verticalAlignment = VerticalAlignmentEnum.middle;

        scrollUpButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x, 0, 0);
        scrollDownButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x, - inventorySlots.GetDimensions().y + scrollDownButton.GetDimensions().y, 0);

        myWindow.Align();
        titleEntry.EntryInitialize("hello");
        inventorySlots.Initialize();
        scrollUpButton.Align();
        scrollDownButton.Align();
    }

    public override void UpdateGUI()
    {
        Align();
    }
}
