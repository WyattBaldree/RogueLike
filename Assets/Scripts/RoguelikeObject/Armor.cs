using UnityEngine;
using System.Collections;
using static GameController;

public class Armor : Equipment
{
    [Header("Armor")]

    [SerializeField]
    private int armorRating;
    /// <summary>
    /// how good is this armor at protecting the player?
    /// </summary>
    public int ArmorRating
    {
        get => armorRating;
        set => armorRating = value;
    }

    public override void Equip()
    {
        Player player = GetUnitController().player;

        foreach (Gib gib in player.bodyParts)
        {
            if (tags.Contains(gib.ArmorType) && gib.armorInv.isEmpty())
            {
                player.MoveItem(this, gib.armorInv);
                break;
            }
        }
    }
}
