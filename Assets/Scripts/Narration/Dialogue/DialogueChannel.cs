using UnityEngine;

namespace FluffyGameDev.Dialogue
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Dialogue Channel")]
    public class DialogueChannel : ScriptableObject
    {
        public delegate void DialogueCallback(Dialogue dialogue);
        public DialogueCallback OnDialogueRequested;
        public DialogueCallback OnDialogueStart;
        public DialogueCallback OnDialogueEnd;

        public delegate void DialogueNodeCallback(Nodes.DialogueNode node);
        public DialogueNodeCallback OnDialogueNodeRequested;
        public DialogueNodeCallback OnDialogueNodeStart;
        public DialogueNodeCallback OnDialogueNodeEnd;

        public void RaiseRequestDialogue(Dialogue dialogue)
        {
            OnDialogueRequested?.Invoke(dialogue);
        }

        public void RaiseDialogueStart(Dialogue dialogue)
        {
            OnDialogueStart?.Invoke(dialogue);
        }

        public void RaiseDialogueEnd(Dialogue dialogue)
        {
            OnDialogueEnd?.Invoke(dialogue);
        }

        public void RaiseRequestDialogueNode(Nodes.DialogueNode node)
        {
            OnDialogueNodeRequested?.Invoke(node);
        }

        public void RaiseDialogueNodeStart(Nodes.DialogueNode node)
        {
            OnDialogueNodeStart?.Invoke(node);
        }

        public void RaiseDialogueNodeEnd(Nodes.DialogueNode node)
        {
            OnDialogueNodeEnd?.Invoke(node);
        }
    }
}