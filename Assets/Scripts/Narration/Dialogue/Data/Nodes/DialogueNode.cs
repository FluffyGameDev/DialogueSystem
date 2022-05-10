using System;
using UnityEngine;

namespace FluffyGameDev.Dialogue.Nodes
{
    [Serializable]
    public abstract class DialogueNode
    {
        public NarrationCharacter Speaker;
        public string Text;

        public abstract bool CanBeFollowedByNode(DialogueNode node);
        public abstract void Accept(DialogueNodeVisitor visitor);
    }
}