using System;
using System.Linq;
using UnityEngine;

namespace FluffyGameDev.Dialogue.Deprecated
{
    [Serializable]
    public class DialogueChoice
    {
        [SerializeField]
        private string m_ChoicePreview;
        [SerializeField]
        private DialogueNode m_ChoiceNode;

        public string ChoicePreview => m_ChoicePreview;
        public DialogueNode ChoiceNode => m_ChoiceNode;
    }


    [Obsolete("Use FluffyGameDev.Dialogue.Nodes.ChoiceDialogueNode instead.")]
    public class ChoiceDialogueNode : DialogueNode
    {
        [SerializeField]
        private DialogueChoice[] m_Choices;
        public DialogueChoice[] Choices => m_Choices;


        public override bool CanBeFollowedByNode(DialogueNode node)
        {
            return m_Choices.Any(x => x.ChoiceNode == node);
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}