using UnityEngine;

public class StoreUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerChannel PlayerChannel;
    [SerializeField]
    private UIChannel UIChannel;

    [SerializeField]
    private GameObject StoreAccessButton;
    [SerializeField]
    private Transform UpgradesParent;
    [SerializeField]
    private StoreUpgradeUIController UpgradeControllerPrefab;

    [SerializeField]
    private UpgradeData[] Upgrades;

    private Wallet m_Wallet;

    private void Awake()
    {
        UIChannel.OnStoreRequestToggleVisibility += OnStoreRequestToggleVisibility;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIChannel.OnStoreRequestToggleVisibility -= OnStoreRequestToggleVisibility;
    }

    private void OnStoreRequestToggleVisibility(Wallet wallet)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        StoreAccessButton.SetActive(!gameObject.activeSelf);
        m_Wallet = gameObject.activeSelf ? wallet : null;

        PlayerChannel.RaiseMovementEnabled(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            StatHolder stats = m_Wallet.GetComponent<StatHolder>();

            foreach (UpgradeData upgrade in stats.Upgrades)
            {
                StoreUpgradeUIController newController = Instantiate(UpgradeControllerPrefab, UpgradesParent);
                newController.PlayerWallet = m_Wallet;
                newController.Upgrade = upgrade;
            }
        }
        else
        {
            foreach (Transform upgrade in UpgradesParent)
            {
                Destroy(upgrade.gameObject);
            }
        }
    }
}
