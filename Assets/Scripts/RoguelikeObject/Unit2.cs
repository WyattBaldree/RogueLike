using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class Unit2 : WorldObject
{
    [Header("Unit")]
    public Inventory inventory;

    public float greed = 0.5f;
    public float fear = 0.1f;
    public float aggression = 0.8f;

    public int level = 1;

    public int strength = 5;
    public int speed = 5;
    public int intelligence = 5;
    public int dexterity = 5;
    public int endurance = 5;
    public int wisdom = 5;

    public bool burrower = false;

    //our current speed counter. When this reaches 0, take an action.
    protected int speedCounter = 0;


    /// <summary>
    /// Kill the unit
    /// </summary>
    public void Die()
    {
        
    }

    public virtual void Step()
    {
        speedCounter--;
        if (speedCounter < 0)
        {
            //Readjust our speed after we take our action. !!!!placeholder. this should be dependant on the action taken.!!!!!
            speedCounter += 20 - speed;

            //figure out which direction we want to move
            Vector2Int moveDirection = GetMoveDirection();

            AttackMove(GetPositionOffset(moveDirection));
        }
    }
    
    ///Returns the direction that we want to move in as a Vector2Int
    private Vector2Int GetMoveDirection()
    {
        float leastDistance = float.MaxValue;
        Vector2Int moveDirection = new Vector2Int(0, 0);

        int unitx = (int)transform.position.x;
        int unity = (int)transform.position.y;

        float currentDistance = GetMoveMap()[unitx, unity].distance;

        UnityEngine.Random rand = new UnityEngine.Random();

        int startingDirection = (int)Mathf.Floor(4 * UnityEngine.Random.value) * 2;
        for (int direction = 0; direction < 8; direction++)

            switch ((direction + startingDirection) % 8)
            {
                case 0:
                    //up
                    float thisDistance = GetMoveMap()[unitx, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx, unity + 1].distance;
                        moveDirection = new Vector2Int(0, 1);
                    }
                    break;
                case 1:
                    //ur
                    thisDistance = GetMoveMap()[unitx + 1, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx + 1, unity + 1].distance;
                        moveDirection = new Vector2Int(1, 1);
                    }
                    break;
                case 2:
                    //right
                    thisDistance = GetMoveMap()[unitx + 1, unity].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx + 1, unity].distance;
                        moveDirection = new Vector2Int(1, 0);
                    }
                    break;
                case 3:
                    //dr
                    thisDistance = GetMoveMap()[unitx + 1, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx + 1, unity - 1].distance;
                        moveDirection = new Vector2Int(1, -1);
                    }
                    break;
                case 4:
                    //down
                    thisDistance = GetMoveMap()[unitx, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx, unity - 1].distance;
                        moveDirection = new Vector2Int(0, -1);
                    }
                    break;
                case 5:
                    //dl
                    thisDistance = GetMoveMap()[unitx - 1, unity - 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx - 1, unity - 1].distance;
                        moveDirection = new Vector2Int(-1, -1);
                    }
                    break;
                case 6:
                    //left
                    thisDistance = GetMoveMap()[unitx - 1, unity].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx - 1, unity].distance;
                        moveDirection = new Vector2Int(-1, 0);
                    }
                    break;
                case 7:
                    //ul
                    thisDistance = GetMoveMap()[unitx - 1, unity + 1].distance;
                    if (thisDistance < leastDistance && thisDistance < currentDistance)
                    {
                        leastDistance = GetMoveMap()[unitx - 1, unity + 1].distance;
                        moveDirection = new Vector2Int(-1, 1);
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

    ///try moving to or attacking a location.
    public virtual bool AttackMove(Vector2Int targetDestination)
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

    public virtual bool MeleeAttack(RoguelikeObject attackTarget)
    {
        if (attackTarget)
        {
            int damageOutput = 1;
            attackTarget.Health -= damageOutput;
            Vector2Int positionDelta = attackTarget.GetPosition() - GetPosition();
            myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.BounceAnimation, 7, positionDelta.x, positionDelta.y, 1.2f);
            attackTarget.myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.BounceAnimation, 7, positionDelta.x, positionDelta.y, .3f);
            GetLogController().NewEntry("<d>The " + GetFullName() + "<d> attacks " + attackTarget.GetFullName() + "<d> for <color.red>" + damageOutput + "<d> damage.");
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
        Wall2 wall = GetWallController().GetWall(pos);
        bool blockedByWall = wall && wall.BlockUnits && !burrower;

        return GetWorldObjectInventory(pos).GetFirstAvailableSlot() != -1 && !blockedByWall;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        UnitController.unitList.Add(this);
    }

    public override void DestroyObject()
    {
        Assert.IsTrue(UnitController.unitList.Remove(this), "The roguelikeObject being destroyed was not in the RoguelikeObjectList upon being destroyed. Something is terribly wrong.");
        base.DestroyObject();
    }
}
