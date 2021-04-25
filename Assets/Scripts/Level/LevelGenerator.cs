using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelGeneratorTileConfig
{
    public TileBase Tile;
    public uint MinY = 0;
    public uint MaxY = uint.MaxValue;
    public uint Weight = 1;
}

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private LevelGeneratorTileConfig[] TileConfigs;
    [SerializeField]
    private LevelGeneratorTileConfig DefaultTileConfig;
    [SerializeField]
    private LevelGeneratorTileConfig UnbreakableTileConfig;
    [SerializeField]
    private LevelGeneratorTileConfig TreasureTileConfig;
    public uint LevelWidth = 10;
    public uint LevelHeight = 10;
    [SerializeField]
    private PlayerChannel PlayerChannel;

    private Tilemap m_TileMap;
    private readonly System.Random m_Random = new System.Random();

    private void Awake()
    {
        PlayerChannel.OnLostTreasure += SpawnTreasure;
    }

    private void OnDestroy()
    {
        PlayerChannel.OnLostTreasure -= SpawnTreasure;
    }

    private void Start()
    {
        m_TileMap = GetComponent<Tilemap>();

        StartCoroutine(GenerateLevel());
    }

    private IEnumerator GenerateLevel()
    {
        List<LevelGeneratorTileConfig> possibleTiles = new List<LevelGeneratorTileConfig>();

        uint realWidth = LevelWidth + 2;
        uint realHeight = LevelHeight + 2;
        TileBase[] block = new TileBase[realWidth * realHeight];
        for (uint y = 0; y < realHeight; ++y)
        {
            uint yOffset = y * realWidth;
            uint depth = LevelHeight - y;

            if (depth > 0)
            {
                FindPossibleTiles(depth, possibleTiles);
                for (uint x = 0; x < LevelWidth; ++x)
                {
                    block[yOffset + x + 1] = ComputeNextTile(possibleTiles);
                }
            }
            block[yOffset] = UnbreakableTileConfig.Tile;
            block[yOffset + LevelWidth + 1] = UnbreakableTileConfig.Tile;
        }

        BoundsInt bounds = new BoundsInt(-(int)realWidth / 2, -(int)LevelHeight, 0, (int)realWidth, (int)realHeight, 1);

        m_TileMap.SetTilesBlock(bounds, block);
        
        SpawnTreasure();

        yield return null;
    }

    private void SpawnTreasure()
    {
        Vector3 treasureCoord = new Vector3(
            m_Random.Next((int)LevelWidth) - LevelWidth / 2,
            -m_Random.Next((int)TreasureTileConfig.MinY, (int)TreasureTileConfig.MaxY),
            0.0f);

        m_TileMap.SetTile(m_TileMap.WorldToCell(treasureCoord), TreasureTileConfig.Tile);
    }

    private void FindPossibleTiles(uint y, List<LevelGeneratorTileConfig> list)
    {
        list.Clear();
        foreach (LevelGeneratorTileConfig config in TileConfigs)
        {
            if (config.MinY <= y && y <= config.MaxY)
            {
                list.Add(config);
            }
        }
    }

    private TileBase ComputeNextTile(List<LevelGeneratorTileConfig> list)
    {
        LevelGeneratorTileConfig foundTileConfig = DefaultTileConfig;

        if (list.Count == 1)
        {
            foundTileConfig = list[0];
        }
        else if (list.Count > 0)
        {
            uint totalWeight = 0;
            totalWeight = list.Aggregate(totalWeight, (uint total, LevelGeneratorTileConfig config) => total += config.Weight);

            uint wantedWeight = (uint)m_Random.Next((int)totalWeight);

            uint weightAccumulation = 0;
            foreach (LevelGeneratorTileConfig config in list)
            {
                weightAccumulation += config.Weight;
                if (wantedWeight < weightAccumulation)
                {
                    foundTileConfig = config;
                    break;
                }
            }
        }

        return foundTileConfig.Tile;
    }
}
