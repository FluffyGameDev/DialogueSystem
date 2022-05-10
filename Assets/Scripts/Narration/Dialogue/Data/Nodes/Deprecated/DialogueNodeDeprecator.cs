using System.Collections.Generic;

namespace FluffyGameDev.Dialogue.Deprecated
{
    public class DialogueNodeDeprecator : DialogueNodeVisitor
    {
        public Dictionary<DialogueNode, Nodes.DialogueNode> NodeMapping =
                        new Dictionary<DialogueNode, Nodes.DialogueNode>();

#pragma warning disable CS0618
        public void Visit(BasicDialogueNode basicNode)
        {
            if (NodeMapping.ContainsKey(basicNode))
            {
                return;
            }

            Nodes.BasicDialogueNode newBasicNode = new Nodes.BasicDialogueNode();
            NodeMapping.Add(basicNode, newBasicNode);

            if (basicNode.DialogueLine != null)
            {
                newBasicNode.Text = basicNode.DialogueLine.Text;
                newBasicNode.Speaker = basicNode.DialogueLine.Speaker;
            }

            if (basicNode.NextNode != null)
            {
                basicNode.NextNode.Accept(this);
                newBasicNode.NextNode = NodeMapping[basicNode.NextNode];
            }
        }

        public void Visit(ChoiceDialogueNode choiceNode)
        {
            if (NodeMapping.ContainsKey(choiceNode))
            {
                return;
            }

            Nodes.ChoiceDialogueNode newChoiceNode = new Nodes.ChoiceDialogueNode();
            NodeMapping.Add(choiceNode, newChoiceNode);

            if (choiceNode.DialogueLine != null)
            {
                newChoiceNode.Text = choiceNode.DialogueLine.Text;
                newChoiceNode.Speaker = choiceNode.DialogueLine.Speaker;
            }

            newChoiceNode.Choices = new Nodes.DialogueChoice[choiceNode.Choices.Length];

            for (int i = 0; i < choiceNode.Choices.Length; ++i)
            {
                Deprecated.DialogueChoice currentChoice = choiceNode.Choices[i];

                if (currentChoice.ChoiceNode != null)
                {
                    currentChoice.ChoiceNode.Accept(this);

                    Nodes.DialogueChoice newChoice = new Nodes.DialogueChoice();
                    newChoice.ChoiceNode = NodeMapping[currentChoice.ChoiceNode];
                    newChoice.ChoicePreview = currentChoice.ChoicePreview;

                    newChoiceNode.Choices[i] = newChoice;
                }
            }
        }
#pragma warning restore CS0618
    }

}
