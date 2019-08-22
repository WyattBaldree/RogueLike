using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class Unit : WorldObject
{
    public static List<Unit> unitList = new List<Unit>();

    [Header("Unit")]
    [SerializeField]
    private Inventory unitTnventory;
    /// <summary>
    /// The inventory of this unit.
    /// </summary>
    public Inventory UnitInventory { get => unitTnventory; }

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

            if (!attackTarget.AttemptDodge(this))
            {
                int damageOutput = 1;
                GetLogController().NewEntry("<d>The " + GetFullName() + "<d> attacks the " + attackTarget.GetFullName() + "<d> for <color.red>" + damageOutput + "<d> damage.");
                attackTarget.TakeDamage(this, damageOutput);
                return true;
            }
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
}
