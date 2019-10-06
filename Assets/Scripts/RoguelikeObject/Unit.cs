using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;
using static StringHelper;

public class Unit : WorldObject
{
    public static List<Unit> unitList = new List<Unit>();
    public enum BodyTypeEnum { humanoid, mammal }
    public enum GenderTypeEnum { animal, male, female }

    [Header("Unit")]

    [SerializeField]
    private int speed = 5;
    /// <summary>
    /// How fast does this unit move? Factors into how often we take turns.
    /// </summary>
    public int Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField]
    private int reach = 2;
    /// <summary>
    /// How far can this unit reach?
    /// </summary>
    public int Reach
    {
        get => reach;
        set => reach = value;
    }

    [SerializeField]
    private bool burrower = false;
    /// <summary>
    /// Can this unit move through walls?
    /// </summary>
    public bool Burrower
    {
        get => burrower;
        set => burrower = value;
    }

    [SerializeField]
    private bool dead = false;
    /// <summary>
    /// Is this unit dead?
    /// </summary>
    public bool Dead
    {
        get => dead;
        set
        {
            dead = value;
            UpdateRogueSpriteRenderer();
        }
    }

    [SerializeField]
    private BodyTypeEnum bodyType = BodyTypeEnum.humanoid;
    /// <summary>
    /// What body shape does this unit have?
    /// </summary>
    public BodyTypeEnum BodyType
    {
        get => bodyType;
        set
        {
            bodyType = value;
        }
    }

    [SerializeField]
    private GenderTypeEnum gender = GenderTypeEnum.animal;
    public GenderTypeEnum Gender{
        get => gender;
        set => gender = value;
    }

    //The root bodypart of the creature. Other bits get chopped off of this bit
    [System.NonSerialized]
    public Gib rootBodyPart;

    //A list of all bodyparts
    [System.NonSerialized]
    public List<Gib> bodyParts = new List<Gib>();
    
    //A list of all body parts that can gras
    [System.NonSerialized]
    public List<Gib> bodyPartsGrasping = new List<Gib>();

    //A list of all bodyparts that can think
    [System.NonSerialized]
    public List<Gib> bodyPartsThinking = new List<Gib>();

    //A list of all bodyparts that are used for locomotion
    [System.NonSerialized]
    public List<Gib> bodyPartsLocomotion = new List<Gib>();

    //A list of all bodyparts that can be attacked with
    [System.NonSerialized]
    public List<Gib> bodyPartsAttacking = new List<Gib>();

    //The number of legs this creature is supposed to have
    public int intendedNumberOfLegs = 0;

    //our current speed counter. When this reaches 0, take an action.
    private int turnCounter = 0;

    /// <summary>
    /// Returns the direction that we want to move in as a Vector2Int
    /// </summary>
    private Vector2Int GetMoveDirection()
    {
        float leastDistance = float.MaxValue;
        Vector2Int moveDirection = new Vector2Int(0, 0);

        int unitx = (int)transform.position.x;
        int unity = (int)transform.position.y;

        Node[,] moveMap = GetMoveMap();

        float currentDistance = moveMap[unitx, unity].distance;

        UnityEngine.Random rand = new UnityEngine.Random();
        int startingDirection = (int)Mathf.Floor(4 * UnityEngine.Random.value) * 2;
        for (int direction = 0; direction < 8; direction++)

            switch ((direction + startingDirection) % 8)
            {
                case 0:
                    //up
                    float thisDistance = moveMap[unitx, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx, unity + 1].distance;
                        moveDirection = new Vector2Int(0, 1);
                    }
                    break;
                case 1:
                    //ur
                    thisDistance = moveMap[unitx + 1, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx + 1, unity + 1].distance;
                        moveDirection = new Vector2Int(1, 1);
                    }
                    break;
                case 2:
                    //right
                    thisDistance = moveMap[unitx + 1, unity].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx + 1, unity].distance;
                        moveDirection = new Vector2Int(1, 0);
                    }
                    break;
                case 3:
                    //dr
                    thisDistance = moveMap[unitx + 1, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx + 1, unity - 1].distance;
                        moveDirection = new Vector2Int(1, -1);
                    }
                    break;
                case 4:
                    //down
                    thisDistance = moveMap[unitx, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx, unity - 1].distance;
                        moveDirection = new Vector2Int(0, -1);
                    }
                    break;
                case 5:
                    //dl
                    thisDistance = moveMap[unitx - 1, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx - 1, unity - 1].distance;
                        moveDirection = new Vector2Int(-1, -1);
                    }
                    break;
                case 6:
                    //left
                    thisDistance = moveMap[unitx - 1, unity].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx - 1, unity].distance;
                        moveDirection = new Vector2Int(-1, 0);
                    }
                    break;
                case 7:
                    //ul
                    thisDistance = moveMap[unitx - 1, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = moveMap[unitx - 1, unity + 1].distance;
                        moveDirection = new Vector2Int(-1, 1);
                    }
                    break;
            }
        return moveDirection;
    }

    /// <summary>
    /// Combines the different pathfinding maps and returns the result.
    /// </summary>
    /// <returns></returns>
    private Node[,] GetMoveMap()
    {
        /*//Combine the flee and toPlayer maps
        Node[,] newMap = Pathfinding.MapAdd(Pathfinding.MapMultiply(Pathfinding.fleePlayerMap, fear), Pathfinding.MapMultiply(Pathfinding.toPlayerMap, aggression));

        //Combine in the greedMap
        newMap = Pathfinding.MapAdd(newMap, Pathfinding.MapMultiply(Pathfinding.toGoldMap, greed));*/
        return Pathfinding.toPlayerMap;
    }

    /// <summary>
    /// Try moving to a location and, if we cannot, attack what is blocking us.
    /// </summary>
    /// <param name="targetDestination"></param>
    /// <returns></returns>
    protected bool AttackMove(Vector2Int targetDestination)
    {
        if (MoveToLocation(targetDestination))
        {
            return true;
        }

        WorldObject targetObject = GetUnitController().GetUnit(targetDestination);
        if(!targetObject) targetObject = GetWallController().GetWall(targetDestination);
        if (!targetObject) targetObject = GetFloorController().GetFloor(targetDestination);

        MeleeAttack(targetObject);

        return false;
    }

    /// <summary>
    /// This function is used to make the unit move an item from one inventory to another. Returns true if the item was moved.
    /// </summary>
    /// <param name="rlObject"></param>
    /// <param name="targetInv"></param>
    /// <param name="targetIndex"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool MoveItem(RoguelikeObject rlObject, Inventory targetInv, int targetIndex = -1, int amount = -1)
    {
        Inventory sourceInv = rlObject.ParentInventory;

        float distance1 = Vector2.Distance(rlObject.GetPosition(), GetPosition());
        float distance2 = Vector2.Distance(new Vector2(targetInv.transform.position.x, targetInv.transform.position.y), GetPosition());
        if (distance1 < reach && distance2 < reach)
        {
            Debug.Log("You can reach that far.");

            RoguelikeObject newStack = sourceInv.MoveItem(rlObject, targetInv, targetIndex, amount);
            return true;
        }
        else
        {
            Debug.Log("You can't reach that far.");
            return false;
        }
    }

    /// <summary>
    /// Melee attack a RoguelikeObject.
    /// </summary>
    /// <param name="attackTarget"></param>
    /// <returns></returns>
    private bool MeleeAttack(RoguelikeObject attackTarget)
    {
        if (attackTarget)
        {
            Vector2Int positionDelta = attackTarget.GetPosition() - GetPosition();
            myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.BounceAnimation, 7, positionDelta.x, positionDelta.y, 1.2f);

            bool atLeastOneHit = false;
            foreach (Gib gib in bodyPartsAttacking)
            {
                if (gib.Attacking)
                {
                    bool isUnarmedAttack;

                    int damageOutput;
                    string attackVerb;
                    string itemNoun;
                    if (gib.Grasping == false || gib.graspInv.itemList[0] == null)
                    {
                        //unarmed
                        isUnarmedAttack = true;
                        damageOutput = 1;
                        attackVerb = gib.AttackVerb;
                        itemNoun = GetPossessiveDeterminer() + " " + gib.partName;
                    }
                    else
                    {
                        isUnarmedAttack = false;
                        damageOutput = gib.graspInv.itemList[0].MeleeDamage;
                        attackVerb = gib.graspInv.itemList[0].GetAttackVerb();
                        itemNoun = gib.graspInv.itemList[0].GetFullName();
                    }

                    if (!attackTarget.AttemptDodge(this))
                    {
                        if (isUnarmedAttack)
                        {
                            GetLogController().NewEntry("<d>" + CapitalizeFirst(GetFullName()) + "<d> " + attackVerb + "<d> " + attackTarget.GetFullName() + "<d> for <color.red>" + damageOutput + "<d> damage.");
                        }
                        else
                        {
                            GetLogController().NewEntry("<d>" + CapitalizeFirst(GetFullName()) + "<d> " + attackVerb + "<d> " + attackTarget.GetFullName() + "<d> with " + itemNoun + "<d> for <color.red>" + damageOutput + "<d> damage.");
                        }
                        attackTarget.TakeDamage(this, damageOutput);
                        atLeastOneHit = true;
                    }
                }
            }
            return atLeastOneHit;
        }
        return false;
    }

    /// <summary>
    /// Here the unit decides what it wants to do and does it. Called when the unit's stepCounter counts down to 0;
    /// </summary>
    protected virtual void TakeTurn()
    {
        if (Dead) return;

        //figure out which direction we want to move
        Vector2Int moveDirection = GetMoveDirection();

        AttackMove(GetPositionOffset(moveDirection));
    }

    /// <summary>
    /// Create all of the gibs(body parts) taht amake up this unit
    /// </summary>
    protected virtual Gib MakeBody(BodyTypeEnum bodyTypeParam)
    {
        //Remove all of the old body.
        bodyParts.Clear();
        bodyPartsAttacking.Clear();
        bodyPartsGrasping.Clear();
        bodyPartsLocomotion.Clear();
        bodyPartsThinking.Clear();

        if (bodyTypeParam == BodyTypeEnum.humanoid)
        {
            //create torso
            Gib torso = rootBodyPart = new Gib(this, "torso", null);
            torso.ArmorType = RoguelikeObject.TagEnum.torso;

            //create arms
            Gib leftArm = new Gib(this, "left arm", torso);
            leftArm.ArmorType = RoguelikeObject.TagEnum.arm;
            leftArm.Grasping = true;
            leftArm.Attacking = true;

            Gib rightArm = new Gib(this, "right arm", torso);
            rightArm.ArmorType = RoguelikeObject.TagEnum.arm;
            rightArm.Grasping = true;
            rightArm.Attacking = true;

            //create legs
            Gib leftLeg = new Gib(this, "left leg", torso);
            leftLeg.ArmorType = RoguelikeObject.TagEnum.leg;
            leftLeg.Locomoting = true;

            Gib rightLeg = new Gib(this, "right leg", torso);
            rightLeg.ArmorType = RoguelikeObject.TagEnum.leg;
            rightLeg.Locomoting = true;

            //create head
            Gib head = new Gib(this, "head", torso);
            head.ArmorType = RoguelikeObject.TagEnum.head;
            head.Thinking = true;

            intendedNumberOfLegs = 2;
        }
        else if (bodyTypeParam == BodyTypeEnum.mammal)
        {
            //create torso
            Gib torso = rootBodyPart = new Gib(this, "torso", null);

            //create arms
            Gib frontLeftLeg = new Gib(this, "front left leg", torso);
            frontLeftLeg.Attacking = true;
            frontLeftLeg.Locomoting = true;
            frontLeftLeg.AttackVerb = "scratches";

            Gib frontRightLeg = new Gib(this, "front right leg", torso);
            frontRightLeg.Attacking = true;
            frontRightLeg.Locomoting = true;
            frontRightLeg.AttackVerb = "scratches";

            //create legs
            Gib backLeftLeg = new Gib(this, "back left leg", torso);
            backLeftLeg.Locomoting = true;

            Gib backRightLeg = new Gib(this, "back right leg", torso);
            backRightLeg.Locomoting = true;

            //create head
            Gib head = new Gib(this, "head", torso);
            head.Thinking = true;
            head.Attacking = true;
            head.AttackVerb = "bites";

            intendedNumberOfLegs = 4;
        }
        BodyType = bodyTypeParam;
        return rootBodyPart;
    }

    /// <summary>
    /// This function returns the possesive determiner (its, his, her) for this unit based on its gender
    /// </summary>
    /// <returns></returns>
    public string GetPossessiveDeterminer()
    {
        switch (Gender)
        {
            case GenderTypeEnum.animal:
                return "its";
            case GenderTypeEnum.male:
                return "his";
            case GenderTypeEnum.female:
                return "her";
        }
        return "its";
    }
    
    public override void Die()
    {
        if (Dead)
        {
            base.Die();
        }
        else
        {
            GetLogController().NewEntry("<d>The " + GetFullName() + "<d> dies...<color.red><size.13> HORRIBLY<d>.");
            Dead = true;
            Health = HealthMax;
            Take(GetInventoryBelow());
        }
    }

    public override void UpdateRogueSpriteRenderer()
    {
        myRogueSpriteRenderer.Dead = dead;
        base.UpdateRogueSpriteRenderer();
    }

    public override void Step()
    {
        base.Step();
        turnCounter--;
        if (turnCounter < 0)
        {
            //Readjust our speed after we take our action. !!!!placeholder. this should be dependant on the action taken.!!!!!
            turnCounter += 20 - speed;

            TakeTurn();
        }
    }

    public override bool AttemptDodge(RoguelikeObject source)
    {
        if(Random.Range(0, 2) == 1)
        {
            Dodge(source);
            return true;
        }
        return false;
    }

    public override void Initialize()
    {
        base.Initialize();

        MakeBody(BodyType);
    }

    

    public override Inventory GetWorldObjectInventory(Vector2Int pos)
    {
        return GetUnitController().GetUnitInventory(pos);
    }

    public override bool IsSpaceFree(Vector2Int pos)
    {
        if (!base.IsSpaceFree(pos)) return false;
        Wall wall = GetWallController().GetWall(pos);
        bool blockedByWall = wall && wall.BlockUnits && !burrower;

        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1 && !blockedByWall;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        Unit.unitList.Add(this);
    }

    public override void DestroyObject()
    {
        Assert.IsTrue(Unit.unitList.Remove(this), "The unit being destroyed was not in the UnitList upon being destroyed. Something is terribly wrong.");
        base.DestroyObject();
    }

    public override string GetFullName()
    {
        return "<d>the " + ObjectName;
    }
}

