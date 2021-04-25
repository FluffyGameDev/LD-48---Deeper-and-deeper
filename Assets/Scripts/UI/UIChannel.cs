using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UI/UI Channel")]
public class UIChannel : ScriptableObject
{
    public delegate void StoreRequestToggleVisibilityCallback(Wallet wallet);
    public StoreRequestToggleVisibilityCallback OnStoreRequestToggleVisibility;

    public void RaiseStoreRequestToggleVisibility(Wallet wallet)
    {
        OnStoreRequestToggleVisibility?.Invoke(wallet);
    }
}
