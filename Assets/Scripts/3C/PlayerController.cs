using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float BaseSpeed = 0.5f;
    [SerializeField]
    private float FallSpeed = 1.0f;
    [SerializeField]
    private int FallHeightResistance = 3;
    [SerializeField]
    private Vector3 PositionOffset;
    [SerializeField]
    private GroundTile LadderTile;
    [SerializeField]
    private Tilemap Ground;
    [SerializeField]
    private PlayerChannel PlayerChannel;

    [SerializeField]
    private uint DefaultLadderCount = 10;

    [SerializeField]
    private uint DefaultDrillStrength = 1;
    [SerializeField]
    private float DefaultDrillSpeed = 0.5f;

    [SerializeField]
    private UpgradeData SpeedUpgrade;
    [SerializeField]
    private UpgradeData FallResistanceUpgrade;
    [SerializeField]
    private UpgradeData DrillStrengthUpgrade;
    [SerializeField]
    private UpgradeData DrillSpeedUpgrade;
    [SerializeField]
    private UpgradeData LadderCountUpgrade;
    [SerializeField]
    private UpgradeData MoneyMultiplierUpgrade;

    [SerializeField]
    private Vector2Int m_CurrentPosition;
    [SerializeField]
    private Vector2Int m_TargetPosition;
    [SerializeField]
    private Vector2Int m_StartPosition;
    private float m_StartMoveTime = 0.0f;
    private bool m_IsMoving = false;
    private int m_FallDistance = 0;

    private uint m_LadderCount = 0;
    private Vector3 m_DrillPosition;
    private float m_DrillStartTime;
    private bool m_IsDrilling;

    private Wallet m_Wallet;
    private StatHolder m_StatHolder;
    private uint m_CollectedValue = 0;

    private void Awake()
    {
        m_Wallet = GetComponent<Wallet>();
        m_StatHolder = GetComponent<StatHolder>();

        PlayerChannel.OnMovementEnabled += OnMovementEnabled;
    }

    private void OnDestroy()
    {
        PlayerChannel.OnMovementEnabled -= OnMovementEnabled;
    }

    private void Start()
    {
        Vector3 position = transform.position;
        m_CurrentPosition = new Vector2Int((int)position.x, (int)position.y);
        m_TargetPosition = m_CurrentPosition;

        ResetLadderCount();
    }

    private void Update()
    {
        if (m_IsDrilling)
        {
            if (Time.time >= m_DrillStartTime + DefaultDrillSpeed + m_StatHolder.GetUpgradeStatModifier(DrillSpeedUpgrade))
            {
                EndDrillAnimation();
            }
        }
        else if (!m_IsMoving)
        {
            Vector3 GroundPosition = transform.position + Vector3.down;
            GroundTile tile = GetTileAtPosition(GroundPosition);
            while (tile == null)
            {
                ++m_FallDistance;
                GroundPosition += Vector3.down;
                tile = GetTileAtPosition(GroundPosition);

                if (m_FallDistance > 1000)
                {
                    Debug.LogError("No ground below.");
                    m_FallDistance = 0;
                }
            }

            if (m_FallDistance > 0)
            {
                StartMovement(0, -m_FallDistance);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    StartMovement(-1, 0);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    StartMovement(1, 0);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    StartMovement(0, -1);
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    StartMovement(0, 1);
                }
                else if (Input.GetKey(KeyCode.R))
                {
                    DestroySelf();
                }
            }
        }
        else
        {
            Vector3 movementOffset = new Vector3();

            float speed = BaseSpeed + m_StatHolder.GetUpgradeStatModifier(SpeedUpgrade);
            if (m_FallDistance > 0)
            {
                speed *= FallSpeed * m_FallDistance;
            }
            if (Time.time > m_StartMoveTime + speed)
            {
                m_CurrentPosition = m_TargetPosition;
                m_IsMoving = false;

                if (m_FallDistance > FallHeightResistance) //TODO Stat: Fall resistance
                {
                    DestroySelf();
                }

                if (IsAboveSurface())
                {
                    ResetLadderCount();
                    m_Wallet.Money += m_CollectedValue; //TODO Stat: Money multiplier ?
                    m_CollectedValue = 0;
                }

                m_FallDistance = 0;
            }
            else
            {
                float progress = Mathf.Clamp01((Time.time - m_StartMoveTime) / speed);
                movementOffset.x = Mathf.Lerp((float)m_CurrentPosition.x, (float)m_TargetPosition.x, progress) - m_CurrentPosition.x;
                movementOffset.y = Mathf.Lerp((float)m_CurrentPosition.y, (float)m_TargetPosition.y, progress) - m_CurrentPosition.y;
            }

            Vector3 worldPosition = new Vector3(m_CurrentPosition.x, m_CurrentPosition.y);
            transform.position = worldPosition + movementOffset + PositionOffset;
        }
    }

    private void StartMovement(int dx, int dy)
    {
        Vector3 newPosition = transform.position + new Vector3(dx, dy, 0);

        bool canMove = (dy <= 0 || GetTileAtPosition(transform.position) == LadderTile || TryPlaceLadder(transform.position));
        canMove &= TryDrill(newPosition);

        if (canMove)
        {
            m_StartMoveTime = Time.time;
            m_IsMoving = true;

            m_TargetPosition.x += dx;
            m_TargetPosition.y += dy;
        }
    }

    private GroundTile GetTileAtPosition(Vector3 checkPosition)
    {
        return Ground.GetTile<GroundTile>(Ground.WorldToCell(checkPosition));
    }

    private bool TryPlaceLadder(Vector3 position)
    {
        if (m_LadderCount > 0 && position.y <= 0.0f)
        {
            --m_LadderCount;
            PlayerChannel.RaiseLadderCountChanged(m_LadderCount);
            Ground.SetTile(Ground.WorldToCell(position), LadderTile);
            return false;
        }
        return false;
    }

    private bool TryDrill(Vector3 position)
    {
        bool canMove = true;
        GroundTile tile = GetTileAtPosition(position);
        if (tile != null && !tile.TileData.CanPassThrough)
        {
            if (CanDrillTile(tile))
            {
                StartDrillAnimation(position);
            }
            canMove = false;
        }
        return canMove;
    }

    private void StartDrillAnimation(Vector3 position)
    {
        m_IsDrilling = true;
        m_DrillPosition = position;
        m_DrillStartTime = Time.time;
    }

    private void EndDrillAnimation()
    {
        m_IsDrilling = false;
        GroundTile tile = GetTileAtPosition(m_DrillPosition);
        m_CollectedValue += tile.TileData.Value;
        Ground.SetTile(Ground.WorldToCell(m_DrillPosition), null);

        Vector2 diff = m_DrillPosition - transform.position;
        StartMovement((int)diff.x, (int)diff.y);

        //TODO: particles
    }

    private bool CanDrillTile(GroundTile tile)
    {
        return (DefaultDrillStrength + (uint)m_StatHolder.GetUpgradeStatModifierAsInt(DrillStrengthUpgrade)) >= tile.TileData.DrillResistance;
    }

    private void DestroySelf()
    {
        //TODO: particles

        m_CurrentPosition = FindRespawnPosition();
        m_TargetPosition = m_CurrentPosition;
        m_StartMoveTime = 0.0f;
        m_IsMoving = false;
        m_FallDistance = 0;
        m_CollectedValue = 0;
        ResetLadderCount();

        Vector3 worldPosition = new Vector3(m_CurrentPosition.x, m_CurrentPosition.y);
        transform.position = worldPosition + PositionOffset;
    }

    Vector2Int FindRespawnPosition()
    {
        //TODO: find safe position
        return m_StartPosition;
    }

    private void ResetLadderCount()
    {
        m_LadderCount = DefaultLadderCount + (uint)m_StatHolder.GetUpgradeStatModifierAsInt(LadderCountUpgrade);
        PlayerChannel.RaiseLadderCountChanged(m_LadderCount);
    }

    private bool IsAboveSurface()
    {
        return m_CurrentPosition.y >= 0;
    }

    private void OnMovementEnabled(bool enabled)
    {
        this.enabled = enabled;

        if (enabled && IsAboveSurface())
        {
            ResetLadderCount();
        }
    }
}
