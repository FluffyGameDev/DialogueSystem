using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlowListenerEntry
{
    public FlowState m_State;
    public UnityEvent m_Event;
}

public class FlowListener : MonoBehaviour
{
    [SerializeField]
    private FlowChannel m_Channel;
    [SerializeField]
    private FlowListenerEntry[] m_Entries;

    private void Awake()
    {
        m_Channel.OnFlowStateChanged += OnFlowStateChanged;
    }

    private void OnDestroy()
    {
        m_Channel.OnFlowStateChanged -= OnFlowStateChanged;
    }

    private void OnFlowStateChanged(FlowState state)
    {
        FlowListenerEntry foundEntry = Array.Find(m_Entries, x => x.m_State == state);
        if (foundEntry != null)
        {
            foundEntry.m_Event.Invoke();
        }
    }
}