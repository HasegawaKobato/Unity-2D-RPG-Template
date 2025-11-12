using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CompositeCollider2D))]
public class TileMap : MonoBehaviour
{
    public int Layer => layer;

    public TilemapCollider2D MovableTilemap => movableTilemap;

    /// <summary>
    /// 地圖外圍的邊界碰撞器
    /// </summary>
    public CompositeCollider2D BorderCollider2D => borderCollider2D;

    [SerializeField] private int layer = 0;
    [SerializeField] private TilemapCollider2D movableTilemap;
    [SerializeField] private CompositeCollider2D borderCollider2D;

}
