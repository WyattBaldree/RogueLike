using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MapController : MonoBehaviour
{
    //The various walls that can be placed
    public Wall[] wallArray;

    //The various walls that can be placed
    public Floor[] floorArray;

    //The darkness prefab
    public Darkness darknessObj;

    public Door doorSourceObject;

    public Chest chestSourceObject;

    //The pit prefab
    public Pit pitObj;

    //These are the 2d arrays that hold various level information.
    [System.NonSerialized]
    public Wall[,] wallMapArray;
    [System.NonSerialized]
    public Floor[,] floorMapArray;
    [System.NonSerialized]
    public Darkness[,] darknessMapArray;
    [System.NonSerialized]
    public Pit[,] pitMapArray;

    float animationSpeed = 0.1f;
    float animationCount = 0.0f;

    // Update is called once per frame
    void Update()
    {
        animationCount += animationSpeed * Time.deltaTime;
    }


    public void Initialize(Camera cam)
    {

        // create all floors
        floorMapArray = new Floor[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                Floor floor = (Floor)Instantiate(floorArray[0], new Vector3(i, j), Quaternion.identity, transform);
                floorMapArray[i, j] = floor;
            }
        }

        //create all walls
        wallMapArray = new Wall[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                Wall wall = (Wall)Instantiate(wallArray[0], new Vector3(i, j), Quaternion.identity, transform);
                wallMapArray[i, j] = wall;
                wall.Initialize();
            }
        }

        //cut out rooms temporary
        for (int i = 2; i < 6; i++)
        {
            for (int j = 2; j < 4; j++)
            {
                Destroy(wallMapArray[i, j].gameObject);
                wallMapArray[i, j] = null;
            }
        }

        for (int i = 5; i < 12; i++)
        {
            for (int j = 5; j < 9; j++)
            {
                Destroy(wallMapArray[i, j].gameObject);
                wallMapArray[i, j] = null;
            }
        }

        for (int i = 3; i < 10; i++)
        {
            for (int j = 14; j < 17; j++)
            {
                Destroy(wallMapArray[i, j].gameObject);
                wallMapArray[i, j] = null;
            }
        }

        for (int i = 7; i < 8; i++)
        {
            for (int j = 7; j < 15; j++)
            {
                if (wallMapArray[i, j])
                {
                    Destroy(wallMapArray[i, j].gameObject);
                    wallMapArray[i, j] = null;
                }
            }
        }

        for (int i = 6; i < 14; i++)
        {
            for (int j = 6; j < 14; j++)
            {
                if (wallMapArray[i, j])
                {
                    Destroy(wallMapArray[i, j].gameObject);
                    wallMapArray[i, j] = null;
                }
            }
        }

        Destroy(wallMapArray[5, 4].gameObject);
        wallMapArray[5, 4] = null;

        Door door = (Door)Instantiate(doorSourceObject, new Vector3(5, 4), Quaternion.identity, transform);
        wallMapArray[5, 4] = door;
        door.Initialize();

        //Destroy(wallMapArray[3, 2].gameObject);
        wallMapArray[3, 2] = null;

        Chest chest = (Chest)Instantiate(chestSourceObject, new Vector3(3, 2), Quaternion.identity, transform);
        wallMapArray[3, 2] = chest;
        chest.Initialize();



        //create all pits
        pitMapArray = new Pit[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                if (i > 7 && i < 14 && j > 7 && j < 14)
                {
                    Pit pit = (Pit)Instantiate(pitObj, new Vector3(i, j), Quaternion.identity, transform);
                    pitMapArray[i, j] = pit;
                }
                else
                {
                    pitMapArray[i, j] = null;
                }

            }
        }

        //create all darkness
        darknessMapArray = new Darkness[(int)GameController.gameC.ScreenResInUnits.x, (int)(GameController.gameC.ScreenResInUnits.y)];

        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                Darkness darkness = (Darkness)Instantiate(darknessObj, new Vector3(i, j), Quaternion.identity, transform);
                darknessMapArray[i, j] = darkness;

            }
        }

        
        

        //Everything has been place, refresh everything so it looks right
        foreach (Wall wall in wallMapArray)
        {
            if (wall != null)
            {
                wall.RefreshSprite();
            }
        }
        foreach (Floor floor in floorMapArray)
        {
            if (floor != null)
            {
                floor.RefreshSprite();
            }
        }

        foreach (Darkness darkness in darknessMapArray)
        {
            if (darkness != null)
            {
                darkness.RefreshSprite();
            }
        }

        foreach (Pit pit in pitMapArray)
        {
            if (pit != null)
            {
                pit.RefreshSprite();
            }
        }
    }


    public Wall GetWall(int x, int y)
    {
        if(wallMapArray[x, y])
        {
            return wallMapArray[x, y];
        }
        return null;
    }

    public Pit GetPit(int x, int y)
    {
        if (pitMapArray[x, y])
        {
            return pitMapArray[x, y];
        }
        return null;
    }

    public Floor GetFloor(int x, int y)
    {
        if (floorMapArray[x, y])
        {
            return floorMapArray[x, y];
        }
        return null;
    }

    public Darkness GetDarkness(int x, int y)
    {
        if (darknessMapArray[x, y])
        {
            return darknessMapArray[x, y];
        }
        return null;
    }

    public bool IsEmpty(int x, int y)
    {
        if((!GetWall(x, y) || !GetWall(x, y).solid) && !GetPit(x, y))
            return true;
        return false;
    }
    
    //////////////////////////////////////////GUI
    private void OnGUI()
    {
        if (!GameController.gameC.debug) return;
        for (int i = 0; i < GameController.gameC.ScreenResInUnits.x; i++)
        {
            for (int j = 0; j < GameController.gameC.ScreenResInUnits.y; j++)
            {
                if(Pathfinding.toPlayerMap != null)
                {
                    Color c = Color.HSVToRGB(((Pathfinding.toPlayerMap[i, j].distance) + 40) / 80, .7f, .5f);
                    c.a = .8f;

                    GUIDrawRect(new Rect(i, j, 1, 1), c);
                    //GUIDrawRect(new Rect(i, j, 1, 1), new Color(0.07f * Pathfinding.toPlayerMap[i, j].distance, 1.5f - 0.07f * Pathfinding.toPlayerMap[i, j].distance, 0.0f, 0.7f));

                    if (smallFontStyle == null)
                    {
                        smallFontStyle = new GUIStyle();
                    }

                    smallFontStyle.fontSize = 9;

                    float distance = (float)Pathfinding.toPlayerMap[i, j].distance;
                    if (distance == float.MaxValue)
                    {
                        Handles.Label(new Vector3(i + 0.1f, j + 0.9f, 0), "###", smallFontStyle);
                    }
                    else
                    {
                        Handles.Label(new Vector3(i + 0.1f, j + 0.9f, 0), (distance).ToString("F2"), smallFontStyle);
                    }
                }
            }
        }
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    private static GUIStyle smallFontStyle;

    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }
        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }
    
}


