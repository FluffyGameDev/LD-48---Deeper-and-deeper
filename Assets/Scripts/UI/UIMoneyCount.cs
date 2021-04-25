using UnityEngine;
using UnityEngine.UI;

public class UIMoneyCount : MonoBehaviour
{
    [SerializeField]
    private Text CountTextField;
    [SerializeField]
    private Text PendingCountTextField;
    [SerializeField]
    private PlayerChannel PlayerChannel;
    [SerializeField]
    private Transform EarningsSpawnTransform;
    [SerializeField]
    private UIValueChangeDisplay ValueChangeDisplayPrefab;

    uint m_PreviousCount = 0;

    void Awake()
    {
        PlayerChannel.OnMoneyCountChanged += RefreshCount;
        PlayerChannel.OnPendingMoneyCountChanged += RefreshPending;
    }

    void OnDestroy()
    {
        PlayerChannel.OnPendingMoneyCountChanged -= RefreshPending;
        PlayerChannel.OnMoneyCountChanged -= RefreshCount;
    }

    void RefreshCount(uint count)
    {
        CountTextField.text = count.ToString();

        int diff = (int)count - (int)m_PreviousCount;
        if (diff != 0)
        {
            UIValueChangeDisplay valueDisplay = Instantiate(ValueChangeDisplayPrefab, EarningsSpawnTransform);
            valueDisplay.Value = diff;
        }

        m_PreviousCount = count;
    }

    void RefreshPending(uint count)
    {
        PendingCountTextField.gameObject.SetActive(count > 0);
        PendingCountTextField.text = "+" + count.ToString();
    }
}
