
namespace FluffyGameDev.Dialogue.Deprecated
{
    public interface DialogueNodeVisitor
    {
#pragma warning disable CS0618
        void Visit(BasicDialogueNode node);
        void Visit(ChoiceDialogueNode node);
#pragma warning restore CS0618
    }
}