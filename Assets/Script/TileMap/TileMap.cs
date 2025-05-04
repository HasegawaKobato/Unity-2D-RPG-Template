using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CompositeCollider2D))]
public class TileMap : MonoBehaviour
{
    [SerializeField] public TilemapCollider2D movableTilemap;

    [NonSerialized] public CompositeCollider2D compositeCollider2D = null;

    void Start()
    {
        compositeCollider2D = GetComponent<CompositeCollider2D>();
    }
}
