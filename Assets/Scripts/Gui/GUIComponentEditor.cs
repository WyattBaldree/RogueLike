using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GUIComponent), true)]
public class ObjectBuilderEditor : Editor
{
    SerializedProperty guicomp;

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();

        GUIComponent myScript = (GUIComponent)target;


        if (GUILayout.Button("UpdateGUI"))
        {
            myScript.UpdateGUI();
        }
    }
}
