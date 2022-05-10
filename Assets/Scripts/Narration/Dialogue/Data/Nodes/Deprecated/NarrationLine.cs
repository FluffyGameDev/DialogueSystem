using System;
using UnityEngine;

namespace FluffyGameDev.Dialogue.Deprecated
{
    [Obsolete("Narration Lines are no longer useful.")]
    public class NarrationLine : ScriptableObject
    {
        [SerializeField]
        private NarrationCharacter m_Speaker;
        [SerializeField]
        private string m_Text;

        public NarrationCharacter Speaker => m_Speaker;
        public string Text => m_Text;
    }
}