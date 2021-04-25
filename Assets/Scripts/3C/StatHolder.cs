using System.Collections.Generic;
using UnityEngine;

public class StatHolder : MonoBehaviour
{
    public UpgradeData[] Upgrades;

    Dictionary<UpgradeData, uint> m_UpgradeTiers = new Dictionary<UpgradeData, uint>();

    private void Awake()
    {
        foreach (UpgradeData upgrade in Upgrades)
        {
            m_UpgradeTiers[upgrade] = 0;
        }
    }

    public uint GetTierIndex(UpgradeData upgrade)
    {
        return m_UpgradeTiers[upgrade];
    }

    public void IncreaseTier(UpgradeData upgrade)
    {
        ++m_UpgradeTiers[upgrade];
    }

    public float GetUpgradeStatModifier(UpgradeData upgrade)
    {
        return upgrade.UpgradeTiers[m_UpgradeTiers[upgrade]].StatModifier;
    }

    public int GetUpgradeStatModifierAsInt(UpgradeData upgrade)
    {
        return (int)(GetUpgradeStatModifier(upgrade) + 0.1f);
    }
}
