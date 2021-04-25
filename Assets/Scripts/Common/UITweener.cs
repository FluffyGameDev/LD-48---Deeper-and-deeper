using UnityEngine;
using UnityEngine.UI;

public class UITweener : MonoBehaviour
{
    public float Duration = 1.0f;

    private Graphic m_GraphicElement;
    private float m_StartTime;
    private bool m_IsRunning;

    public void StartTween()
    {
        m_IsRunning = true;
        m_StartTime = Time.time;
    }

    private void Start()
    {
        m_GraphicElement = GetComponent<Graphic>();
    }

    private void Update()
    {
        if (m_IsRunning)
        {
            float progress = Mathf.Clamp01((Time.time - m_StartTime) / Duration);

            Color color = m_GraphicElement.color;
            color.a = Mathf.Lerp(0.0f, 1.0f, progress);
            m_GraphicElement.color = color;
            if (Time.time > m_StartTime + Duration)
            {
                m_IsRunning = false;
            }
        }
    }
}
