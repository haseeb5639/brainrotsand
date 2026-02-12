using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdsConfigLoader))]
public class AdsConfigLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AdsConfigLoader adsConfigLoader = (AdsConfigLoader)target;

        if (GUILayout.Button("Set Field"))
        {
            adsConfigLoader.LoadIds();
        }

        DrawDefaultInspector();
    }
}