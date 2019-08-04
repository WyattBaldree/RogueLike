using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    public static List<Gold> goldList = new List<Gold>();

    public void Start()
    {
        Initialize();
    }
    public Sprite copperSingle;
    public Sprite copperStack;
    public Sprite copperPile;
    public Sprite silverSingle;
    public Sprite silverStack;
    public Sprite silverPile;
    public Sprite goldSingle;
    public Sprite goldStack;
    public Sprite goldPile;

    public override void Initialize()
    {
        base.Initialize();
        UpdateImage();
        //goldList.Add(this);
        //Pathfinding.GenerateToGoldMap();
    }

    public void OnDestroy()
    {
        //goldList.Remove(this);
    }

    private void OnValidate()
    {
        //Initialize();
    }

    public override void UpdateImage()
    {
        if (stackSize > 500)
        {
            itemSprite = goldPile;
        }
        else if (stackSize > 100)
        {
            itemSprite = goldStack;
        }
        else if (stackSize == 100)
        {
            itemSprite = goldSingle;
        }
        else if (stackSize > 50)
        {
            itemSprite = silverPile;
        }
        else if (stackSize > 10)
        {
            itemSprite = silverStack;
        }
        else if (stackSize == 10)
        {
            itemSprite = silverSingle;
        }
        else if (stackSize > 5)
        {
            itemSprite = copperPile;
        }
        else if (stackSize > 1)
        {
            itemSprite = copperStack;
        }
        else
        {
            itemSprite = copperSingle;
        }

        mySpriteRenderer.sprite = itemSprite;
    }

    public override void Obtained()
    {
        base.Obtained();
        //Pathfinding.GenerateToGoldMap();
    }

    public override void Dropped()
    {
        base.Dropped();
        //Pathfinding.GenerateToGoldMap();
    }

}
