using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Floor : Object
{
    SpriteController spriteController;
    MapController mapController;

    private string debugString = "";

    private bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!initialized) Initalize();
        //RefreshSprite();
    }

    private void Initalize()
    {
        mapController = GameController.mapC;

        spriteController = GetComponent<SpriteController>();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshSprite()
    {
        if (!initialized) Initalize();

        /* At the end of the double for loop, nearbyList will be filled in this order
         * 258
         * 147
         * 036
         */
        List<int> nearbyList = new List<int>();
        int nearbyCount = 0;
        int touchingCount = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int checkPosX = (int)transform.position.x + i;
                int checkPosY = (int)transform.position.y + j;
                if (i == 0 && j == 0)
                {
                    nearbyList.Add(1);
                }
                else if (checkPosX < 0 ||
                    checkPosY < 0 ||
                    checkPosX >= mapController.wallMapArray.GetLength(0) ||
                    checkPosY >= mapController.wallMapArray.GetLength(1))

                {
                    if (nearbyList.Count == 1 || nearbyList.Count == 3 || nearbyList.Count == 5 || nearbyList.Count == 7)
                    {
                        touchingCount++;
                    }

                    nearbyList.Add(1);
                    nearbyCount++;
                }
                else
                {
                    bool isPresent = mapController.wallMapArray[checkPosX, checkPosY] != null && mapController.wallMapArray[checkPosX, checkPosY].influenceFloors == true;
                    isPresent = isPresent || mapController.pitMapArray[checkPosX, checkPosY] != null;
                    if (isPresent)
                    {
                        if (nearbyList.Count == 1 || nearbyList.Count == 3 || nearbyList.Count == 5 || nearbyList.Count == 7)
                        {
                            touchingCount++;
                        }
                        nearbyList.Add(1);
                        nearbyCount++;
                    }
                    else
                    {
                        nearbyList.Add(0);
                    }
                }

            }
        }

        debugString = "" + touchingCount.ToString();

        if (touchingCount == 4)
        {
            spriteController.SetSprite(15);
        }
        else if (touchingCount == 3)
        {
            // left empty
            if (nearbyList[1] == 0)
            {
                spriteController.SetSprite(19);
            }
            // down empty
            else if (nearbyList[3] == 0)
            {
                spriteController.SetSprite(9);
            }
            // up empty
            else if (nearbyList[5] == 0)
            {
                spriteController.SetSprite(11);
            }
            // right empty
            else if (nearbyList[7] == 0)
            {
                spriteController.SetSprite(13);
            }
        }
        else if (touchingCount == 2)
        {
            // left
            if (nearbyList[1] == 1)
            {
                // right
                if (nearbyList[7] == 1)
                {
                    spriteController.SetSprite(10);
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(0);
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(2);
                }
            }
            // right
            else if (nearbyList[7] == 1)
            {
                // up
                if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(6);
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(8);
                }
            }
            // up
            else if (nearbyList[5] == 1)
            {
                // down
                if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(16);
                }
            }
        }
        else if (touchingCount == 1)
        {
            // left
            if (nearbyList[1] == 1)
            {
                spriteController.SetSprite(1);
            }
            // down
            else if (nearbyList[3] == 1)
            {
                spriteController.SetSprite(5);
            }
            // up
            else if (nearbyList[5] == 1)
            {
                spriteController.SetSprite(3);
            }
            // right
            else if (nearbyList[7] == 1)
            {
                spriteController.SetSprite(7);
            }
        }
        else if (touchingCount == 0)
        {
            spriteController.SetSprite(4);
        }
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, debugString);
    }
}