//a gib represents a body part on the creature
//can be cut off, puched, and potentialy unique attributes
//for example, can this gib hold a weapon? An arm.
public class Gib : Object
{
    private bool grasping = false;
    public bool Grasping
    {
        get => grasping;
        set
        {
            if (value)
            {
                grasping = true;
                if (graspInv == null)
                {
                    graspInv = Instantiate<Inventory>(GetGameController().singleInventorySource, parentUnit.transform);
                    graspInv.transform.localPosition = new Vector3();
                    graspInv.inventoryName = "Weapon Slot " + partName;
                    graspInv.gameObject.name = graspInv.inventoryName;
                    graspInv.acceptableTypes.Add(RoguelikeObject.TagEnum.weapon);
                    graspInv.acceptableTypes.Add(RoguelikeObject.TagEnum.shield);
                }
                parentUnit.bodyPartsGrasping.Add(this);
                //drop weapon
            }
            else
            {
                grasping = false;
                parentUnit.bodyPartsGrasping.Remove(this);
            }
        }
    }


    private bool thinking = false;
    public bool Thinking
    {
        get => thinking;
        set
        {
            if (value)
            {
                thinking = true;
                parentUnit.bodyPartsThinking.Add(this);
                //drop weapon
            }
            else
            {
                thinking = false;
                parentUnit.bodyPartsThinking.Remove(this);
            }
        }
    }

