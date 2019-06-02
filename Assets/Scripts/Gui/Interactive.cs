using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    public List<UnityEvent> myInteractions;

    public bool Interact(int index = 0)
    {
        if (index >= myInteractions.Count) return false;
        myInteractions[index].Invoke();
        return true;
    }

}
