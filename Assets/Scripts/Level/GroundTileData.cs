using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level/Ground Tile Data")]
public class GroundTileData : ScriptableObject
{
    public uint Value = 0;
    public uint DrillResistance = 0;
    public bool CanPassThrough = false;
}
