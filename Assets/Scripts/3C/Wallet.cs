using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;

    [SerializeField]
    private uint m_Money = 0;
    public uint Money
    {
        get { return m_Money; }
        set
        {
            if (m_Money != value)
            {
                m_Money = value;
                PlayerChannel.OnMoneyCountChanged(m_Money);
            }
        }
    }
}
