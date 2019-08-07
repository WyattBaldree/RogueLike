using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoguelikeObject),true)]
public class RoguelikeObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoguelikeObject myScript = (RoguelikeObject)target;

        if (GUILayout.Button("Debug Button"))
        {
        }
    }
}
