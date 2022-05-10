using UnityEngine;

namespace FluffyGameDev.Dialogue.Deprecated
{
    public abstract class DialogueNode : ScriptableObject
    {
#pragma warning disable CS0618
        [SerializeField]
        private NarrationLine m_DialogueLine;

        public NarrationLine DialogueLine => m_DialogueLine;
#pragma warning restore CS0618

        public abstract bool CanBeFollowedByNode(DialogueNode node);
        public abstract void Accept(DialogueNodeVisitor visitor);
    }
}