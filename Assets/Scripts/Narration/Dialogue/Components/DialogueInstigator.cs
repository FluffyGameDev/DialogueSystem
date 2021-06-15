using UnityEngine;

public class DialogueInstigator : MonoBehaviour
{
    [SerializeField]
    private DialogueChannel m_DialogueChannel;
    [SerializeField]
    private FlowChannel m_FlowChannel;
    [SerializeField]
    private FlowState m_DialogueState;

    private DialogueSequencer m_DialogueSequencer;
    private FlowState m_CachedFlowState;

    private void Awake()
    {
        m_DialogueSequencer = new DialogueSequencer();

        m_DialogueSequencer.OnDialogueStart += OnDialogueStart;
        m_DialogueSequencer.OnDialogueEnd += OnDialogueEnd;
        m_DialogueSequencer.OnDialogueNodeStart += m_DialogueChannel.RaiseDialogueNodeStart;
        m_DialogueSequencer.OnDialogueNodeEnd += m_DialogueChannel.RaiseDialogueNodeEnd;

        m_DialogueChannel.OnDialogueRequested += m_DialogueSequencer.StartDialogue;
        m_DialogueChannel.OnDialogueNodeRequested += m_DialogueSequencer.StartDialogueNode;
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeRequested -= m_DialogueSequencer.StartDialogueNode;
        m_DialogueChannel.OnDialogueRequested -= m_DialogueSequencer.StartDialogue;

        m_DialogueSequencer.OnDialogueNodeEnd -= m_DialogueChannel.RaiseDialogueNodeEnd;
        m_DialogueSequencer.OnDialogueNodeStart -= m_DialogueChannel.RaiseDialogueNodeStart;
        m_DialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueSequencer.OnDialogueStart -= OnDialogueStart;

        m_DialogueSequencer = null;
    }

    private void OnDialogueStart(Dialogue dialogue)
    {
        m_DialogueChannel.RaiseDialogueStart(dialogue);

        m_CachedFlowState = FlowStateMachine.Instance.CurrentState;
        m_FlowChannel.RaiseFlowStateRequest(m_DialogueState);
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {
        m_FlowChannel.RaiseFlowStateRequest(m_CachedFlowState);
        m_CachedFlowState = null;

        m_DialogueChannel.RaiseDialogueEnd(dialogue);
    }
}
