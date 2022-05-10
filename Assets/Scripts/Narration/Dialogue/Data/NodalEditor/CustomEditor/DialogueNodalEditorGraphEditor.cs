using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

namespace FluffyGameDev.Dialogue.NodalEditor.Editor
{
    [CustomNodeGraphEditor(typeof(DialogueNodalEditorGraph))]
    public class DialogueGraphEditor : NodeGraphEditor
    {
        public override bool CanRemove(XNode.Node node)
        {
            return !(node is StartNode);
        }

        public override string GetNodeMenuName(System.Type type)
        {
            if (type == typeof(DialogueNode))
                return type.Name;
            else
                return null;
        }
    }
}
