using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/3C/Player Channel")]
public class PlayerChannel : ScriptableObject
{
    public delegate void ResourceCountChangedCallback(uint count);
    public ResourceCountChangedCallback OnLadderCountChanged;
    public ResourceCountChangedCallback OnMoneyCountChanged;
    public ResourceCountChangedCallback OnPendingMoneyCountChanged;

    public delegate void MovementEnabledCallback(bool enabled);
    public MovementEnabledCallback OnMovementEnabled;

    public delegate void PositionChangedCallback(Vector2Int position);
    public PositionChangedCallback OnPositionChanged;

    public delegate void GameCompletedCallback();
    public GameCompletedCallback OnGameCompleted;

    public delegate void TreasureCallback();
    public TreasureCallback OnLostTreasure;
    public TreasureCallback OnFoundTreasure;

    public void RaiseLadderCountChanged(uint count)
    {
        OnLadderCountChanged?.Invoke(count);
    }

    public void RaiseMoneyCountChanged(uint count)
    {
        OnMoneyCountChanged?.Invoke(count);
    }

    public void RaisePendingMoneyCountChanged(uint count)
    {
        OnPendingMoneyCountChanged?.Invoke(count);
    }

    public void RaiseMovementEnabled(bool enabled)
    {
        OnMovementEnabled?.Invoke(enabled);
    }

    public void RaisePositionChanged(Vector2Int position)
    {
        OnPositionChanged?.Invoke(position);
    }

    public void RaiseGameCompleted()
    {
        OnGameCompleted?.Invoke();
    }

    public void RaiseLostTreasure()
    {
        OnLostTreasure?.Invoke();
    }

    public void RaiseFoundTreasure()
    {
        OnFoundTreasure?.Invoke();
    }
}
