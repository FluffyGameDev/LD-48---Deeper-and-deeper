using UnityEngine;

public class FlowManager : MonoBehaviour
{
    static private FlowManager ms_Instance;
    static public FlowManager Instance => ms_Instance;

    [SerializeField]
    private FlowState DefaultState;
    [SerializeField]
    private FlowChannel FlowChannel;

    private FlowState m_CurrentState = null;
    public FlowState CurrentState
    {
        get { return m_CurrentState; }
        set
        {
            if (m_CurrentState != value)
            {
                m_CurrentState = value;
                FlowChannel.RaiseFlowStateChanged(m_CurrentState);
            }
        }
    }

    public delegate void FlowStateChangeCallback(FlowState state);
    public FlowStateChangeCallback OnFlowStateChange;

    private void Awake()
    {
        ms_Instance = this;
        FlowChannel.OnFlowStateRequested += RequestFlowState;
    }

    private void OnDestroy()
    {
        FlowChannel.OnFlowStateRequested -= RequestFlowState;
        ms_Instance = null;
    }

    private void Start()
    {
        RequestFlowState(DefaultState);
    }

    private void RequestFlowState(FlowState state)
    {
        CurrentState = state;
    }
}
