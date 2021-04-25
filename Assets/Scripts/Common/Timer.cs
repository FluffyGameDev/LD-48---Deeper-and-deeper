using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float TimerDuration = 1.0f;
    [SerializeField]
    private bool LoopTimer = false;
    [SerializeField]
    private UnityEvent EndEvent;

    private float m_StartTime = 0.0f;
    private bool m_IsRunning = false;

    public void StartTimer()
    {
        m_StartTime = Time.time;
        m_IsRunning = true;
    }

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (m_IsRunning && Time.time > m_StartTime + TimerDuration)
        {
            EndEvent?.Invoke();
            if (LoopTimer)
            {
                StartTimer();
            }
            else
            {
                m_IsRunning = false;
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
