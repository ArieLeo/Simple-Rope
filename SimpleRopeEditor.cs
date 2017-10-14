#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleRope))]
public class SimpleRopeEditor : Editor {

    public override void OnInspectorGUI()
    {
        SimpleRope RopeScript = (SimpleRope)target;
        if (GUILayout.Button("Generate New Rope"))
        {
            RopeScript.GenerateRope();
        }
        if (GUILayout.Button("Refresh Rope"))
        {
            RopeScript.RefreshRope();
        }
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck()) { RopeScript.RefreshRope(); }
             
    }

}
#endif
