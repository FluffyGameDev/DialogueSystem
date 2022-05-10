using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FluffyGameDev.Dialogue
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {

        [HideInInspector]
        [SerializeReference]
        public List<Nodes.DialogueNode> m_NodeList;

        [HideInInspector]
        [SerializeReference]
        public Nodes.DialogueNode m_StartNode;

#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField]
        private NodalEditor.DialogueNodalEditorGraph m_EditorGraph = null;
        public NodalEditor.DialogueNodalEditorGraph EditorGraph => m_EditorGraph;

        [HideInInspector]
        [SerializeField]
        private Deprecated.DialogueNode m_FirstNode;

        private void Init()
        {
            if (m_FirstNode != null)
            {
                Deprecated.DialogueNodeDeprecator deprecator = new Deprecated.DialogueNodeDeprecator();

                m_FirstNode.Accept(deprecator);

                m_NodeList = new List<Nodes.DialogueNode>();
                foreach (Nodes.DialogueNode node in deprecator.NodeMapping.Values)
                {
                    m_NodeList.Add(node);
                }

                m_StartNode = deprecator.NodeMapping[m_FirstNode];
                m_FirstNode = null;
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();

                Debug.Log(string.Format("Updated Dialogue '{0}' to new format.", name), this);
            }

            if (m_EditorGraph == null)
            {
                // If this asset already exists initialize immediately
                if (UnityEditor.AssetDatabase.Contains(this))
                {
                    DelayedInit();
                }
                // otherwise attach a callback to the editor update to re-check repeatedly until it exists
                // this means it is currently being created an the name has not been confirmed yet
                else
                {
                    UnityEditor.EditorApplication.update -= DelayedInit;
                    UnityEditor.EditorApplication.update += DelayedInit;
                }
            }
        }

        private void Awake()
        {
            Init();
        }

        private void OnValidate()
        {
            Init();
        }

        private void OnDestroy()
        {
            UnityEditor.EditorApplication.update -= DelayedInit;
        }

        private void DelayedInit()
        {
            // Solution found on https://stackoverflow.com/questions/71250177/trying-to-create-nested-scriptableobject-addassettosamefile-failed-because-the

            // if this asset dos still not exist do nothing
            // this means it is currently being created and the name not confirmed yet
            if (!UnityEditor.AssetDatabase.Contains(this))
            {
                return;
            }

            // as soon as the asset exists remove the callback as we don't need it anymore
            UnityEditor.EditorApplication.update -= DelayedInit;

            // first try to find existing child within all assets contained in this asset
            var assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));
            // you could as well use a loop but this Linq query is a shortcut for finding the first sub asset
            // of type "DialogueNodalEditorGraph" or "null" if there was none
            m_EditorGraph = assets.FirstOrDefault(a => a.GetType() == typeof(NodalEditor.DialogueNodalEditorGraph)) as NodalEditor.DialogueNodalEditorGraph;

            if (m_EditorGraph == null)
            {
                m_EditorGraph = CreateInstance<NodalEditor.DialogueNodalEditorGraph>();
                m_EditorGraph.name = "Nodal Graph";
                UnityEditor.AssetDatabase.AddObjectToAsset(m_EditorGraph, this);

                NodalEditor.Editor.DialogueNodalEditorHelper.ConvertDialogueNodesToEditorNodes(this);
            }

            // Mark this asset as dirty so it is correctly saved in case we just changed the "m_EditorGraph" field
            // without using the "AddObjectToAsset" (which afaik does this automatically)
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }

#endif // UNITY_EDITOR

    }
}