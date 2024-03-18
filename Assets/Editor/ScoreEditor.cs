using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreManager))]
class ScoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScoreManager score = target as ScoreManager;

        if (GUILayout.Button("Reset player prefs"))
        {
            PlayerPrefs.DeleteAll();

            EditorUtility.SetDirty(score);
            AssetDatabase.SaveAssets();
        }

        base.OnInspectorGUI();
    }
}