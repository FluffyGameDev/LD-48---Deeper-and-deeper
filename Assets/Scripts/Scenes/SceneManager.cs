using System.Linq;
using UnityEngine;

[System.Serializable]
public class SceneFlowMapEntry
{
    public FlowState State;
    public string SceneName;
}

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFlowMapEntry[] SceneFlowMap;
    [SerializeField]
    private FlowChannel FlowChannel;

    private string m_CurrentSceneName = "";

    private void Awake()
    {
        FlowChannel.OnFlowStateChanged += OnFlowStateChange;
    }

    private void OnDestroy()
    {
        FlowChannel.OnFlowStateChanged += OnFlowStateChange;
    }

    private void OnFlowStateChange(FlowState state)
    {
        if (m_CurrentSceneName.Length > 0)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(m_CurrentSceneName);
        }

        SceneFlowMapEntry foundEntry = SceneFlowMap.FirstOrDefault(entry => entry.State == state);
        m_CurrentSceneName = foundEntry != null ? foundEntry.SceneName : "";

        UnityEngine.SceneManagement.SceneManager.LoadScene(m_CurrentSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
