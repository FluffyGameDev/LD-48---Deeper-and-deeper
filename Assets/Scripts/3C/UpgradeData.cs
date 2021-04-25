using UnityEngine;

[System.Serializable]
public class UpgradeTierData
{
    public string UpgradeName;
    public Sprite UpgradeImage;
    public uint UpgradePrice;
    public float StatModifier;
}

[CreateAssetMenu(menuName = "Scriptable Objects/3C/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public UpgradeTierData[] UpgradeTiers;
}
