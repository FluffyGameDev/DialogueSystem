using UnityEngine;

public class FlowStateMachine : MonoBehaviour
{
    [SerializeField]
    private FlowChannel m_Channel;
    [SerializeField]
    private FlowState m_StartupState;

    private FlowState m_CurrentState;
    public FlowState CurrentState => m_CurrentState;

    private static FlowStateMachine ms_Instance;
    public static FlowStateMachine Instance => ms_Instance;

    private void Awake()
    {
        ms_Instance = this;

        m_Channel.OnFlowStateRequested += SetFlowState;
    }

    private void Start()
    {
        SetFlowState(m_StartupState);
    }

    private void OnDestroy()
    {
        m_Channel.OnFlowStateRequested -= SetFlowState;

        ms_Instance = null;
    }

    private void SetFlowState(FlowState state)
    {
        if (m_CurrentState != state)
        {
            m_CurrentState = state;
            m_Channel.RaiseFlowStateChanged(m_CurrentState);
        }
    }
}
