using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FluffyGameDev.Dialogue.NodalEditor.Editor
{
    public class DialogueGraphSaveAssetsProcessor : UnityEditor.AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (string path in paths)
            {
                Dialogue dialogue = (Dialogue)AssetDatabase.LoadAssetAtPath(path, typeof(Dialogue));
                if (dialogue != null && dialogue.EditorGraph != null)
                {
                    StartNode foundStartNode = null;
                    Dictionary<DialogueNode, Nodes.DialogueNode> nodeMap = new Dictionary<DialogueNode, Nodes.DialogueNode>();

                    for (int i = 0; i < dialogue.EditorGraph.nodes.Count; ++i)
                    {
                        dialogue.m_StartNode = null;
                        dialogue.m_NodeList.Clear();

                        if (dialogue.EditorGraph.nodes[i] is DialogueNode dialogueNode)
                        {
                            if (dialogueNode.m_Answers.Count == 0)
                            {
                                nodeMap.Add(dialogueNode, new Nodes.BasicDialogueNode());
                            }
                            else
                            {
                                nodeMap.Add(dialogueNode, new Nodes.ChoiceDialogueNode());
                            }
                        }
                        else if (dialogue.EditorGraph.nodes[i] is StartNode startNode)
                        {
                            foundStartNode = startNode;
                        }
                    }

                    DialogueNodeConverter converter = new DialogueNodeConverter(nodeMap);
                    foreach (var pair in nodeMap)
                    {
                        converter.m_SourceNode = pair.Key;
                        pair.Value.Accept(converter);
                    }

                    if (foundStartNode != null)
                    {
                        dialogue.m_StartNode = nodeMap[foundStartNode.GetPort("m_FirstNode").Connection.node as DialogueNode];
                    }
                }
            }
    
            return paths;
        }

        private class DialogueNodeConverter : Nodes.DialogueNodeVisitor
        {
            public DialogueNode m_SourceNode = null;
            private Dictionary<DialogueNode, Nodes.DialogueNode> m_NodeMap;

            public DialogueNodeConverter(Dictionary<DialogueNode, Nodes.DialogueNode> nodeMap)
            {
                m_NodeMap = nodeMap;
            }

            public void Visit(Nodes.BasicDialogueNode basicNode)
            {
                basicNode.Speaker = m_SourceNode.m_Character;
                basicNode.Text = m_SourceNode.m_Text;

                XNode.NodePort outputPort = m_SourceNode.GetOutputPort("m_Output");
                if (outputPort != null && outputPort.Connection != null)
                {
                    DialogueNode nextNode = outputPort.Connection.node as DialogueNode;
                    if (nextNode != null)
                    {
                        basicNode.NextNode = m_NodeMap[nextNode];
                    }
                }
            }

            public void Visit(Nodes.ChoiceDialogueNode choiceNode)
            {
                choiceNode.Speaker = m_SourceNode.m_Character;
                choiceNode.Text = m_SourceNode.m_Text;

                List<Nodes.DialogueChoice> choices = new List<Nodes.DialogueChoice>();
                for (int i = 0; i < m_SourceNode.m_Answers.Count; ++i)
                {
                    DialogueNode.Answer answer = m_SourceNode.m_Answers[i];
                    XNode.NodePort outputPort = m_SourceNode.GetOutputPort("m_Answers " + i);
                    DialogueNode nextNode = null;
                    if (outputPort != null && outputPort.Connection != null)
                    {
                        nextNode = outputPort.Connection.node as DialogueNode;
                    }

                    Nodes.DialogueChoice newChoice = new Nodes.DialogueChoice();
                    newChoice.ChoicePreview = answer.text;
                    if (nextNode != null)
                    {
                        newChoice.ChoiceNode = m_NodeMap[nextNode];
                    }

                    choices.Add(newChoice);
                }

                choiceNode.Choices = choices.ToArray();
            }
        }
    }
}
