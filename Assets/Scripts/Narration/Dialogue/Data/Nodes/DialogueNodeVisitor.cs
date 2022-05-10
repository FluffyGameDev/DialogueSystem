
namespace FluffyGameDev.Dialogue.Nodes
{
    public interface DialogueNodeVisitor
    {
        void Visit(BasicDialogueNode node);
        void Visit(ChoiceDialogueNode node);
    }
}