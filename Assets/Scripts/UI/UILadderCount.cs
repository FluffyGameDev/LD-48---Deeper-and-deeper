using UnityEngine;
using UnityEngine.UI;

public class UILadderCount : MonoBehaviour
{
    [SerializeField]
    private Text CountTextField;
    [SerializeField]
    private PlayerChannel PlayerChannel;

    void Awake()
    {
        PlayerChannel.OnLadderCountChanged += RefreshCount;
    }

    void OnDestroy()
    {
        PlayerChannel.OnLadderCountChanged -= RefreshCount;
    }

    void RefreshCount(uint count)
    {
        CountTextField.text = count.ToString();
    }
}
