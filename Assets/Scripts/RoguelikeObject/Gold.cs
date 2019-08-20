﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Gold : RoguelikeObject
{
    public static List<Gold> goldList = new List<Gold>();

    [Header("Gold")]
    public Sprite copperSingle;
    public Sprite copperStack;
    public Sprite copperPile;
    public Sprite silverSingle;
    public Sprite silverStack;
    public Sprite silverPile;
    public Sprite goldSingle;
    public Sprite goldStack;
    public Sprite goldPile;

    public override Sprite GetCurrentSprite()
    {
        if (StackSize > 500)
        {
            return goldPile;
        }
        else if (StackSize > 100)
        {
            return goldStack;
        }
        else if (StackSize == 100)
        {
            return goldSingle;
        }
        else if (StackSize > 50)
        {
            return silverPile;
        }
        else if (StackSize > 10)
        {
            return silverStack;
        }
        else if (StackSize == 10)
        {
            return silverSingle;
        }
        else if (StackSize > 5)
        {
            return copperPile;
        }
        else if (StackSize > 1)
        {
            return copperStack;
        }
        else
        {
            return copperSingle;
        }
    }

    public override void OnCreate()
    {
        base.OnCreate();
        Gold.goldList.Add(this);
    }

    public override void DestroyObject()
    {
        Assert.IsTrue(Gold.goldList.Remove(this), "The gold being destroyed was not in the goldList upon being destroyed. Something is terribly wrong.");
        base.DestroyObject();
    }
}