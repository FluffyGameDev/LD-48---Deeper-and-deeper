using UnityEngine;

public class UIShowOnGameEnd : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;
    [SerializeField]
    private bool ShowOnGameEnd = false;

    private void Awake()
    {
        PlayerChannel.OnGameCompleted += OnGameCompleted;
        if (ShowOnGameEnd)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PlayerChannel.OnGameCompleted -= OnGameCompleted;
    }

    private void OnGameCompleted()
    {
        gameObject.SetActive(ShowOnGameEnd);
    }
}
