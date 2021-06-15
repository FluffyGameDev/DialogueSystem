
public interface DialogueNodeVisitor
{
    void Visit(BasicDialogueNode node);
    void Visit(ChoiceDialogueNode node);
}