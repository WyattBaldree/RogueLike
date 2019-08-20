using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class WallConnected : Wall2
{
    [Header("Wall Connected")]
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

    public override Sprite GetWorldSprite()
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
                if (i == 0 && j == 0)
                {
                    nearbyList.Add(1);
                }
                else if (checkPosX < 0 ||
                    checkPosY < 0 ||
                    checkPosX >= GetGameController().ScreenResInUnits.x ||
                    checkPosY >= GetGameController().ScreenResInUnits.y)

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
                    bool isPresent = GetWallController().GetWall(new Vector2Int(checkPosX, checkPosY)) != null;
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

        if (nearbyCount == 8)
        {
            return surrounded;
        }
        else if (nearbyCount == 7)
        {
            if (nearbyList[0] == 0)
            {
                return topRight;
            }
            else if (nearbyList[1] == 0)
            {
                return vertical;
            }
            else if (nearbyList[2] == 0)
            {
                return bottomRight;
            }
            else if (nearbyList[3] == 0)
            {
                return horizontal;
            }
            else if (nearbyList[5] == 0)
            {
                return horizontal;
            }
            else if (nearbyList[6] == 0)
            {
                return topLeft;
            }
            else if (nearbyList[7] == 0)
            {
                return vertical;
            }
            else if (nearbyList[8] == 0)
            {
                return bottomLeft;
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
                    return vertical;
                }
                // and down 
                else if (nearbyList[3] == 0)
                {
                    return bottomLeft;
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    return topLeft;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    return vertical;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    return tRightDownUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    return vertical;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return tRightDownUp;
                }
            }
            // right
            else if (nearbyList[7] == 0)
            {
                // and down 
                if (nearbyList[3] == 0)
                {
                    return bottomRight;
                }
                //and up
                else if (nearbyList[5] == 0)
                {
                    return topRight;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    return tLeftDownUp;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    return vertical;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    return tLeftDownUp;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return vertical;
                }
            }
            // down
            else if (nearbyList[3] == 0)
            {
                //and up
                if (nearbyList[5] == 0)
                {
                    return horizontal;
                }
                //and ul
                else if (nearbyList[2] == 0)
                {
                    return tLeftRightUp;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    return tLeftRightUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    return horizontal;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return horizontal;
                }
            }

            // up
            else if (nearbyList[5] == 0)
            {
                //and ul
                if (nearbyList[2] == 0)
                {
                    return horizontal;
                }
                //and ur
                else if (nearbyList[8] == 0)
                {
                    return horizontal;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    return tLeftRightDown;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return tLeftRightDown;
                }
            }
            // ul
            else if (nearbyList[2] == 0)
            {
                //and ur
                if (nearbyList[8] == 0)
                {
                    return tLeftRightUp;
                }
                //and dl
                else if (nearbyList[0] == 0)
                {
                    return tLeftDownUp;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return fourWay;
                }
            }
            // ur
            else if (nearbyList[8] == 0)
            {
                //and dl
                if (nearbyList[0] == 0)
                {
                    return fourWay;
                }
                //and dr
                else if (nearbyList[6] == 0)
                {
                    return tRightDownUp;
                }
            }
            // dl
            else if (nearbyList[0] == 0)
            {
                //and dr
                if (nearbyList[6] == 0)
                {
                    return tLeftRightDown;
                }
            }

        }
        else if (nearbyCount == 5)
        {
            //rows
            // up ul ur
            if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[8] == 0)
            {
                return horizontal;
            }
            // down dl dr
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[6] == 0)
            {
                return horizontal;
            }
            // left dl ul
            else if (nearbyList[1] == 0 && nearbyList[0] == 0 && nearbyList[2] == 0)
            {
                return vertical;
            }
            // right dr ur
            else if (nearbyList[7] == 0 && nearbyList[6] == 0 && nearbyList[8] == 0)
            {
                return vertical;
            }
            //corners
            // up ul left
            else if (nearbyList[5] == 0 && nearbyList[2] == 0 && nearbyList[1] == 0)
            {
                return topLeft;
            }
            // down dl left
            else if (nearbyList[3] == 0 && nearbyList[0] == 0 && nearbyList[1] == 0)
            {
                return bottomLeft;
            }
            // up ur right
            else if (nearbyList[5] == 0 && nearbyList[8] == 0 && nearbyList[7] == 0)
            {
                return topRight;
            }
            // down dr right
            else if (nearbyList[3] == 0 && nearbyList[6] == 0 && nearbyList[7] == 0)
            {
                return bottomRight;
            }
            //cross
            // down up right left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1 && nearbyList[7] == 1)
            {
                return fourWay;
            }

            // Ts
            // down up left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                return tLeftDownUp;
            }
            // down up right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[5] == 1)
            {
                return tRightDownUp;
            }
            // left up right
            else if (nearbyList[7] == 1 && nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                return tLeftRightUp;
            }
            // left down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1 && nearbyList[1] == 1)
            {
                return tLeftRightDown;
            }

            // Ls
            // up left
            else if (nearbyList[1] == 1 && nearbyList[5] == 1)
            {
                return bottomRight;
            }
            // up right
            else if (nearbyList[7] == 1 && nearbyList[5] == 1)
            {
                return bottomLeft;
            }
            // down right
            else if (nearbyList[7] == 1 && nearbyList[3] == 1)
            {
                return topLeft;
            }
            // down left
            else if (nearbyList[1] == 1 && nearbyList[3] == 1)
            {
                return topRight;
            }

            // Is
            // left
            else if (nearbyList[1] == 1)
            {
                return horizontal;
            }
            // right
            else if (nearbyList[7] == 1)
            {
                return horizontal;
            }
            // down
            else if (nearbyList[3] == 1)
            {
                return vertical;
            }
            // up
            else if (nearbyList[5] == 1)
            {
                return bottomCap;
            }
        }
        else if (nearbyCount == 4)
        {
            //touchingCount is 4
            if (touchingCount == 4)
            {
                return fourWay;
            }
            //touchingCount is 3
            else if (touchingCount == 3)
            {
                // left empty
                if (nearbyList[1] == 0)
                {
                    return tRightDownUp;
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    return tLeftRightUp;
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    return tLeftRightDown;
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    return tLeftDownUp;
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
                        return horizontal;
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        return bottomRight;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        return topRight;
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        return bottomLeft;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        return topLeft;
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        return vertical;
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    return horizontal;
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    return vertical;
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    return bottomCap;
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    return horizontal;
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                return tLeftRightUp;
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
                    return tRightDownUp;
                }
                // down empty
                else if (nearbyList[3] == 0)
                {
                    return tLeftRightUp;
                }
                // up empty
                else if (nearbyList[5] == 0)
                {
                    return tLeftRightDown;
                }
                // right empty
                else if (nearbyList[7] == 0)
                {
                    return tLeftDownUp;
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
                        return horizontal;
                    }
                    // up
                    else if (nearbyList[5] == 1)
                    {
                        return bottomRight;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        return topRight;
                    }
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    // up
                    if (nearbyList[5] == 1)
                    {
                        return bottomLeft;
                    }
                    // down
                    else if (nearbyList[3] == 1)
                    {
                        return topLeft;
                    }
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    // down
                    if (nearbyList[3] == 1)
                    {
                        return vertical;
                    }
                }
            }
            //touchingCount is 1
            else if (touchingCount == 1)
            {
                // left
                if (nearbyList[1] == 1)
                {
                    return horizontal;
                }
                // down
                else if (nearbyList[3] == 1)
                {
                    return vertical;
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    return bottomCap;
                }
                // right
                else if (nearbyList[7] == 1)
                {
                    return horizontal;
                }
            }
            //touchingCount is 0
            else if (touchingCount == 0)
            {
                return tLeftRightUp;
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
                    return horizontal;
                }
                // down 
                else if (nearbyList[3] == 1)
                {
                    return topRight;
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    return bottomRight;
                }
                else
                {
                    return horizontal;
                }
            }
            // right
            else if (nearbyList[7] == 1)
            {
                // down 
                if (nearbyList[3] == 1)
                {
                    return topLeft;
                }
                //up
                else if (nearbyList[5] == 1)
                {
                    return bottomLeft;
                }
                else
                {
                    return horizontal;
                }
            }
            // down
            else if (nearbyList[3] == 1)
            {
                //up
                if (nearbyList[5] == 1)
                {
                    return vertical;
                }
                else
                {
                    return vertical;
                }
            }
            // up
            else if (nearbyList[5] == 1)
            {
                return bottomCap;
            }
            else
            {
                return tLeftRightUp;
            }
        }
        else if (nearbyCount == 1)
        {
            // left
            if (nearbyList[1] == 1)
            {
                return horizontal;
            }
            // right
            else if (nearbyList[7] == 1)
            {
                return horizontal;
            }
            // down 
            else if (nearbyList[3] == 1)
            {
                return vertical;
            }
            //up
            else if (nearbyList[5] == 1)
            {
                return bottomCap;
            }
            else
            {
                return tLeftRightUp;
            }
        }
        else if (nearbyCount == 0)
        {
            return tLeftRightUp;
        }
        return null;
    }
}
