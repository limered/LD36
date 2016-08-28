using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Fuel))]
public class FuelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var fuel = (Fuel)target;
        if (GUILayout.Button("Ignite"))
        {
            fuel.gameObject.AddComponent<IsBurning>();
        }
    }
}
