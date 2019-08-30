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
    public GUIButton xButton;

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

        titleEntry.maxSize.x = inventorySlots.actualSize.x + scrollButtonSeparation + scrollUpButton.GetDimensions().x - xButton.GetDimensions().x;
        titleEntry.transform.position = transform.position + new Vector3(bezelSides, -bezelTop, -2);
        titleEntry.horizontalAlignment = HorizontalAlignmentEnum.middle;
        titleEntry.verticalAlignment = VerticalAlignmentEnum.top;
        titleEntry.UpdateGUI();

        xButton.transform.position = titleEntry.transform.position + new Vector3(titleEntry.GetDimensions().x, 0, -2);
        xButton.UpdateGUI();

        float topBarHeight = Mathf.Max(titleEntry.GetDimensions().y, xButton.GetDimensions().y);

        inventorySlots.transform.position = titleEntry.transform.position + new Vector3(0, -topBarHeight - titleSeparation, -2);

        if(inventorySlots.GetRows() > inventorySlots.GetVisibleRows())
        {
            scrollUpButton.gameObject.SetActive(true);
            scrollUpButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x + scrollButtonSeparation, 0, -2);
            scrollUpButton.UpdateGUI();

            scrollDownButton.gameObject.SetActive(true);
            scrollDownButton.transform.position = inventorySlots.transform.position + new Vector3(inventorySlots.GetDimensions().x + scrollButtonSeparation, -inventorySlots.GetDimensions().y + scrollDownButton.GetDimensions().y, -2);
            scrollDownButton.UpdateGUI();
        }
        else
        {
            scrollUpButton.gameObject.SetActive(false);
            scrollDownButton.gameObject.SetActive(false);
        }

        window.transform.position = transform.position + new Vector3(0,0,-1);
        window.maxSize = new Vector2(inventorySlots.GetDimensions().x + scrollButtonSeparation + scrollUpButton.GetDimensions().x + (bezelSides * 2), topBarHeight + inventorySlots.GetDimensions().y + bezelTop + bezelBottom + titleSeparation);
        window.UpdateGUI();

        myCollider.size = GetDimensions();
        myCollider.offset = new Vector2((myCollider.size.x / 2), -(myCollider.size.y / 2));
    }

    public override void UpdateGUI()
    {
        Align();
        scrollUpButton.pressEvent.RemoveAllListeners();
        scrollUpButton.pressEvent.AddListener(inventorySlots.ScrollUp);
        scrollDownButton.pressEvent.RemoveAllListeners();
        scrollDownButton.pressEvent.AddListener(inventorySlots.ScrollDown);
        xButton.pressEvent.RemoveAllListeners();
        xButton.pressEvent.AddListener(Hide);

        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }

    public void Popup(Vector2 targetPosition, Inventory targetInventory)
    {
        titleEntry.text = "<d>"+targetInventory.inventoryName;
        inventorySlots.ConnectToInventory(targetInventory);
        Popup(targetPosition);
    }
}