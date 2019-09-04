using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Assertions;
using static GameController;

public class PickupDrop : MouseInteractive
{
    //This class is used to make pickup and drop zones.
    //For example, the inventory is made from these and the equipment menu is made of these.
    //You can drag an item contained in one of these to another empty one to "drop the item there" 

    public static List<PickupDrop> pickupDropList = new List<PickupDrop>();
    public InventoryGUI myInventoryGUI;
    public SpriteRenderer backgroundSpriteRenderer;
    public RogueSpriteRenderer itemSpriteRenderer;
    public SpriteRenderer disabledSpriteRenderer;
    
    public Sprite spriteButton;
    public Sprite spriteButtonHovered;

    [System.NonSerialized]
    public int myIndex = 0;

    public bool disabled = false;

    public bool hidden = false;

    public bool mouseHovering = false;

    private int defaultItemSortingOrder;

    public void Initialize()
    {
        pickupDropList.Add(this);
        home = itemSpriteRenderer.transform.position;
        defaultItemSortingOrder = itemSpriteRenderer.SortingOrder;
    }

    private void Update()
    {
        //handle the dropping controls
        if (held)
        {
            //Right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                    {
                        // shift and alt
                        // drop 100
                        DropItem(100);
                    }
                    else
                    {
                        // shift but no alt
                        // drop 10
                        DropItem(10);
                    }
                }
                else
                {
                    // no shift no alt
                    // drop 1
                    DropItem(1);
                }
            }
        }
    }

    private void OnDestroy()
    {
        pickupDropList.Remove(this);
    }

    bool held = false;

    Vector3 home = new Vector3();

    /// Called when the user picks us up
    void Pickup()
    {
        if (disabled || hidden) return;
        held = true;
        itemSpriteRenderer.transform.parent = null;
        itemSpriteRenderer.transform.position = new Vector3(itemSpriteRenderer.transform.position.x, itemSpriteRenderer.transform.position.y, 0);
    }

    /// <summary>
    /// Drop some amount of our item where we are currently hovering
    /// </summary>
    /// <param name="amount">how many to drop. -1 to drop all</param>
    private void DropItem(int amount = -1)
    {
        if (myInventoryGUI.targetInventory.GetItem(myIndex) == null)
        {
            return;
        }

        foreach (PickupDrop pd in pickupDropList)
        {
            if (pd == this) continue;
            if (!pd.isActiveAndEnabled) continue;

            bool dropSpotHere = pd.mouseHovering;

            if (dropSpotHere)
            {
                Inventory inv = myInventoryGUI.targetInventory;
                if (inv == null)
                {
                    return;
                }
                RoguelikeObject heldItem = inv.itemList[myIndex];

                inv.MoveItem(myIndex, pd.myInventoryGUI.targetInventory, pd.myIndex, amount);

                break;
            }
        }
    }


    /// <summary>
    /// Called when the user lets go of us
    /// </summary>
    void Drop()
    {
        held = false;
        itemSpriteRenderer.gameObject.transform.position = backgroundSpriteRenderer.transform.position;
        itemSpriteRenderer.transform.parent = this.transform;

        if (!disabled && !hidden) DropItem();
    }
    
    /// <summary>
    /// Updates our sprite based on what item is currently in the item slot.
    /// </summary>
    public void UpdateSprite()
    {
        Assert.IsNotNull(itemSpriteRenderer);
        Assert.IsNotNull(backgroundSpriteRenderer);
        Assert.IsNotNull(disabledSpriteRenderer);
        Assert.IsNotNull(spriteButton);
        Assert.IsNotNull(spriteButtonHovered);

        if (hidden)
        {
            itemSpriteRenderer.gameObject.SetActive(false);
            backgroundSpriteRenderer.gameObject.SetActive(false);
            disabledSpriteRenderer.gameObject.SetActive(false);
        }
        else
        {
            itemSpriteRenderer.gameObject.SetActive(true);
            backgroundSpriteRenderer.gameObject.SetActive(true);

            if (disabled)
            {
                disabledSpriteRenderer.gameObject.SetActive(true);
            }
            else
            {
                disabledSpriteRenderer.gameObject.SetActive(false);
            }

            if (mouseHovering)
            {
                backgroundSpriteRenderer.sprite = spriteButtonHovered;
            }
            else
            {
                backgroundSpriteRenderer.sprite = spriteButton;
            }

            if (myInventoryGUI && myInventoryGUI.targetInventory)
            {
                if (myIndex >= myInventoryGUI.targetInventory.inventoryCapacity || myInventoryGUI.targetInventory.GetItem(myIndex) == null)
                {
                    itemSpriteRenderer.gameObject.SetActive(false);
                    itemSpriteRenderer.ClearValues();
                    return;
                }

                RoguelikeObject item = myInventoryGUI.targetInventory.GetItem(myIndex);

                if (item)
                {
                    itemSpriteRenderer.CopyValues(item.myRogueSpriteRenderer);
                }
            }
            else
            {
                itemSpriteRenderer.gameObject.SetActive(false);
                itemSpriteRenderer.ClearValues();
            }

        }
    }

    public void SetDisabled(bool doDisable)
    {
        disabled = doDisable;
        if (disabled)
        {
            mouseHovering = false;
            Drop();
        }
        UpdateSprite();
    }

    public void SetHidden(bool doHide)
    {
        hidden = doHide;
        if (hidden)
        {
            mouseHovering = false;
            Drop();
        }
        UpdateSprite();
    }

    public override void CustomOnLeftMouseDown()
    {
        Pickup();
    }

    public override void CustomOnRightMouseDown()
    {
        RoguelikeObject item = myInventoryGUI.targetInventory.GetItem(myIndex);
        if (item)
        {
            GetPopupController().contextMenuGUI.Popup(MouseInputController.GetMousePosition(), item);
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    public override void CustomOnMouseEnter()
    {
        if (disabled || hidden) return;
        mouseHovering = true;

        UpdateSprite();
    }

    public override void CustomOnMouseExit()
    {
        if (disabled || hidden) return;
        mouseHovering = false;

        UpdateSprite();
    }

    private void LateUpdate()
    {
        //if we are held, let our item sprite renderere follow the movement of the mouse.
        if (held)
        {
            Vector3 mousePos = MouseInputController.GetMousePosition();
            Vector2 spriteRendererSize = itemSpriteRenderer.GetDimensions();
            itemSpriteRenderer.gameObject.transform.position = new Vector3(mousePos.x - (spriteRendererSize.x/2), mousePos.y - (spriteRendererSize.y / 2), 0);
        }
    }

    private void OnValidate()
    {
        SetDisabled(disabled);
        SetHidden(hidden);
    }
}
