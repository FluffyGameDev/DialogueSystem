using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField]
    private string m_CharacterName;

    public string CharacterName => m_CharacterName;
}