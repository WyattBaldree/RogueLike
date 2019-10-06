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
        GetPopupController().containerGUI.Popup(new Vector2(0, 0), GetInventoryBelow());
    }

    public override void Initialize()
    {
        base.Initialize();
        GetUnitController().player = this;
        GetGUIController().mainInventoryGUI.ConnectToInventory(MyInventory);
    }

    protected override Gib MakeBody(BodyTypeEnum bodyTypeParam)
    {
        //create torso
        Gib torso = rootBodyPart = new Gib(this, "Torso", null);
        torso.ArmorType = RoguelikeObject.TagEnum.torso;
        GetGUIController().equipmentGUITorso.ConnectToInventory(torso.armorInv);

        //create arms
        Gib leftArm = new Gib(this, "Left Arm", torso);
        leftArm.ArmorType = RoguelikeObject.TagEnum.arm;
        leftArm.Grasping = true;
        leftArm.Attacking = true;
        GetGUIController().equipmentGUILeftArm.ConnectToInventory(leftArm.armorInv);
        GetGUIController().equipmentGUILeftHand.ConnectToInventory(leftArm.graspInv);

        Gib rightArm = new Gib(this, "Right Arm", torso);
        rightArm.ArmorType = RoguelikeObject.TagEnum.arm;
        rightArm.Grasping = true;
        rightArm.Attacking = true;
        GetGUIController().equipmentGUIRightArm.ConnectToInventory(rightArm.armorInv);
        GetGUIController().equipmentGUIRightHand.ConnectToInventory(rightArm.graspInv);

        //create legs
        Gib leftLeg = new Gib(this, "Left Leg", torso);
        leftLeg.ArmorType = RoguelikeObject.TagEnum.leg;
        leftLeg.Locomoting = true;
        GetGUIController().equipmentGUILeftLeg.ConnectToInventory(leftLeg.armorInv);

        Gib rightLeg = new Gib(this, "Right Leg", torso);
        rightLeg.ArmorType = RoguelikeObject.TagEnum.leg;
        rightLeg.Locomoting = true;
        GetGUIController().equipmentGUIRightLeg.ConnectToInventory(rightLeg.armorInv);

        //create head
        Gib head = new Gib(this, "Head", torso);
        head.ArmorType = RoguelikeObject.TagEnum.head;
        head.Thinking = true;
        GetGUIController().equipmentGUIHead.ConnectToInventory(head.armorInv);

        intendedNumberOfLegs = 2;

        return rootBodyPart;
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
        if (Dead) return;

        //take turn
        UnitController unitController = GetUnitController();
        unitController.gameState = UnitController.GameStateEnum.playerTurn;
    }
}
