using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/3C/Player Channel")]
public class PlayerChannel : ScriptableObject
{
    public delegate void ResourceCountChangedCallback(uint count);
    public ResourceCountChangedCallback OnLadderCountChanged;
    public ResourceCountChangedCallback OnMoneyCountChanged;

    public delegate void MovementEnabledCallback(bool enabled);
    public MovementEnabledCallback OnMovementEnabled;

    public void RaiseLadderCountChanged(uint count)
    {
        OnLadderCountChanged?.Invoke(count);
    }

    public void RaiseMoneyCountChanged(uint count)
    {
        OnMoneyCountChanged?.Invoke(count);
    }

    public void RaiseMovementEnabled(bool enabled)
    {
        OnMovementEnabled?.Invoke(enabled);
    }
}
