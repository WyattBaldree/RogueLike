using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Unit : Object
{
    public SpriteRenderer mySpriteRenderer;

    public Inventory inventorySource;
    public Inventory inventory;

    private int weaponAttackBonus = 0;
    private int helmetArmorBonus = 0;
    private int chestArmorBonus = 0;
    private int glovesArmorBonus = 0;
    private int bootsArmorBonus = 0;

    public Inventory equipmentSlotSource;

    [System.NonSerialized]
    public Inventory slotWeapon;
    [System.NonSerialized]
    public Inventory slotHelmet;
    [System.NonSerialized]
    public Inventory slotChest;
    [System.NonSerialized]
    public Inventory slotGloves;
    [System.NonSerialized]
    public Inventory slotBoots;
    [System.NonSerialized]
    public Inventory slotRing1;
    [System.NonSerialized]
    public Inventory slotRing2;
    [System.NonSerialized]
    public Inventory slotAmulet;

    public float greed = 0.5f;
    public float fear = 0.1f;
    public float aggression = 0.8f;

    //Enumerator for directions up, down, left, right
    public enum dir { none, up, right, down, left, ur, ul, dr, dl };

    public virtual void Initialize()
    {
        SetHealth(GetMaxHealth());

        
        /*[Remove?]
        slotWeapon = newInventory(equipmentSlotSource, "Weapon", new List<string> { "weapon" }, UpdateWeapon);

        slotHelmet = newInventory(equipmentSlotSource, "Head", new List<string> { "helmet" }, UpdateHelmet);

        slotChest = newInventory(equipmentSlotSource, "Chest", new List<string> { "chest" }, UpdateChest);

        slotGloves = newInventory(equipmentSlotSource, "Hands", new List<string> { "gloves" }, UpdateGloves);
        
        slotBoots = newInventory(equipmentSlotSource, "Feet", new List<string> { "boots" }, UpdateBoots);

        slotRing1 = newInventory(equipmentSlotSource, "Ring Finger", new List<string> { "ring" }, null);

        slotRing2 = newInventory(equipmentSlotSource, "Index Finger", new List<string> { "ring" }, null);

        slotAmulet = newInventory(equipmentSlotSource, "Neck", new List<string> { "amulet" }, null);
        */
    }

    /// <summary>
    /// This function creates a new Inventory for the Unit
    /// </summary>
    /// <param name="source">The source inventory from which we are instantiating</param>
    /// <param name="name">The name of the new inventory</param>
    /// <param name="invEnum">the InventoryController.inventoryEnum that will decide which inventory space we are connected to</param>
    /// <param name="_acceptableTypes">A list of strings which define what items can be placed into the inventory</param>
    /// <param name="updateListenerFunction">The function that is called when the inventory contents change</param>
    /// <returns></returns>
    Inventory newInventory(Inventory source, string name, List<RoguelikeObject.TagEnum> _acceptableTypes, UnityAction updateListenerFunction = null)
    {
        Inventory inv = Instantiate<Inventory>(source, transform);
        inv.inventoryName = name;
        inv.acceptableTypes = _acceptableTypes;
        return inv;
    }

    /*[remove?]
    ///This function is called when the unit equips a weapon
    private void UpdateWeapon()
    {
        Weapon newWeapon = (Weapon)slotWeapon.itemList[0];

        if (newWeapon)
        {
            weaponAttackBonus = newWeapon.damage;
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> wields the " + newWeapon.instanceName + "<d>.");
        }
        else
        {
            weaponAttackBonus = 0;
            GameController.logC.NewEntry(instanceName + "<d> is disarmed.");
        }
        Debug.Log("Weapon Attack Bonus: " + weaponAttackBonus);
    }

    ///This function is called when the unit equips a helmet
    private void UpdateHelmet()
    {
        Armor newHelmet = (Armor)slotHelmet.itemList[0];

        if (newHelmet)
        {
            helmetArmorBonus = newHelmet.armor;
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> dons the " + newHelmet.instanceName + "<d>.");
        }
        else
        {
            helmetArmorBonus = 0;
            GameController.logC.NewEntry(instanceName + "'s<d> helmet is removed.");
        }
        Debug.Log("helmet armor Bonus: " + helmetArmorBonus);
    }

    ///This function is called when the unit equips chest armor
    private void UpdateChest()
    {
        Armor newChest = (Armor)slotChest.itemList[0];

        if (newChest)
        {
            chestArmorBonus = newChest.armor;
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> dons the " + newChest.instanceName + "<d>.");
        }
        else
        {
            chestArmorBonus = 0;
            GameController.logC.NewEntry(instanceName + "'s<d> chestpiece is removed.");
        }
        Debug.Log("chest armor Bonus: " + chestArmorBonus);
    }

    ///This function is called when the unit equips gloves
    private void UpdateGloves()
    {
        Armor newGloves = (Armor)slotGloves.itemList[0];

        if (newGloves)
        {
            glovesArmorBonus = newGloves.armor;
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> dons the " + newGloves.instanceName + "<d>.");
        }
        else
        {
            glovesArmorBonus = 0;
            GameController.logC.NewEntry(instanceName + "'s<d> gloves are removed.");
        }
        Debug.Log("gloves armor Bonus: " + glovesArmorBonus);
    }

    ///This function is called when the unit equips boots
    private void UpdateBoots()
    {
        Armor newBoots = (Armor)slotBoots.itemList[0];

        if (newBoots)
        {
            bootsArmorBonus = newBoots.armor;
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> dons the " + newBoots.instanceName + "<d>.");
        }
        else
        {
            bootsArmorBonus = 0;
            GameController.logC.NewEntry(instanceName + "'s<d> boots are removed.");
        }
        Debug.Log("boots armor Bonus: " + bootsArmorBonus);
    }

    ///This function is called when the unit equips an amulet
    private void UpdateAmulet()
    {
        Armor newAmulet = (Armor)slotAmulet.itemList[0];

        if (newAmulet)
        {
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> dons the " + newAmulet.instanceName + "<d>.");
        }
        else
        {
            GameController.logC.NewEntry(instanceName + "'s<d> amulet is removed.");
        }
    }
    
    ///This function is called when the unit equips ring1
    private void UpdateRing1()
    {
        Armor newRing1 = (Armor)slotRing1.itemList[0];

        if (newRing1)
        {
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> puts on the " + newRing1.instanceName + "<d>.");
        }
        else
        {
            GameController.logC.NewEntry(instanceName + "'s<d> Ring is removed.");
        }
    }

    ///This function is called when the unit equips ring2
    private void UpdateRing2()
    {
        Armor newRing2 = (Armor)slotRing2.itemList[0];

        if (newRing2)
        {
            GameController.logC.NewEntry("<d>The " + instanceName + "<d> puts on the " + newRing2.instanceName + "<d>.");
        }
        else
        {
            GameController.logC.NewEntry(instanceName + "'s<d> Ring is removed.");
        }
    }*/

    /// <summary>
    /// Remove the Unit from the game.
    /// </summary>
    private void Remove()
    {
        GameController.unitC.unitList.Remove(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Kill the unit
    /// </summary>
    public void Die()
    {
        //Drop all items, drop a corpse, experience

        /*[remove?]
        if (slotHelmet.itemList[0]) Drop(slotHelmet, slotHelmet.itemList[0], 0, 0);
        if (slotGloves.itemList[0]) Drop(slotGloves, slotGloves.itemList[0], 0, 0);
        if (slotChest.itemList[0]) Drop(slotChest, slotChest.itemList[0], 0, 0);
        if (slotBoots.itemList[0]) Drop(slotBoots, slotBoots.itemList[0], 0, 0);
        if (slotWeapon.itemList[0]) Drop(slotWeapon, slotWeapon.itemList[0], 0, 0);*/

        foreach (RoguelikeObject i in inventory.itemList)
        {
            if (i) Drop(inventory, i, 0, 0);
        }

        Remove();
    }

    //our current speed counter. When this reaches 0, take an action.
    [System.NonSerialized]
    public int speedCounter = 0;

    //Subtract from our speed counter
    //
    public virtual void Step()
    {
        speedCounter--;
        if(speedCounter < 0)
        {
            //Readjust our speed after we take our action. !!!!placeholder. this should be dependant on the action taken.!!!!!
            speedCounter += 20-speed;

            //figure out which direction we want to move
            dir moveDirection = GetMoveDirection();

            MoveDirection(moveDirection);
        }
    }

    ///Attempt to move in the specified direction using attack move
    void MoveDirection(dir moveDirection)
    {
        if (moveDirection == dir.none)
        {
            AttackMoveLocal(0, 0);
        }
        else if (moveDirection == dir.up)
        {
            AttackMoveLocal(0, 1);
        }
        else if (moveDirection == dir.right)
        {
            AttackMoveLocal(1, 0);
        }
        else if (moveDirection == dir.down)
        {
            AttackMoveLocal(0, -1);
        }
        else if (moveDirection == dir.left)
        {
            AttackMoveLocal(-1, 0);
        }
        else if (moveDirection == dir.ur)
        {
            AttackMoveLocal(1, 1);
        }
        else if (moveDirection == dir.ul)
        {
            AttackMoveLocal(-1, 1);
        }
        else if (moveDirection == dir.dr)
        {
            AttackMoveLocal(1, -1);
        }
        else if (moveDirection == dir.dl)
        {
            AttackMoveLocal(-1, -1);
        }
    }

    ///Returns the direction that we want to move
    private dir GetMoveDirection()
    {
        float leastDistance = float.MaxValue;
        dir moveDirection = dir.none;

        int unitx = (int)transform.position.x;
        int unity = (int)transform.position.y;

        float currentDistance = GetMoveMap()[unitx, unity].distance;

        UnityEngine.Random rand = new UnityEngine.Random();

        int startingDirection = (int)Math.Floor(4*UnityEngine.Random.value)*2;
        for(int direction = 0; direction < 8; direction++)

        switch ((direction+startingDirection)%8)
        {
            case 0:
                //up
                float thisDistance = GetMoveMap()[unitx, unity + 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx, unity + 1].distance;
                    moveDirection = dir.up;
                }
                break;
            case 1:
                //ur
                thisDistance = GetMoveMap()[unitx + 1, unity + 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx + 1, unity + 1].distance;
                    moveDirection = dir.ur;
                }
                break;
            case 2:
                //right
                thisDistance = GetMoveMap()[unitx + 1, unity].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx + 1, unity].distance;
                    moveDirection = dir.right;
                }
                break;
            case 3:
                //dr
                thisDistance = GetMoveMap()[unitx + 1, unity - 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx + 1, unity - 1].distance;
                    moveDirection = dir.dr;
                }
                break;
            case 4:
                //down
                thisDistance = GetMoveMap()[unitx, unity - 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx, unity - 1].distance;
                    moveDirection = dir.down;
                }
                break;
            case 5:
                //dl
                thisDistance = GetMoveMap()[unitx - 1, unity - 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx - 1, unity - 1].distance;
                    moveDirection = dir.dl;
                }
                break;
            case 6:
                //left
                thisDistance = GetMoveMap()[unitx - 1, unity].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx - 1, unity].distance;
                    moveDirection = dir.left;
                }
                break;
            case 7:
                //ul
                thisDistance = GetMoveMap()[unitx - 1, unity + 1].distance;
                if (thisDistance < leastDistance && thisDistance < currentDistance)
                {
                    leastDistance = GetMoveMap()[unitx - 1, unity + 1].distance;
                    moveDirection = dir.ul;
                }
                break;
        }
        return moveDirection;
    }

    private Node[,] GetMoveMap()
    {
        //Combine the flee and toPlayer maps
        Node[,] newMap = Pathfinding.MapAdd(Pathfinding.MapMultiply(Pathfinding.fleePlayerMap, fear), Pathfinding.MapMultiply(Pathfinding.toPlayerMap, aggression));

        //Combine in the greedMap
        newMap = Pathfinding.MapAdd(newMap, Pathfinding.MapMultiply(Pathfinding.toGoldMap, greed));
        return newMap;
    }

    //------------------------------------------------------STATS--------------------------------------------------------//
    
    public int level = 1;

    public int strength = 5;
    public int speed = 5;
    public int intelligence = 5;
    public int dexterity = 5;
    public int endurance = 5;
    public int wisdom = 5;

    private int health;

    public Healthbar myHealthbar;

    //returns the current maximum health of the character.
    public int GetMaxHealth()
    {
        return 5 + level * endurance;
    }

    public void SetHealth( int h )
    {
        health = h;
        myHealthbar.SetHealth((float)health/(float)GetMaxHealth());
        if (health <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        PlayAnimations();
    }
    // ------------------------------------------------------ANIMATION STUFF--------------------------------------------------------//

    public enum AnimationStateEnum { IdleAnimation, MoveAnimation, AttackAnimation, EatAnimation, HitAnimation, ShakeAnimation, BounceAnimation }

    private float animationTime = 0.0f;
    private float animationSpeed = 5f;
    private bool isAnimating = false;
    private AnimationStateEnum animationStateCurrent = AnimationStateEnum.IdleAnimation;

    private float animationParam1 = 0;
    private float animationParam2 = 0;
    private float animationParam3 = 0;

    //Start an animation with various parameters
    public void StartAnimation(AnimationStateEnum animState, float speed = 5, float param1 = 1, float param2 = 0, float param3 = 1)
    {
        animationSpeed = speed;
        animationParam1 = param1;
        animationParam2 = param2;
        animationParam3 = param3;

        isAnimating = true;
        animationTime = 0.0f;

        animationStateCurrent = animState;

        //This is where we would set the animation speed.
        switch (animState)
        {
            case AnimationStateEnum.MoveAnimation:
                break;
            case AnimationStateEnum.AttackAnimation:
                break;
            case AnimationStateEnum.EatAnimation:
                break;
            case AnimationStateEnum.HitAnimation:
                break;
        }
    }


    //perform different sprite movements depending on the current animation
    void PlayAnimations()
    {
        if (isAnimating)
        {
            animationTime += Time.deltaTime * animationSpeed;

            
            if (animationStateCurrent == AnimationStateEnum.BounceAnimation)
            {
                if (animationTime < 0.5f)
                {
                    mySpriteRenderer.transform.localPosition = new Vector3(animationTime * animationParam1, animationTime * animationParam2, 0) * animationParam3;
                }
                else
                {
                    mySpriteRenderer.transform.localPosition = new Vector3((1 - animationTime) * animationParam1, (1 - animationTime) * animationParam2, 0) * animationParam3;
                }
            }
            else if (animationStateCurrent == AnimationStateEnum.MoveAnimation)
            {
                mySpriteRenderer.transform.localPosition = new Vector3((1-animationTime) * -animationParam1, (1 - animationTime) * -animationParam2, 0);
            }
            else if (animationStateCurrent == AnimationStateEnum.ShakeAnimation)
            {
                mySpriteRenderer.transform.localPosition = new Vector3((float)(Math.Sin(animationTime * animationParam3) * animationParam1), (float)(Math.Sin(animationTime * animationParam3) * animationParam2), 0);
            }

            if (animationTime >= 1.0f)
            {
                mySpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);
            }
            if (animationTime >= 1.0f)
            {
                isAnimating = false;
            }

        }
    }


    // --------------------------------------------------------------MOVEMENT-----------------------------------------------------------//
    //try moving to a position local to here.
    public bool TryMoveToLocationLocal(int deltaX, int deltaY)
    {
        MapController mapController = GameController.mapC;
        int newX = Helper.GetX(this.gameObject) + deltaX;
        int newY = Helper.GetY(this.gameObject) + deltaY;

        bool areaClear = GameController.wallC.GetWall(new Vector2Int(newX, newY)) == null;

        areaClear = (areaClear &&
                    newX >= 0 &&
                    newX < GameController.gameC.ScreenResInUnits.x &&
                    newY >= 0 &&
                    newY < GameController.gameC.ScreenResInUnits.y);

        if (areaClear)
        {
            MoveToLocationLocal(deltaX, deltaY);
            return true;
        }

        return false;
    }

    ///try moving to or attacking a location.
    public virtual bool AttackMoveLocal(int deltaX, int deltaY)
    {
        MapController mapController = GameController.mapC;
        int newX = Helper.GetX(this.gameObject) + deltaX;
        int newY = Helper.GetY(this.gameObject) + deltaY;


        bool inMap = (newX >= 0 &&
                    newX < GameController.gameC.ScreenResInUnits.x &&
                    newY >= 0 &&
                    newY < GameController.gameC.ScreenResInUnits.y);

        UnitController unitController = GameController.unitC;
        foreach (Unit u in unitController.unitList)
        {
            if (u != this && u.transform.position == transform.position + new Vector3(deltaX, deltaY))
            {
                int damageOutput = 1 + weaponAttackBonus;
                u.SetHealth(u.health - damageOutput);
                StartAnimation(AnimationStateEnum.BounceAnimation, 7, deltaX, deltaY, 1.2f);
                u.StartAnimation(AnimationStateEnum.BounceAnimation, 5, deltaX, deltaY, 0.3f);
                GameController.logC.NewEntry("<d>The " + instanceName + "<d> attacks " + u.instanceName + "<d> for <color.red>" + damageOutput + "<d> damage.");

                return false;
            }
        }


        bool areaClear = GameController.wallC.GetWall(new Vector2Int(newX, newY)) == null;


        if (areaClear)
        {
            MoveToLocationLocal(deltaX, deltaY);
            return true;
        }

        return false;
    }

    public void MoveToLocationLocal(int deltaX, int deltaY)
    {
        StartAnimation(AnimationStateEnum.MoveAnimation, 5, deltaX, deltaY);
        int newX = Helper.GetX(this.gameObject) + deltaX;
        int newY = Helper.GetY(this.gameObject) + deltaY;
        SetLocation(newX, newY);
    }

    //set the location
    public void SetLocation(int x, int y)
    {
        transform.position = new Vector3(x, y);
    }

    //set the x position
    public void SetX(int x)
    {
        transform.position = new Vector3(x, transform.position.y);
    }

    //set the y position
    public void SetY(int y)
    {
        transform.position = new Vector3(transform.position.x, y);
    }

    /// <summary>
    /// Returns the inventory located beneath the unit
    /// </summary>
    /// <returns></returns>
    public Inventory GetInventoryBelow()
    {
        return GameController.itemC.inventoryArray[(int)(transform.position.x), (int)(transform.position.y)];
    }

    /// <summary>
    /// Currently only picks up the first available stack under the unit
    /// </summary>
    public virtual void PickUp()
    {
        //pick up the first item available (for now)
        Inventory sourceInventory = GetInventoryBelow();

        int itemIndex = sourceInventory.GetAvailableStack();
        
        if(sourceInventory.MoveItem(itemIndex, this.inventory, -1))
        {
            RoguelikeObject pickupItem = sourceInventory.GetItem(sourceInventory.GetAvailableStack());
            GameController.logC.NewEntry(name + "<d> obtained " + pickupItem.GetFullName() + "<d>.");
        }
    }

    /// <summary>
    /// Drop an Item from the inventory
    /// </summary>
    /// <param name="inv">The inv we are dropping from</param>
    /// <param name="item">The item we are droping</param>
    /// <param name="xOffset">x offset from unit</param>
    /// <param name="yOffset">y offset from unit</param>
    /// <returns>returns true if the drop was successful</returns>
    public bool Drop(Inventory inv, RoguelikeObject item, int xOffset = 0, int yOffset = 0)
    {
        int groundX = (int)(transform.position.x) + xOffset;
        int groundY = (int)(transform.position.y) + yOffset;

        if (groundX < 0 || groundX >= GameController.gameC.ScreenResInUnits.x ||
            groundY < 0 || groundY >= GameController.gameC.ScreenResInUnits.y)
        {
            return false;
        }

        Inventory invBelow = GameController.itemC.inventoryArray[groundX, groundY];

        if (invBelow)
        {
            bool itemAdded = invBelow.AddItem(item);
            if (itemAdded)
            {
                inv.RemoveItem(item);
                return true;
            }
            else
            {
                //eventually, we will want to recursively iterate through the level grid until we find a free space.
                //Drop item in other slot
                return false;
            }
        }

        return false;
    }
}
