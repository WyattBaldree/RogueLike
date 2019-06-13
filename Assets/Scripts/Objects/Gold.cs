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

    public override void Initialize()
    {
        base.Initialize();
        UpdateImage();
        goldList.Add(this);
        Pathfinding.GenerateToGoldMap();
    }

    public void OnDestroy()
    {
        goldList.Remove(this);
    }

    private void OnValidate()
    {
        //Initialize();
    }

    public override void UpdateImage()
    {
        if (stackSize > 500)
        {
            mySpriteController.SetSprite(1);
        }
        else if (stackSize > 100)
        {
            mySpriteController.SetSprite(3);
        }
        else if (stackSize == 100)
        {
            mySpriteController.SetSprite(5);
        }
        else if (stackSize > 50)
        {
            mySpriteController.SetSprite(7);
        }
        else if (stackSize > 10)
        {
            mySpriteController.SetSprite(9);
        }
        else if (stackSize == 10)
        {
            mySpriteController.SetSprite(11);
        }
        else if (stackSize > 5)
        {
            mySpriteController.SetSprite(0);
        }
        else if (stackSize > 1)
        {
            mySpriteController.SetSprite(2);
        }
        else
        {
            mySpriteController.SetSprite(4);
        }
    }

    public override void Obtained(Inventory newInventory)
    {
        base.Obtained(newInventory);
        Pathfinding.GenerateToGoldMap();
    }

    public override void Dropped()
    {
        base.Dropped();
        Pathfinding.GenerateToGoldMap();
    }

}
