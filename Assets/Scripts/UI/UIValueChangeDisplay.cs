using UnityEngine;
using UnityEngine.UI;

public class UIValueChangeDisplay : MonoBehaviour
{
    private int m_Value = 0;
    public int Value
    {
        get { return m_Value; }
        set
        {
            if (m_Value != value)
            {
                m_Value = value;

                RefreshText();
            }
        }
    }

    private Text m_TextField;

    private void Awake()
    {
        m_TextField = GetComponent<Text>();
    }

    private void RefreshText()
    {
        m_TextField.text = (m_Value >= 0 ? "+ " : "- ") + System.Math.Abs(m_Value).ToString();
        m_TextField.color = (m_Value >= 0 ? Color.green : Color.red);
    }
}
