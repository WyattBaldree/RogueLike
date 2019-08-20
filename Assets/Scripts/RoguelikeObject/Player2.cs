using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Player2 : Unit2
{
    //on player step, subtract our speed and potentially start our turn.
    public override void Step()
    {
        speedCounter--;
        if (speedCounter < 0)
        {
            speedCounter += 20 - speed;

            //take turn
            UnitController unitController = GetUnitController();
            unitController.gameState = UnitController.GameStateEnum.playerTurn;
        }
    }

    //on the player turn, decide what action to take.
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

    void EndTurn()
    {
        GetUnitController().FinishStep();
    }

    public void PickUp()
    {
        //base.PickUp();
        //inventory.ShowInventoryGUI();
        GetPopupController().containerGUI.Popup(new Vector2(), GetInventoryBelow());
    }

    public override bool AttackMove(Vector2Int targetDestination)
    {
        GetPopupController().containerGUI.Hide();
        return base.AttackMove(targetDestination);
    }

    public override void Initialize()
    {
        base.Initialize();
        GetUnitController().player = this;
        GetGUIController().mainInventoryGUI.ConnectToInventory(inventory);
    }
}
