using SaveSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Save))]
public class SaveEditor : Editor
{
    private Save _save;

    private void OnEnable()
    {
        _save = (Save)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUI.contentColor = Color.red;
        if (GUILayout.Button("Remove save file"))
        {
            _save.RemoveSaveFile();
        }
        GUI.contentColor = Color.white;
    }
}
