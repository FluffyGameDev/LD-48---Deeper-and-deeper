using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplay : MonoBehaviour
{
    [SerializeField]
    private Text CountTextField;
    [SerializeField]
    private StatData Stat;
    [SerializeField]
    private StatHolder StatHolder;
    [SerializeField]
    private bool DisplayAsTime;

    void Update()
    {
        uint statValue = StatHolder.GetStat(Stat);
        if (DisplayAsTime)
        {
            CountTextField.text = string.Format("{0,2:D2}:{1,2:D2}", statValue / 60, statValue % 60);
        }
        else
        {
            CountTextField.text = statValue.ToString();
        }
    }
}
