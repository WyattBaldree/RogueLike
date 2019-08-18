using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class InventoryGUI : GUIComponent
{
    private List<PickupDrop> pickupDropList = new List<PickupDrop>();

    /// <summary>
    /// The inventory this is currently attached to.
    /// </summary>
    public Inventory targetInventory;

    public Vector2 separation = new Vector2(0.8f,0.8f);
    public float scale = 1f;
    public int numFrames = 40;

    /// <summary>
    /// The size of the visible rows.
    /// </summary>
    [NonSerialized]
    public Vector2 actualSize = new Vector2();

    /// <summary>
    /// The height of all rows including hidden ones. 
    /// </summary>
    public float totalHeight = 0;

    public int scrollAmount = 0;

    /// <summary>
    /// The source pickupDrop object.
    /// </summary>
    public PickupDrop pickupDropSource;

    //the total number of rows we have
    private int rows = 0;

    //how many rows are visble
    private int visibleRows = 0;

    //hold the number of columns we have
    private int columns = 0;

    /// <summary>
    /// When we are initialized, create each of the PickupDrop objects then initialize our frames and update the location of the pickupDrop objects.
    /// </summary>
    public void Initialize()
    {
        foreach (PickupDrop pd in pickupDropList)
        {
            if (pd)
            {
                DestroyImmediate(pd.gameObject);
                PickupDrop.pickupDropList.Remove(pd);
            }
        }

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
    }

    public void ConnectToInventory(Inventory inv)
    {
        targetInventory = inv;
        numFrames = inv.inventoryCapacity;
        SetScrollAmount(0);
        inv.myInventoryGUI = this;
        UpdateGUI();
    }

    /// <summary>
    /// Update the locations of the frames based on the offset and separations variables.
    /// </summary>
    void UpdateFrameLocations()
    {
        float currentWidth = 0;
        float currentHeight = 0;
        int pickupDropsOnLine = 0;

        // The following variable is used to determine the actual width of the inventory GUI within it's maxSize
        bool hasWrapped = false;

        bool hasPlacedOnRow = false;

        // The row we are currently on.
        int rowCount = 1;

        // used to determine how many rows are visible
        bool rowVisible = false;
        int visibleRowCount = 0;

        // reset our rows variable to 0 to be updated when a pickupDrop is placed on a row.
        rows = 0;

        for (int i = 0; i < pickupDropList.Count; i++)
        {
            // only update our rows variable when we actually place something on the row.
            rows = rowCount;

            if (!hasPlacedOnRow)
            {
                if (i == 0)
                {
                    currentHeight += scale;
                }
                else
                {
                    currentHeight += (scale + separation.y);
                }

                //Determine if the row we are currently on is visible or not.
                if (rows <= scrollAmount || currentHeight - (scrollAmount * (scale + separation.y)) > maxSize.y)
                {
                    rowVisible = false;
                }
                else
                {
                    rowVisible = true;
                    visibleRowCount++;
                }
            }
            
            //pickupDropList[i].transform.localPosition = new Vector2((i % columns) * (scale + separation.x),-scale - (float)Math.Floor(((float)i) / columns) * (scale + separation.y));
            pickupDropList[i].transform.position = transform.position + new Vector3(currentWidth, - currentHeight + (scrollAmount * (scale + separation.y)), 0);
            
            pickupDropsOnLine++;

            currentWidth += (scale + separation.x);

            hasPlacedOnRow = true;

            

            if (currentWidth - separation.x > maxSize.x)
            {
                //wrap and don't forget to wrap the one we just placed
                // if it was the only box on that line, don't wrap it.
                if(pickupDropsOnLine > 1)
                {
                    // if there was more than one box on this line, make sure to wrap the box we just placed
                    // by subtracting from the index, we replace the one we just placed.
                    i--;
                    if (!hasWrapped)
                    {
                        actualSize.x = currentWidth - (scale + (separation.x * 2));
                        columns = i + 1;
                    }
                }
                else
                {
                    if (!hasWrapped)
                    {
                        actualSize.x = currentWidth - (separation.x);
                        columns = i + 1;
                    }
                }
                hasWrapped = true;
                hasPlacedOnRow = false;
                rowCount++;
                currentWidth = 0;
                pickupDropsOnLine = 0;


            }

            // Set the pickup drops to hidden if they fall outside of the max size
            if (rowVisible)
            {
                pickupDropList[i].SetHidden(false);
            }
            else
            {
                pickupDropList[i].SetHidden(true);
            }
        }
        //if (stillVisible) visibleRowCount++;

        visibleRows = visibleRowCount;
        actualSize.y = (visibleRowCount * scale) + ((visibleRowCount - 1) * separation.y);
        totalHeight = currentHeight;
        Align();
    }

    public override void Align()
    {
        // Vertical Alignment
        float xAlign;
        if (horizontalAlignment == HorizontalAlignmentEnum.left)
        {
            xAlign = 0;
        }
        else if (horizontalAlignment == HorizontalAlignmentEnum.middle)
        {
            xAlign = (Mathf.Max(minSize.x, maxSize.x) / 2) - (actualSize.x / 2);
        }
        else
        {
            xAlign = Mathf.Max(minSize.x, maxSize.x) - (actualSize.x);
        }

        // Vertical Alignment
        float yAlign;
        if (verticalAlignment == VerticalAlignmentEnum.top)
        {
            yAlign = 0;
        }
        else if (verticalAlignment == VerticalAlignmentEnum.middle)
        {
            yAlign = (Mathf.Max(minSize.y, maxSize.y) / 2) - ((actualSize.y) / 2);
        }
        else
        {
            yAlign = Mathf.Max(minSize.y, maxSize.y) - (actualSize.y);
        }

        foreach (PickupDrop pd in pickupDropList)
        {
            pd.transform.position += new Vector3(xAlign, -yAlign, 0);
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

    public void SetScrollAmount(int amount)
    {
        scrollAmount = Mathf.Clamp(amount, 0, rows - visibleRows);
        UpdateFrameLocations();
    }

    public void ScrollUp()
    {
        SetScrollAmount(scrollAmount-1);
    }

    public void ScrollDown()
    {
        SetScrollAmount(scrollAmount + 1);
    }

    public int GetRows()
    {
        return rows;
    }

    public int GetColumns()
    {
        return columns;
    }

    public override Vector2 GetDimensions()
    {
        //float rows = GetRows();
        //return new Vector2( (columns * scale) + ((columns - 1) * separation.x), (rows * scale) + ((rows - 1) * separation.y));
        return actualSize;
    }

    public override void UpdateGUI()
    {
        Initialize();
        UpdateFrameContent();
        UpdateFrameLocations();
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
    }
}
