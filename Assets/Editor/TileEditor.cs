using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileController))]
class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TileController tile = target as TileController;

        if (GUILayout.Button("Add Wall"))
        {
            tile?.AddWallOnTile();

            EditorUtility.SetDirty(tile);
            AssetDatabase.SaveAssets();
        }
        
        if (GUILayout.Button("Update Tile"))
        {
            tile?.UpdateTile();

            EditorUtility.SetDirty(tile);
            AssetDatabase.SaveAssets();
        }
        
        base.OnInspectorGUI();
    }
}