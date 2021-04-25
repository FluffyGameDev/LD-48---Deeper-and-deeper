using UnityEngine;
using UnityEngine.UI;

public class StoreAccessUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;

    private Button m_Button;

    void Awake()
    {
        m_Button = GetComponent<Button>();

        PlayerChannel.OnPositionChanged += RefreshInteractability;
    }

    void OnDestroy()
    {
        PlayerChannel.OnPositionChanged -= RefreshInteractability;
    }

    void RefreshInteractability(Vector2Int position)
    {
        m_Button.interactable = position.y >= 0;
    }
}
