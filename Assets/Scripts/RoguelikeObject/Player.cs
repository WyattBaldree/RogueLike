using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Player : Unit
{
    

    /// <summary>
    /// Take the player's turn. Called repeatedly in update on the player's turn.
    /// </summary>
    public void Turn()
    {
        //MapController mapController = GameController.mapC;

        if (Input.GetKeyDown("up"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(0, 1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            //PathFinding.GenerateFleeMap((int)transform.position.x, (int)transform.position.y);
            EndTurn();
        }
        else if (Input.GetKeyDown("down"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(0, -1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("left"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(-1, 0))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("right"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(1, 0))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("7"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(-1, 1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("1"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(-1, -1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("3"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(1, -1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("9"))
        {
            if (AttackMove(GetPositionOffset(new Vector2Int(1, 1))))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("space"))
        {
            myRogueSpriteRenderer.StartAnimation(RogueSpriteRenderer.AnimationStateEnum.ShakeAnimation, 5, 1);
        }
        else if (Input.GetKeyDown("p"))
        {
            Debug.Log("pickup");
            PickUp();
            //GameController.inventoryC.UpdateFrameSprites();

            EndTurn();
        }
    }

    /// <summary>
    /// End the player's turn and finish the rest of the units' steps.
    /// </summary>
    private void EndTurn()
    {
        GetUnitController().FinishStep();
    }

    /// <summary>
    /// Open the inventory below the player.
    /// </summary>
    private void PickUp()
    {
        //base.PickUp();
        //inventory.ShowInventoryGUI();
        GetPopupController().containerGUI.Popup(new Vector2(), GetInventoryBelow());
    }

    public override void Initialize()
    {
        base.Initialize();
        GetUnitController().player = this;
        GetGUIController().mainInventoryGUI.ConnectToInventory(UnitInventory);
    }

    public override bool MoveToLocation(Vector2Int targetDestination)
    {
        GetPopupController().containerGUI.Hide();
        if (base.MoveToLocation(targetDestination))
        {
            Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            return true;
        }
        return false;
    }

    protected override void TakeTurn()
    {
        //take turn
        UnitController unitController = GetUnitController();
        unitController.gameState = UnitController.GameStateEnum.playerTurn;
    }
}
