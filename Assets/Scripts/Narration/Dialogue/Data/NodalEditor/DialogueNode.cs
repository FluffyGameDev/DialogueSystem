using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace FluffyGameDev.Dialogue.NodalEditor
{
    [NodeTint("#CCFFCC"), NodeWidth(300)]
    public class DialogueNode : Node
    {
        [System.Serializable]
        public class Answer
        {
            public string text;
        }

        [SerializeField]
        [Input(backingValue = ShowBackingValue.Never)]
        public DialogueNode m_Input;
        [SerializeField]
        [Output(backingValue = ShowBackingValue.Never)]
        public DialogueNode m_Output;

        [SerializeField]
        public NarrationCharacter m_Character;

        [SerializeField]
        [TextArea]
        public string m_Text;

        [SerializeField]
        [Output(dynamicPortList = true)]
        public List<Answer> m_Answers = new List<Answer>();


        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
