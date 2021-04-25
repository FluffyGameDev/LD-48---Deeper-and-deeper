using System.Linq;
using UnityEngine;

[System.Serializable]
public class MusicFlowMapEntry
{
    public FlowState State;
    public AudioClip MusicClip;
}

public class MusicScheduler : MonoBehaviour
{
    [SerializeField]
    private MusicFlowMapEntry[] MusicFlowMap;
    [SerializeField]
    private FlowChannel FlowChannel;

    private AudioClip m_CurrentMusic = null;
    private AudioSource m_AudioSource = null;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

        FlowChannel.OnFlowStateChanged += OnFlowStateChange;
    }

    private void OnDestroy()
    {
        FlowChannel.OnFlowStateChanged += OnFlowStateChange;
    }

    private void OnFlowStateChange(FlowState state)
    {
        MusicFlowMapEntry foundEntry = MusicFlowMap.FirstOrDefault(entry => entry.State == state);
        m_CurrentMusic = foundEntry != null ? foundEntry.MusicClip : null;

        if (m_CurrentMusic != m_AudioSource.clip)
        {
            m_AudioSource.Stop();
            if (m_CurrentMusic != null)
            {
                m_AudioSource.clip = m_CurrentMusic;
                m_AudioSource.Play();
            }
        }
    }
}
