using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EndGameStatDisplay
{
    public StatData Stat;
    public Text TextField;
    public bool DisplayAsTime = false;
}

public class UIShowOnGameEnd : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;
    [SerializeField]
    private bool ShowOnGameEnd = false;

    [SerializeField]
    private StatHolder StatHolder;
    [SerializeField]
    private EndGameStatDisplay[] EndGameStats;

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

        foreach (EndGameStatDisplay statDisplay in EndGameStats)
        {
            uint statValue = StatHolder.GetStat(statDisplay.Stat);

            if (statDisplay.DisplayAsTime)
            {
                statDisplay.TextField.text = string.Format("{0,2:D2}:{1,2:D2}", statValue / 60, statValue % 60);
            }
            else
            {
                statDisplay.TextField.text = statValue.ToString();
            }
        }
    }
}
