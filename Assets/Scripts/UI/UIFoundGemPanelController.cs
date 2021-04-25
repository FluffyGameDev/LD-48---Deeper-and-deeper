using UnityEngine;

public class UIFoundGemPanelController : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;

    void Awake()
    {
        PlayerChannel.OnLostTreasure += HidePanel;
        PlayerChannel.OnFoundTreasure += ShowPanel;

        HidePanel();
    }

    void OnDestroy()
    {
        PlayerChannel.OnFoundTreasure -= ShowPanel;
        PlayerChannel.OnLostTreasure -= HidePanel;
    }

    void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
