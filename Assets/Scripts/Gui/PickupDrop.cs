using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PickupDrop : MonoBehaviour
{
    //This class is used to make pickup and drop zones.
    //For example, the inventory is made from these and the equipment menu is made of these.
    //You can drag an item contained in one of these to another empty one to "drop the item there" 

    public static List<PickupDrop> pickupDropList = new List<PickupDrop>();
    public InventoryGUI myInventoryGUI;
    public SpriteRenderer itemSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;

    public int myIndex = 0;

    public bool mouseHovering = false;

    public void Initialize()
    {
        pickupDropList.Add(this);
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
        held = true;
        home = itemSpriteRenderer.gameObject.transform.position;
        itemSpriteRenderer.sortingOrder++;
    }

    /// <summary>
    /// Drop some amount of our item where we are currently hovering
    /// </summary>
    /// <param name="amount">how many to drop. -1 to drop all</param>
    private void DropItem(int amount = -1)
    {
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
                Item heldItem = inv.itemList[myIndex];

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

        itemSpriteRenderer.gameObject.transform.position = home;

        itemSpriteRenderer.sortingOrder--;

        DropItem();
    }
    
    /// <summary>
    /// Updates our sprite based on what item is currently in the item slot.
    /// </summary>
    public void UpdateSprite()
    {
        if (myInventoryGUI.targetInventory.GetItem(myIndex) == null)
        {
            GetSpriteRenderer().sprite = null;
            return;
        }

        Sprites sprite = myInventoryGUI.targetInventory.GetItem(myIndex).mySpriteController.GetMySpriteRenderers()[0];

        if (sprite)
        {
            GetSpriteRenderer().sprite = sprite.mySprite;
        }
    }

    private void OnMouseDown()
    {
        Pickup();
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void OnMouseEnter()
    {
        mouseHovering = true;

        //turn the whole background a solid color to highlight the one we are hovering over.
        backgroundSpriteRenderer.sortingOrder += 2;
    }

    private void OnMouseExit()
    {
        mouseHovering = false;

        //undo the highlight
        backgroundSpriteRenderer.sortingOrder -= 2;
    }

    private void LateUpdate()
    {
        //if we are held, let our item sprite renderere follow the movement of the mouse.
        if (held)
        {
            itemSpriteRenderer.gameObject.transform.position = GetMousePosition();
        }
    }

    /// <summary>
    /// Returns the mosue position on the screen.
    /// </summary>
    /// <returns></returns>
    Vector3 GetMousePosition()
    {
        Camera cam = Camera.main;

        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        return cam.ScreenToWorldPoint(new Vector3(mousePos.x - 25f, mousePos.y - 25f, cam.nearClipPlane));
    }

    SpriteRenderer mySpriteRenderer;

    /// <summary>
    /// Returns the item sprite renderer.
    /// </summary>
    /// <returns></returns>
    SpriteRenderer GetSpriteRenderer()
    {
        if(mySpriteRenderer == null)
        {
            mySpriteRenderer = itemSpriteRenderer.GetComponent<SpriteRenderer>();
        }
        return mySpriteRenderer;
    }

    /// <summary>
    /// Set the item sprite
    /// </summary>
    /// <param name="s"></param>
    public void SetSprite(Sprite s)
    {
        GetSpriteRenderer().sprite = s;
    }
}
