using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    //on player step, subtract our speed and potentially start our turn.
    public void Step()
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

    public void PickUp()
    {
        //base.PickUp();
        //inventory.ShowInventoryGUI();
        GetInventoryBelow().Open(transform.position);
    }

    public override bool AttackMoveLocal(int deltaX, int deltaY)
    {
        GetInventoryBelow().Close();
        return base.AttackMoveLocal(deltaX, deltaY);
    }

    public override void Initialize()
    {
        base.Initialize();

        //Hook up the inventorys to the correct PickUpDrops

        slotWeapon.myInventoryGUIEnum = InventoryController.inventoryEnum.weapon;
        slotWeapon.Initialize();

        slotHelmet.myInventoryGUIEnum = InventoryController.inventoryEnum.helmet;
        slotHelmet.Initialize();

        slotChest.myInventoryGUIEnum = InventoryController.inventoryEnum.chest;
        slotChest.Initialize();

        slotGloves.myInventoryGUIEnum = InventoryController.inventoryEnum.gloves;
        slotGloves.Initialize();

        slotBoots.myInventoryGUIEnum = InventoryController.inventoryEnum.boots;
        slotBoots.Initialize();

       /*slotGloves
        slotGreaves
        slotBoots
        slotRing1
        slotRing2
        slotAmulet*/
    }
}
