using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace FluffyGameDev.Dialogue.Edition
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Edit graph", GUILayout.Height(40)))
            {
                Dialogue dialogue = serializedObject.targetObject as Dialogue;
                NodeEditorWindow w = NodeEditorWindow.Open(dialogue.EditorGraph);
                w.Home();
            }

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