    private bool attacking = false;
    public bool Attacking
    {
        get => attacking;
        set
        {
            if (value)
            {
                attacking = true;
                parentUnit.bodyPartsAttacking.Add(this);
                //drop weapon
            }
            else
            {
                attacking = false;
                parentUnit.bodyPartsAttacking.Remove(this);
            }
        }
    }

    private string attackVerb = "punches";
    public string AttackVerb
    {
        get => attackVerb;
        set => attackVerb = value;
    }

    private bool locomoting = false;
    public bool Locomoting
    {
        get => locomoting;
        set
        {
            if (value)
            {
                locomoting = true;
                parentUnit.bodyPartsLocomotion.Add(this);
            }
            else
            {
                locomoting = false;
                parentUnit.bodyPartsLocomotion.Remove(this);
            }
        }
    }

    private RoguelikeObject.TagEnum armorType = RoguelikeObject.TagEnum.none;
    public RoguelikeObject.TagEnum ArmorType
    {
        get => armorType;
        set
        {
            armorType = value;
            if (value != RoguelikeObject.TagEnum.none)
            {
                if (armorInv == null)
                {
                    armorInv = Instantiate<Inventory>(GetGameController().singleInventorySource, parentUnit.transform);
                    armorInv.transform.localPosition = new Vector3();
                    armorInv.inventoryName = "Armor Slot " + partName;
                    armorInv.gameObject.name = armorInv.inventoryName;
                }
                armorInv.acceptableTypes.Clear();
                armorInv.acceptableTypes.Add(value);
            }
        }
    }

    public string partName = "torso";
    Gib parentGib = null;
    List<Gib> children = new List<Gib>();
    public Inventory armorInv = null;
    public Inventory graspInv = null;

    public Unit parentUnit;

    public Gib(Unit parent, string newName, Gib newParent)
    {
        partName = newName;

        parentGib = newParent;
        if(parentGib != null) parentGib.children.Add(this);

        parent.bodyParts.Add(this);

        parentUnit = parent;
    }
}
