using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[Serializable]
public class MovableTilemap
{
    public int layer = 0;

    /// <summary>
    /// 當前Layer可行走的tileMap
    /// </summary>
    public TileMap tileMap;
}

public class MoveModel : CharacterModelBase
{
    public Rigidbody2D Rgd2D => GetComponent<Rigidbody2D>();
    public MoveDirect Direction => directiion;
    public float PerStepDistance => perStepDistance;
    public float Speed => isRunning ? moveSpeed * 2 : moveSpeed;
    public bool IsRunning => isRunning;
    public bool IsMoving => moveDirect != MoveDirect.None;
    public TileMap TileMapSetting
    {
        get
        {
            if (movableColliderMap.ContainsKey(layer)) return movableColliderMap[layer];
            return null;
        }
    }
    [NonSerialized] public UnityEvent<MoveDirect> onDirectionChanged = new UnityEvent<MoveDirect>();
    [NonSerialized] public UnityEvent<Collision2D> onColiisionEnter = new UnityEvent<Collision2D>();

    [Tooltip("每步的距離，取絕對值。(0, 0)表示無最小步數，可以隨意移動")]
    [SerializeField] private MoveDirect defaultDirect = MoveDirect.Down;
    [SerializeField] private float perStepDistance = 1;
    [SerializeField] private bool canMoveAnywhere = false;
    [SerializeField] private List<MovableTilemap> movableTilemaps = new List<MovableTilemap>();
    [SerializeField] private float moveSpeed = 10;

    private Dictionary<int, TileMap> movableColliderMap = new Dictionary<int, TileMap>();

    /// <summary>
    /// 決定目標位置時的當前位置
    /// </summary>
    private Vector2 previousPosition = Vector2.zero;

    /// <summary>
    /// 目標位置
    /// </summary>
    private Vector2 targetPosition = Vector2.zero;

    /// <summary>
    /// 移動方向
    /// </summary>
    private MoveDirect moveDirect = MoveDirect.None;

    /// <summary>
    /// 角色面向
    /// </summary>
    private MoveDirect directiion = MoveDirect.None;

    private List<RaycastHit2D> results = new List<RaycastHit2D>();

    private int layer = 0;
    private bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        directiion = defaultDirect;
        onDirectionChanged.Invoke(directiion);

        if (!canMoveAnywhere)
        {
            movableTilemaps.ForEach(tilemap =>
            {
                if (!movableColliderMap.ContainsKey(tilemap.layer))
                {
                    movableColliderMap.Add(tilemap.layer, tilemap.tileMap);
                }
            });
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (moveDirect != MoveDirect.None)
        {
            switch (moveDirect)
            {
                case MoveDirect.Left:
                    applyCharacter.transform.Translate(Vector2.left * Time.deltaTime * Speed);
                    if (applyCharacter.transform.position.x <= targetPosition.x) setCharacterTargetPosition();
                    break;
                case MoveDirect.Right:
                    applyCharacter.transform.Translate(Vector2.right * Time.deltaTime * Speed);
                    if (applyCharacter.transform.position.x >= targetPosition.x) setCharacterTargetPosition();
                    break;
                case MoveDirect.Up:
                    applyCharacter.transform.Translate(Vector2.up * Time.deltaTime * Speed);
                    if (applyCharacter.transform.position.y >= targetPosition.y) setCharacterTargetPosition();
                    break;
                case MoveDirect.Down:
                    applyCharacter.transform.Translate(Vector2.down * Time.deltaTime * Speed);
                    if (applyCharacter.transform.position.y <= targetPosition.y) setCharacterTargetPosition();
                    break;
            }
        }
    }

    public override void Init(MapCharacterBase characterBase)
    {
        base.Init(characterBase);
        SetLayer(layer);
    }

    public void UpdateTargetPosition(Vector2 _targetPosition)
    {
        previousPosition = applyCharacter.transform.position;

        if (!canMoveAnywhere)
        {
            Rgd2D.Cast(_targetPosition - previousPosition, results, perStepDistance);
            if (!isCastingInLayer())
            {
                targetPosition = _targetPosition;
            }
            else
            {
                targetPosition = previousPosition;
            }
        }
        else
        {
            targetPosition = _targetPosition;
        }
    }

    public void Move(MoveDirect direct)
    {
        if (moveDirect != MoveDirect.None) return;

        if (canMoveAnywhere || TileMapSetting != null)
        {
            moveDirect = direct;
            directiion = moveDirect;
            onDirectionChanged.Invoke(directiion);
            previousPosition = applyCharacter.transform.position;

            if (!canMoveAnywhere)
            {
                Rgd2D.Cast(GetDirect(direct), results, perStepDistance);
                if (!isCastingInLayer())
                {
                    targetPosition += GetDirect(direct);
                }
                else
                {
                    targetPosition = previousPosition;
                }
            }
        }
    }

    public void SetLayer(int _layer)
    {
        layer = _layer;

        if (tag == "PlayerModel")
        {
            applyCharacter.GetComponent<Player>().spriteModel.spriteRenderer.sortingOrder = layer;

            movableTilemaps.ForEach(setting =>
            {
                if (setting.layer > layer)
                {
                    setting.tileMap.GetComponentsInChildren<Tilemap>(true).ToList().ForEach(map =>
                    {
                        map.color = new Color(1, 1, 1, 0.5f);
                    });
                }
                else
                {
                    setting.tileMap.GetComponentsInChildren<Tilemap>(true).ToList().ForEach(map =>
                    {
                        map.color = Color.white;
                    });
                }
            });
        }
    }

    public void SetRunning(bool _running)
    {
        isRunning = _running;
    }

    public void ChangeDirection(MoveDirect direct)
    {
        if (IsMoving) return;
        directiion = direct;
        onDirectionChanged.Invoke(directiion);
    }

    public Vector2 GetDirect(MoveDirect direct)
    {
        switch (direct)
        {
            case MoveDirect.Left:
                return new Vector2(-Mathf.Abs(perStepDistance), 0);
            case MoveDirect.Right:
                return new Vector2(Mathf.Abs(perStepDistance), 0);
            case MoveDirect.Up:
                return new Vector2(0, Mathf.Abs(perStepDistance));
            case MoveDirect.Down:
                return new Vector2(0, -Mathf.Abs(perStepDistance));
        }
        return Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onColiisionEnter.Invoke(collision);
    }

    private void setCharacterTargetPosition()
    {
        applyCharacter.transform.position = targetPosition;
        moveDirect = MoveDirect.None;
    }

    private bool isCastingInLayer()
    {
        return results.FindIndex(result => result.collider.Equals(TileMapSetting.compositeCollider2D)) != -1;
    }

}
