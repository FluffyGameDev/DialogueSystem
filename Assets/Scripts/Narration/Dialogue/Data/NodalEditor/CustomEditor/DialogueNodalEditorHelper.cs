using System.Collections.Generic;
using UnityEngine;

namespace FluffyGameDev.Dialogue.NodalEditor.Editor
{
    public static class DialogueNodalEditorHelper
    {
        public static void ConvertDialogueNodesToEditorNodes(Dialogue dialogue)
        {
            dialogue.EditorGraph.Clear();

            StartNode startNode = dialogue.EditorGraph.AddNode(typeof(StartNode)) as StartNode;
            startNode.position = new Vector2(0, 0);
            startNode.name = "Start";
            UnityEditor.AssetDatabase.AddObjectToAsset(startNode, dialogue.EditorGraph);

            Dictionary<Nodes.DialogueNode, DialogueNode> nodeMapping =
                            new Dictionary<Nodes.DialogueNode, DialogueNode>();

            foreach (Nodes.DialogueNode node in dialogue.m_NodeList)
            {
                DialogueNode newNode = dialogue.EditorGraph.AddNode(typeof(DialogueNode)) as DialogueNode;
                newNode.m_Character = node.Speaker;
                newNode.m_Text = node.Text;
                newNode.position = new Vector2(0, 0);
                newNode.name = "DialogueNode";
                UnityEditor.AssetDatabase.AddObjectToAsset(newNode, dialogue.EditorGraph);

                nodeMapping.Add(node, newNode);
            }

            DialogueNodeConnector connector = new DialogueNodeConnector(nodeMapping);
            foreach (Nodes.DialogueNode node in dialogue.m_NodeList)
            {
                node.Accept(connector);
            }

            if (dialogue.m_StartNode != null)
            {
                DialogueNode destinationNode;
                nodeMapping.TryGetValue(dialogue.m_StartNode, out destinationNode);
                if (destinationNode != null)
                {
                    startNode.GetPort("m_FirstNode").Connect(destinationNode.GetPort("m_Input"));
                }
            }
        }

        private class DialogueNodeConnector : Nodes.DialogueNodeVisitor
        {
            private Dictionary<Nodes.DialogueNode, DialogueNode> m_NodeMapping;

            public DialogueNodeConnector(Dictionary<Nodes.DialogueNode, DialogueNode> nodeMapping)
            {
                m_NodeMapping = nodeMapping;
            }

            public void Visit(Nodes.BasicDialogueNode basicNode)
            {
                if (basicNode == null || basicNode.NextNode == null)
                {
                    return;
                }

                DialogueNode sourceNode;
                DialogueNode destinationNode;
                m_NodeMapping.TryGetValue(basicNode, out sourceNode);
                m_NodeMapping.TryGetValue(basicNode.NextNode, out destinationNode);

                if (sourceNode != null && destinationNode != null)
                {
                    sourceNode.GetPort("m_Output").Connect(destinationNode.GetPort("m_Input"));
                }
            }

            public void Visit(Nodes.ChoiceDialogueNode choiceNode)
            {
                DialogueNode sourceNode;
                m_NodeMapping.TryGetValue(choiceNode, out sourceNode);

                if (sourceNode == null)
                {
                    return;
                }

                sourceNode.m_Answers.Clear();

                for (int index = 0; index < choiceNode.Choices.Length; ++index)
                {
                }

                for (int index = 0; index < choiceNode.Choices.Length; ++index)
                {
                    Nodes.DialogueChoice choice = choiceNode.Choices[index];
                    sourceNode.m_Answers.Add(new DialogueNode.Answer { text = choice.ChoicePreview });

                    string portName = "m_Answers " + index;
                    sourceNode.AddDynamicOutput(typeof(DialogueNode),
                                                XNode.Node.ConnectionType.Override,
                                                XNode.Node.TypeConstraint.None,
                                                portName);

                    if (choiceNode.Choices[index].ChoiceNode != null)
                    {
                        DialogueNode destinationNode;
                        m_NodeMapping.TryGetValue(choice.ChoiceNode, out destinationNode);
                        if (destinationNode != null)
                        {
                            sourceNode.GetPort(portName).Connect(destinationNode.GetPort("m_Input"));
                        }
                    }
                }
            }
        }
    }
}
