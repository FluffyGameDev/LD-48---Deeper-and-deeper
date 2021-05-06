using System.Collections.Generic;
using UnityEngine;

public class StatHolder : MonoBehaviour
{
    public UpgradeData[] Upgrades;
    public StatData[] StatsData;
    public StatData TimeStat;
    public PlayerChannel PlayerChannel;

    private Dictionary<UpgradeData, uint> m_UpgradeTiers = new Dictionary<UpgradeData, uint>();
    private Dictionary<StatData, uint> m_Stats = new Dictionary<StatData, uint>();
    private float m_GameStartTime = 0.0f;
    private bool m_GameStarted = false;

    private void Awake()
    {
        foreach (UpgradeData upgrade in Upgrades)
        {
            m_UpgradeTiers[upgrade] = 0;
        }

        foreach (StatData stat in StatsData)
        {
            m_Stats[stat] = 0;
        }

        PlayerChannel.OnMovementEnabled += OnMovementEnabled;
    }

    private void OnDestroy()
    {
        PlayerChannel.OnMovementEnabled -= OnMovementEnabled;
    }

    private void Update()
    {
        if (m_GameStarted)
        {
            SetStat(TimeStat, (uint)(Time.time - m_GameStartTime));
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

    public uint GetStat(StatData stat)
    {
        return m_Stats[stat];
    }

    public void SetStat(StatData stat, uint value)
    {
        m_Stats[stat] = value;
    }

    public void IncrementStat(StatData stat)
    {
        ++m_Stats[stat];
    }

    private void OnMovementEnabled(bool enabled)
    {
        if (!m_GameStarted && enabled)
        {
            m_GameStarted = true;
            m_GameStartTime = Time.time;
        }
    }
}
