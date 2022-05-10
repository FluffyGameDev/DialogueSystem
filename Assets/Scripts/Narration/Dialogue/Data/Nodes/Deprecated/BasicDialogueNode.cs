using System;
using UnityEngine;

namespace FluffyGameDev.Dialogue.Deprecated
{
    [Obsolete("Use FluffyGameDev.Dialogue.Nodes.BasicDialogueNode instead.")]
    public class BasicDialogueNode : DialogueNode
    {
        [SerializeField]
        private DialogueNode m_NextNode;
        public DialogueNode NextNode => m_NextNode;


        public override bool CanBeFollowedByNode(DialogueNode node)
        {
            return m_NextNode == node;
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}