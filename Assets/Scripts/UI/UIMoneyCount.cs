using UnityEngine;
using UnityEngine.UI;

public class UIMoneyCount : MonoBehaviour
{
    [SerializeField]
    private Text CountTextField;
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
    }

    void OnDestroy()
    {
        PlayerChannel.OnMoneyCountChanged -= RefreshCount;
    }

    void RefreshCount(uint count)
    {
        CountTextField.text = count.ToString() + "$";

        int diff = (int)count - (int)m_PreviousCount;
        if (diff != 0)
        {
            UIValueChangeDisplay valueDisplay = Instantiate(ValueChangeDisplayPrefab, EarningsSpawnTransform);
            valueDisplay.Value = diff;
        }

        m_PreviousCount = count;
    }
}
