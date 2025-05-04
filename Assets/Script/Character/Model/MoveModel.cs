using System;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<int, TileMap> movableColliderMap = new Dictionary<int, TileMap>();
    private Rigidbody2D rgd2D => GetComponent<Rigidbody2D>();

    [Tooltip("每步的距離，取絕對值。(0, 0)表示無最小步數，可以隨意移動")]
    [SerializeField] private MoveDirect defaultDirect = MoveDirect.Down;
    [SerializeField] private Vector2 stepSize = Vector2.one;
    [SerializeField] private bool canMoveAnywhere = false;
    [SerializeField] private List<MovableTilemap> movableTilemaps = new List<MovableTilemap>();
    [SerializeField] private float moveSpeed = 10;

    [NonSerialized] public UnityEvent<MoveDirect> onDirectionChanged = null;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        directiion = defaultDirect;
        onDirectionChanged?.Invoke(directiion);

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
                    applyCharacter.transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.x <= targetPosition.x) setCharacterTargetPosition();
                    break;
                case MoveDirect.Right:
                    applyCharacter.transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.x >= targetPosition.x) setCharacterTargetPosition();
                    break;
                case MoveDirect.Up:
                    applyCharacter.transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.y >= targetPosition.y) setCharacterTargetPosition();
                    break;
                case MoveDirect.Down:
                    applyCharacter.transform.Translate(Vector2.down * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.y <= targetPosition.y) setCharacterTargetPosition();
                    break;
            }
        }
    }

    public override void Init(MapCharacterBase characterBase)
    {
        base.Init(characterBase);
    }

    public void UpdateTargetPosition(Vector2 _targetPosition)
    {
        previousPosition = applyCharacter.transform.position;

        if (!canMoveAnywhere)
        {
            rgd2D.Cast(_targetPosition - previousPosition, results, 1);
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

        if (canMoveAnywhere || movableColliderMap[layer] != null)
        {
            moveDirect = direct;
            directiion = moveDirect;
            onDirectionChanged?.Invoke(directiion);
            previousPosition = applyCharacter.transform.position;

            if (!canMoveAnywhere)
            {
                rgd2D.Cast(getDirect(direct), results, 1);
                if (!isCastingInLayer())
                {
                    targetPosition += getDirect(direct);
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
    }

    private Vector2 getDirect(MoveDirect direct)
    {
        switch (direct)
        {
            case MoveDirect.Left:
                return new Vector2(-Mathf.Abs(stepSize.x), 0);
            case MoveDirect.Right:
                return new Vector2(Mathf.Abs(stepSize.x), 0);
            case MoveDirect.Up:
                return new Vector2(0, Mathf.Abs(stepSize.y));
            case MoveDirect.Down:
                return new Vector2(0, -Mathf.Abs(stepSize.y));
        }
        return Vector2.zero;
    }

    private void setCharacterTargetPosition()
    {
        applyCharacter.transform.position = targetPosition;
        moveDirect = MoveDirect.None;
    }

    private bool isCastingInLayer()
    {
        return results.FindIndex(result => result.collider.Equals(movableColliderMap[layer].compositeCollider2D)) != -1;
    }

}
