using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This script acts as an interface between the various objects that exist in the game 
 * and the right-click context menu system. Interactions are an event and a name. 
 * Interactions will be defined in the inspector which involves naming them and attaching
 * each event to a function.
 */

[System.Serializable]
public struct Interaction
{
    public string interactionName;
    public UnityEvent interactionEvent;
}

public class Interactive : MonoBehaviour
{
    public List<Interaction> myInteractions;

    public bool Interact(int index = 0)
    {
        if (index >= myInteractions.Count) return false;
        myInteractions[index].interactionEvent.Invoke();
        return true;
    }

    public void ShowMenu()
    {
        GameController.interactiveGUI.Show(this);
    }

    public void HideMenu()
    {
        GameController.interactiveGUI.Hide();
    }

}
