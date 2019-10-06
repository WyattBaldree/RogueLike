using UnityEngine;
using System.Collections;
using static GameController;
using System.Collections.Generic;

public class Weapon : Equipment
{
    [SerializeField]
    private string attackVerb = "bashes";
    /// <summary>
    /// The verb that is used when this weapon hits something.
    /// </summary>
    public string AttackVerb
    {
        get => attackVerb;
        set => attackVerb = value;
    }

    public override void Equip()
    {
        Player player = GetUnitController().player;

        foreach (Gib gib in player.bodyParts)
        {
            Debug.Log(gib.partName);
            if (gib.Grasping && gib.graspInv.isEmpty())
            {
                player.MoveItem(this, gib.graspInv);
                break;
            }
        }
    }

    public override string GetAttackVerb()
    {
        return attackVerb;
    }
}
