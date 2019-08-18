using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    //on player step, subtract our speed and potentially start our turn.
    public override void Step()
    {
        speedCounter--;
        if (speedCounter < 0)
        {
            speedCounter += 20 - speed;

            //take turn
            UnitController unitController = GameController.unitC;
            unitController.gameState = UnitController.GameStateEnum.playerTurn;
        }
    }

    //on the player turn, decide what action to take.
    public void Turn()
    {
        MapController mapController = GameController.mapC;

        if (Input.GetKeyDown("up"))
        {
            if (AttackMoveLocal(0, 1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            //PathFinding.GenerateFleeMap((int)transform.position.x, (int)transform.position.y);
            EndTurn();
        }
        else if(Input.GetKeyDown("down"))
        {
            if(AttackMoveLocal(0, -1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if(Input.GetKeyDown("left"))
        {
            if(AttackMoveLocal(-1, 0))
                Pathfinding.GenerateToPlayerMap();
            Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("right"))
        {
            if(AttackMoveLocal(1, 0))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        if (Input.GetKeyDown("7"))
        {
            if(AttackMoveLocal(-1, 1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("1"))
        {
            if(AttackMoveLocal(-1, -1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("3"))
        {
            if(AttackMoveLocal(1, -1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("9"))
        {
            if(AttackMoveLocal(1, 1))
                Pathfinding.GenerateToPlayerMap();
                Pathfinding.GenerateFleePlayerMap();
            EndTurn();
        }
        else if (Input.GetKeyDown("space"))
        {
            StartAnimation(AnimationStateEnum.ShakeAnimation, 5,  1);
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
        GameController.unitC.FinishStep();
    }

    public override void PickUp()
    {
        //base.PickUp();
        //inventory.ShowInventoryGUI();
        GameController.popupC.containerGUI.Popup(new Vector2(), GetInventoryBelow());
    }

    public override bool AttackMoveLocal(int deltaX, int deltaY)
    {
        GameController.popupC.containerGUI.Hide();
        return base.AttackMoveLocal(deltaX, deltaY);
    }

    public override void Initialize()
    {
        base.Initialize();
        GameController.inventoryC.inventoryGUIs[1].ConnectToInventory(inventory);
    }
}
