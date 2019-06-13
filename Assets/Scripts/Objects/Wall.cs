using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wall : Object
{
    SpriteController spriteController;
    MapController mapController;

    private string debugString = "";

    //Does this block characters?
    public bool solid = true;

    public bool influenceFloors = true;

    public float cost = float.MaxValue;

    public virtual void Initialize()
    {
        mapController = GameController.mapC;

        spriteController = GetComponent<SpriteController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnPush()
    {

    }

    public virtual void OnInteract()
    {

    }

    public virtual void RefreshSprite()
    {
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
                if(i == 0 && j == 0)
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
                    bool isPresent = mapController.wallMapArray[checkPosX, checkPosY] != null;
                    if (isPresent) {
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

        if (nearbyCount == 8)
        {
            spriteController.SetSprite(9);
        }
        else if (nearbyCount == 7)
        {
            if (nearbyList[0] == 0)
            {
                spriteController.SetSprite(6);
            }
            else if (nearbyList[1] == 0)
            {
                spriteController.SetSprite(1);
            }
            else if (nearbyList[2] == 0)
            {
                spriteController.SetSprite(8);
            }
            else if (nearbyList[3] == 0)
            {
                spriteController.SetSprite(3);
            }
            else if (nearbyList[5] == 0)
            {
                spriteController.SetSprite(3);
            }
            else if (nearbyList[6] == 0)
            {
                spriteController.SetSprite(0);
            }
            else if (nearbyList[7] == 0)
            {
                spriteController.SetSprite(1);
            }
            else if (nearbyList[8] == 0)
            {
                spriteController.SetSprite(2);
            }
        }
        else if (nearbyCount == 6)
        {
            // left
            if (nearbyList[1] == 0)
            {
                // and right
                if (nearbyList[7] == 0)
                {
                    spriteController.SetSprite(1);
                }
                // and down 
                else if (nearbyList[3] == 0)
                {
                    spriteController.SetSprite(2);
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    spriteController.SetSprite(0);
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    spriteController.SetSprite(1);
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    spriteController.SetSprite(10);
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(1);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(10);
                }
            }
            // right
            else if (nearbyList[7] == 0)
            {
                // and down 
                if (nearbyList[3] == 0)
                {
                    spriteController.SetSprite(8);
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    spriteController.SetSprite(6);
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    spriteController.SetSprite(16);
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    spriteController.SetSprite(1);
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(16);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(1);
                }
            }
            // down
            else if (nearbyList[3] == 0)
            {
                //and up
                if (nearbyList[5] == 0)
                {
                    spriteController.SetSprite(3);
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    spriteController.SetSprite(14);
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    spriteController.SetSprite(14);
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(3);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(3);
                }
            }

            // up
            else if (nearbyList[5] == 0)
            {
                //and ul
                if (nearbyList[2] == 0)
                {
                    spriteController.SetSprite(3);
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    spriteController.SetSprite(3);
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(12);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(12);
                }
            }
            // ul
            else if (nearbyList[2] == 0)
            {
                //and ur
                if (nearbyList[8] == 0)
                {
                    spriteController.SetSprite(14);
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(16);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(13);
                }
            }
            // ur
            else if (nearbyList[8] == 0)
            {
                //and dl
                if (nearbyList[0] == 0)
                {
                    spriteController.SetSprite(13);
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(10);
                }
            }
            // dl
            else if (nearbyList[0] == 0)
            {
                //and dr
                if (nearbyList[6] == 0)
                {
                    spriteController.SetSprite(12);
                }
            }

        }
        else if (nearbyCount == 5)
        {
            //rows
            // up ul ur
            if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[8] == 0)
            {
                spriteController.SetSprite(3);
            }
            // down dl dr
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[6] == 0)
            {
                spriteController.SetSprite(3);
            }
            // left dl ul
            else if (nearbyList[1] == 0 && nearbyList[0] == 0 && nearbyList[2] == 0)
            {
                spriteController.SetSprite(1);
            }
            // right dr ur
            else if (nearbyList[7] == 0 && nearbyList[6] == 0 && nearbyList[8] == 0)
            {
                spriteController.SetSprite(1);
            }
            //corners
            // up ul left
            else if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[1] == 0)
            {
                spriteController.SetSprite(0);
            }
            // down dl left
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[1] == 0)
            {
                spriteController.SetSprite(2);
            }
            // up ur right
            else if (nearbyList[5] == 0 && nearbyList[8] == 0 && nearbyList[7] == 0)
            {
                spriteController.SetSprite(6);
            }
            // down dr right
            else if (nearbyList[3] == 0 && nearbyList[6] == 0 && nearbyList[7] == 0)
            {
                spriteController.SetSprite(8);
            }
            //cross
            // down up right left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1 && nearbyList[7] == 1)
            {
                spriteController.SetSprite(13);
            }

            // Ts
            // down up left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                spriteController.SetSprite(16);
            }
            // down up right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                spriteController.SetSprite(10);
            }
            // left up right
            else if (nearbyList[7] == 1 && nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                spriteController.SetSprite(14);
            }
            // left down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[1] == 1)
            {
                spriteController.SetSprite(12);
            }

            // Ls
            // up left
            else if (nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                spriteController.SetSprite(8);
            }
            // up right
            else if (nearbyList[7] == 1 && nearbyList[5] == 1)
            {
                spriteController.SetSprite(2);
            }
            // down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1)
            {
                spriteController.SetSprite(0);
            }
            // down left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1)
            {
                spriteController.SetSprite(6);
            }

            // Is
            // left
            else if (nearbyList[1] == 1)
            {
                spriteController.SetSprite(3);
            }
            // right
            else if (nearbyList[7] == 1)
            {
                spriteController.SetSprite(3);
            }
            // down
            else if (nearbyList[3] == 1)
            {
                spriteController.SetSprite(1);
            }
            // up
            else if (nearbyList[5] == 1)
            {
                spriteController.SetSprite(4);
            }
        }
        else if (nearbyCount == 4)
        {
            //touchingCount is 4
            if (touchingCount == 4)
            {
                spriteController.SetSprite(13);
            }
            //touchingCount is 3
            else if (touchingCount == 3)
            {
                // left empty
                if (nearbyList[1] == 0)
                {
                    spriteController.SetSprite(10);
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    spriteController.SetSprite(14);
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    spriteController.SetSprite(12);
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    spriteController.SetSprite(16);
                }
            }
            //touchingCount is 2
            else if (touchingCount == 2)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    // right
                    if (nearbyList[7] == 1)
                    {
                        spriteController.SetSprite(3);
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        spriteController.SetSprite(8);
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(6);
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        spriteController.SetSprite(2);
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(0);
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(1);
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    spriteController.SetSprite(3);
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(1);
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(4);
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    spriteController.SetSprite(3);
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                spriteController.SetSprite(14);
            }
        }
        else if (nearbyCount == 3)
        {
            //touchingCount is 3
            if (touchingCount == 3)
            {
                // left empty
                if (nearbyList[1] == 0)
                {
                    spriteController.SetSprite(10);
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    spriteController.SetSprite(14);
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    spriteController.SetSprite(12);
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    spriteController.SetSprite(16);
                }
            }
            //touchingCount is 2
            else if (touchingCount == 2)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    // right
                    if (nearbyList[7] == 1)
                    {
                        spriteController.SetSprite(3);
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        spriteController.SetSprite(8);
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(6);
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        spriteController.SetSprite(2);
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(0);
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        spriteController.SetSprite(1);
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    spriteController.SetSprite(3);
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(1);
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(4);
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    spriteController.SetSprite(3);
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                spriteController.SetSprite(14);
            }
        }
        else if (nearbyCount == 2)
        {
            // left
            if (nearbyList[1] == 1)
            {
                // right
                if (nearbyList[7] == 1)
                {
                    spriteController.SetSprite(3);
                }
                // down 
                else if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(6);
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(8);
                }
                else
                {
                    spriteController.SetSprite(3);
                }
            }
            // right
            else if (nearbyList[7] == 1)
            {
                // down 
                if (nearbyList[3] == 1)
                {
                    spriteController.SetSprite(0);
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(2);
                }
                else
                {
                    spriteController.SetSprite(3);
                }
            }
            // down
            else if (nearbyList[3] == 1)
            {
                //up
                if (nearbyList[5] == 1)
                {
                    spriteController.SetSprite(1);
                }
                else
                {
                    spriteController.SetSprite(1);
                }
            }
            // up
            else if (nearbyList[5] == 1)
            {
                spriteController.SetSprite(4);
            }
            else
            {
                spriteController.SetSprite(14);
            }
        }
        else if (nearbyCount == 1)
        {
            // left
            if (nearbyList[1] == 1)
            {
                spriteController.SetSprite(3);
            }
            // right
            else if (nearbyList[7] == 1)
            {
                spriteController.SetSprite(3);
            }
            // down 
            else if (nearbyList[3] == 1)
            {
                spriteController.SetSprite(1);
            }
            //up
            else if (nearbyList[5] == 1)
            {
                spriteController.SetSprite(4);
            }
            else
            {
                spriteController.SetSprite(14);
            }
        }
        else if (nearbyCount == 0)
        {
            spriteController.SetSprite(14);
        }
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position, debugString);
    }

    void OnMouseOver()
    {
        return;
        mapController.wallMapArray[(int)transform.position.x, (int)transform.position.y] = null;


        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int checkPosX = (int)transform.position.x + i;
                int checkPosY = (int)transform.position.y + j;

                if (checkPosX >= 0 &&
                    checkPosY >= 0 &&
                    checkPosX < mapController.wallMapArray.GetLength(0) &&
                    checkPosY < mapController.wallMapArray.GetLength(1))

                {
                    if (mapController.wallMapArray[checkPosX, checkPosY])
                        mapController.wallMapArray[checkPosX, checkPosY].RefreshSprite();

                    if (mapController.floorMapArray[checkPosX, checkPosY])
                        mapController.floorMapArray[checkPosX, checkPosY].RefreshSprite();

                    if (mapController.pitMapArray[checkPosX, checkPosY])
                        mapController.pitMapArray[checkPosX, checkPosY].RefreshSprite();

                    if (mapController.darknessMapArray[checkPosX, checkPosY])
                        mapController.darknessMapArray[checkPosX, checkPosY].RefreshSprite();
                }
            }
        }

        Destroy(gameObject);

    }
}
