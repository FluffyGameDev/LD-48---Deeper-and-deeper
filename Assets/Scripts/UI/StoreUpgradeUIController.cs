using UnityEngine;
using UnityEngine.UI;

public class StoreUpgradeUIController : MonoBehaviour
{
    [SerializeField]
    private Text NameField;
    [SerializeField]
    private Text CounterField;
    [SerializeField]
    private Text PriceField;
    [SerializeField]
    private Image ImageField;

    private Button m_Button;

    public Wallet PlayerWallet;

    private UpgradeData m_Upgrade;
    public UpgradeData Upgrade
    {
        get { return m_Upgrade; }
        set
        {
            if (m_Upgrade != value)
            {
                m_Upgrade = value;

                RefreshUI();
            }
        }
    }

    private void Awake()
    {
        m_Button = GetComponent<Button>();
    }

    private void RefreshUI()
    {
        uint tierIndex = PlayerWallet.GetComponent<StatHolder>().GetTierIndex(m_Upgrade);
        UpgradeTierData tier = Upgrade.UpgradeTiers[tierIndex];

        bool isAtMaxTier = tierIndex >= Upgrade.UpgradeTiers.Length - 1;
        if (isAtMaxTier)
        {
            CounterField.text = "Max";
            PriceField.text = "";
        }
        else
        {
            CounterField.text = string.Format("{0}/{1}", tierIndex + 1, Upgrade.UpgradeTiers.Length);
            PriceField.text = tier.UpgradePrice.ToString() + "$";
        }

        NameField.text = tier.UpgradeName;
        ImageField.sprite = tier.UpgradeImage;


        m_Button.interactable = PlayerWallet.Money >= tier.UpgradePrice && !isAtMaxTier;
    }

    public void PurchaseUpgrade()
    {
        StatHolder stats = PlayerWallet.GetComponent<StatHolder>();

        uint tierIndex = stats.GetTierIndex(m_Upgrade);
        UpgradeTierData tier = Upgrade.UpgradeTiers[tierIndex];

        if (PlayerWallet.Money >= tier.UpgradePrice)
        {
            PlayerWallet.Money -= tier.UpgradePrice;
            stats.IncreaseTier(m_Upgrade);


            foreach (Transform upgradeTransform in transform.parent)
            {
                StoreUpgradeUIController upgrade = upgradeTransform.GetComponent<StoreUpgradeUIController>();
                if (upgrade != null)
                {
                    upgrade.RefreshUI();
                }
            }
        }
    }
}
