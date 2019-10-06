using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameController;

public abstract class Equipment : RoguelikeObject
{
    /// <summary>
    /// equip this equipment in the first available equipment slot
    /// </summary>
    public abstract void Equip();

    public override List<Interaction> GetInteractions()
    {
        List<Interaction> newInteractionList =  base.GetInteractions();
        newInteractionList.Add(new Interaction("Equip", "Equip this item.", Equip));
        return newInteractionList;
    }
}
