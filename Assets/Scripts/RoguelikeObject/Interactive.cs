using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class Interactive : MonoBehaviour
{
    public abstract List<Interaction> GetInteractions();
    public abstract string GetInteractiveName();
}

public class Interaction
{
    public Interaction(string n, string d, UnityAction a)
    {
        name = n;
        description = d;
        myAction = a;
    }

    public string name;
    public string description;
    public UnityAction myAction;
}
