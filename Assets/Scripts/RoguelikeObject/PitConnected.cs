using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GameController;

public class PitConnected : Floor
{
    [Header("Pit")]
    public Sprite topLeft;
    public Sprite topRight;
    public Sprite top;
    public Sprite left;
    public Sprite right;
    public Sprite topCap;
    public Sprite vertical;
    public Sprite surrounded;
    public Sprite curveLeft;
    public Sprite curveUp;
    public Sprite curveRight;



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
                    bool isPresent = GetFloorController().GetFloor(new Vector2Int(checkPosX, checkPosY)) is PitConnected;
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

        if (touchingCount == 4)
        {
            return surrounded;
        }
        else if (touchingCount == 3)
        {
            // left empty
            if (nearbyList[1] == 0)
            {
                return left;
            }
            // down empty
            else if (nearbyList[3] == 0)
            {
                return surrounded;
            }
            // up empty
            else if (nearbyList[5] == 0)
            {
                return top;
            }
            // right empty
            else if (nearbyList[7] == 0)
            {
                return right;
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
                    return top;
                }
                // up
                else if (nearbyList[5] == 1)
                {
                    return right;
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
                    return left;
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
        else if (touchingCount == 1)
        {
            // left
            if (nearbyList[1] == 1)
            {
                return topRight;
            }
            // down
            else if (nearbyList[3] == 1)
            {
                return topCap;
            }
            // up
            else if (nearbyList[5] == 1)
            {
                return vertical;
            }
            // right
            else if (nearbyList[7] == 1)
            {
                return topLeft;
            }
        }
        else if (touchingCount == 0)
        {
            return surrounded;
        }
        return null;
    }
}
