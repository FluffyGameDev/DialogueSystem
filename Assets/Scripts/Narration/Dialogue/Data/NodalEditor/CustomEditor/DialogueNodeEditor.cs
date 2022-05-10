using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace FluffyGameDev.Dialogue.NodalEditor.Editor
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : NodeEditor
    {
        public override void OnBodyGUI() {
            serializedObject.Update();

            DialogueNode node = target as DialogueNode;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Character"), GUIContent.none);
            if (node.m_Answers.Count == 0)
            {
                GUILayout.BeginHorizontal();
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("m_Input"), GUILayout.MinWidth(0));
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("m_Output"), GUILayout.MinWidth(0));
                GUILayout.EndHorizontal();
            }
            else
            {
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("m_Input"));
            }
            GUILayout.Space(-30);

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("m_Text"), GUIContent.none);
            NodeEditorGUILayout.DynamicPortList("m_Answers", typeof(DialogueNode), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
