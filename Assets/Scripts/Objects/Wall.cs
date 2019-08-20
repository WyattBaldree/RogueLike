using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wall : Object
{
    public SpriteRenderer mySpriteRenderer;
    MapController mapController;

    private string debugString = "";

    public Sprite topLeft;
    public Sprite topRight;
    public Sprite bottomLeft;
    public Sprite bottomRight;
    public Sprite horizontal;
    public Sprite vertical;
    public Sprite bottomCap;
    public Sprite surrounded;
    public Sprite fourWay;
    public Sprite tLeftRightDown;
    public Sprite tLeftRightUp;
    public Sprite tRightDownUp;
    public Sprite tLeftDownUp;


    //Does this block characters?
    public bool solid = true;

    public bool influenceFloors = true;

    public float cost = float.MaxValue;

    public virtual void Initialize()
    {
        //mapController = GameController.mapC;
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
            mySpriteRenderer.sprite = surrounded;
        }
        else if (nearbyCount == 7)
        {
            if (nearbyList[0] == 0)
            {
                mySpriteRenderer.sprite = topRight;
            }
            else if (nearbyList[1] == 0)
            {
                mySpriteRenderer.sprite = vertical;
            }
            else if (nearbyList[2] == 0)
            {
                mySpriteRenderer.sprite = bottomRight;
            }
            else if (nearbyList[3] == 0)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            else if (nearbyList[5] == 0)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            else if (nearbyList[6] == 0)
            {
                mySpriteRenderer.sprite = topLeft;
            }
            else if (nearbyList[7] == 0)
            {
                mySpriteRenderer.sprite = vertical;
            }
            else if (nearbyList[8] == 0)
            {
                mySpriteRenderer.sprite = bottomLeft;
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
                    mySpriteRenderer.sprite = vertical;
                }
                // and down 
                else if (nearbyList[3] == 0)
                {
                    mySpriteRenderer.sprite = bottomLeft;
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    mySpriteRenderer.sprite = topLeft;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    mySpriteRenderer.sprite = tRightDownUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = tRightDownUp;
                }
            }
            // right
            else if (nearbyList[7] == 0)
            {
                // and down 
                if (nearbyList[3] == 0)
                {
                    mySpriteRenderer.sprite = bottomRight;
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    mySpriteRenderer.sprite = topRight;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    mySpriteRenderer.sprite = tLeftDownUp;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = tLeftDownUp;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = vertical;
                }
            }
            // down
            else if (nearbyList[3] == 0)
            {
                //and up
                if (nearbyList[5] == 0)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightUp;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
            }

            // up
            else if (nearbyList[5] == 0)
            {
                //and ul
                if (nearbyList[2] == 0)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightDown;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightDown;
                }
            }
            // ul
            else if (nearbyList[2] == 0)
            {
                //and ur
                if (nearbyList[8] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = tLeftDownUp;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = fourWay;
                }
            }
            // ur
            else if (nearbyList[8] == 0)
            {
                //and dl
                if (nearbyList[0] == 0)
                {
                    mySpriteRenderer.sprite = fourWay;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = tRightDownUp;
                }
            }
            // dl
            else if (nearbyList[0] == 0)
            {
                //and dr
                if (nearbyList[6] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightDown;
                }
            }

        }
        else if (nearbyCount == 5)
        {
            //rows
            // up ul ur
            if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[8] == 0)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // down dl dr
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[6] == 0)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // left dl ul
            else if (nearbyList[1] == 0 && nearbyList[0] == 0 && nearbyList[2] == 0)
            {
                mySpriteRenderer.sprite = vertical;
            }
            // right dr ur
            else if (nearbyList[7] == 0 && nearbyList[6] == 0 && nearbyList[8] == 0)
            {
                mySpriteRenderer.sprite = vertical;
            }
            //corners
            // up ul left
            else if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[1] == 0)
            {
                mySpriteRenderer.sprite = topLeft;
            }
            // down dl left
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[1] == 0)
            {
                mySpriteRenderer.sprite = bottomLeft;
            }
            // up ur right
            else if (nearbyList[5] == 0 && nearbyList[8] == 0 && nearbyList[7] == 0)
            {
                mySpriteRenderer.sprite = topRight;
            }
            // down dr right
            else if (nearbyList[3] == 0 && nearbyList[6] == 0 && nearbyList[7] == 0)
            {
                mySpriteRenderer.sprite = bottomRight;
            }
            //cross
            // down up right left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1 && nearbyList[7] == 1)
            {
                mySpriteRenderer.sprite = fourWay;
            }

            // Ts
            // down up left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = tLeftDownUp;
            }
            // down up right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = tRightDownUp;
            }
            // left up right
            else if (nearbyList[7] == 1 && nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = tLeftRightUp;
            }
            // left down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[1] == 1)
            {
                mySpriteRenderer.sprite = tLeftRightDown;
            }

            // Ls
            // up left
            else if (nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = bottomRight;
            }
            // up right
            else if (nearbyList[7] == 1 && nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = bottomLeft;
            }
            // down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1)
            {
                mySpriteRenderer.sprite = topLeft;
            }
            // down left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1)
            {
                mySpriteRenderer.sprite = topRight;
            }

            // Is
            // left
            else if (nearbyList[1] == 1)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // right
            else if (nearbyList[7] == 1)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // down
            else if (nearbyList[3] == 1)
            {
                mySpriteRenderer.sprite = vertical;
            }
            // up
            else if (nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = bottomCap;
            }
        }
        else if (nearbyCount == 4)
        {
            //touchingCount is 4
            if (touchingCount == 4)
            {
                mySpriteRenderer.sprite = fourWay;
            }
            //touchingCount is 3
            else if (touchingCount == 3)
            {
                // left empty
                if (nearbyList[1] == 0)
                {
                    mySpriteRenderer.sprite = tRightDownUp;
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightUp;
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightDown;
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    mySpriteRenderer.sprite = tLeftDownUp;
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
                        mySpriteRenderer.sprite = horizontal;
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        mySpriteRenderer.sprite = bottomRight;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = topRight;
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        mySpriteRenderer.sprite = bottomLeft;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = topLeft;
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = vertical;
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    mySpriteRenderer.sprite = bottomCap;
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                mySpriteRenderer.sprite = tLeftRightUp;
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
                    mySpriteRenderer.sprite = tRightDownUp;
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightUp;
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    mySpriteRenderer.sprite = tLeftRightDown;
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    mySpriteRenderer.sprite = tLeftDownUp;
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
                        mySpriteRenderer.sprite = horizontal;
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        mySpriteRenderer.sprite = bottomRight;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = topRight;
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        mySpriteRenderer.sprite = bottomLeft;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = topLeft;
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        mySpriteRenderer.sprite = vertical;
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    mySpriteRenderer.sprite = bottomCap;
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    mySpriteRenderer.sprite = horizontal;
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                mySpriteRenderer.sprite = tLeftRightUp;
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
                    mySpriteRenderer.sprite = horizontal;
                }
                // down 
                else if (nearbyList[3] == 1)
                {
                    mySpriteRenderer.sprite = topRight;
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    mySpriteRenderer.sprite = bottomRight;
                }
                else
                {
                    mySpriteRenderer.sprite = horizontal;
                }
            }
            // right
            else if (nearbyList[7] == 1)
            {
                // down 
                if (nearbyList[3] == 1)
                {
                    mySpriteRenderer.sprite = topLeft;
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    mySpriteRenderer.sprite = bottomLeft;
                }
                else
                {
                    mySpriteRenderer.sprite = horizontal;
                }
            }
            // down
            else if (nearbyList[3] == 1)
            {
                //up
                if (nearbyList[5] == 1)
                {
                    mySpriteRenderer.sprite = vertical;
                }
                else
                {
                    mySpriteRenderer.sprite = vertical;
                }
            }
            // up
            else if (nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = bottomCap;
            }
            else
            {
                mySpriteRenderer.sprite = tLeftRightUp;
            }
        }
        else if (nearbyCount == 1)
        {
            // left
            if (nearbyList[1] == 1)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // right
            else if (nearbyList[7] == 1)
            {
                mySpriteRenderer.sprite = horizontal;
            }
            // down 
            else if (nearbyList[3] == 1)
            {
                mySpriteRenderer.sprite = vertical;
            }
            //up
            else if (nearbyList[5] == 1)
            {
                mySpriteRenderer.sprite = bottomCap;
            }
            else
            {
                mySpriteRenderer.sprite = tLeftRightUp;
            }
        }
        else if (nearbyCount == 0)
        {
            mySpriteRenderer.sprite = tLeftRightUp;
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
