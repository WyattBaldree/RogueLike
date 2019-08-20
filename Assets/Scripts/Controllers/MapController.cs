using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using System;
using static GameController;

public class MapController : MonoBehaviour
{
    //////////////////////////////////////////GUI
    /*private void OnGUI()
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
    }*/

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


