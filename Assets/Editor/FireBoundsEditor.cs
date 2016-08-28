using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FireBounds))]
public class FireBoundsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var fireBounds = (FireBounds)target;
        if (GUILayout.Button("Recalculate Bounds"))
        {
            fireBounds.RecalculateBounds();
        }
    }
}
