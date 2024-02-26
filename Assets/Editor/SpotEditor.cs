using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpotController))]
class SpotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpotController spot = target as SpotController;

        if (GUILayout.Button("Update Spot"))
        {
            spot?.UpdateSpot();

            EditorUtility.SetDirty(spot);
            AssetDatabase.SaveAssets();
        }
        
        base.OnInspectorGUI();
    }
}