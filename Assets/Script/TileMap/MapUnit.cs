using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUnit : MonoBehaviour
{
    public List<TileMap> Maps => maps;

    [SerializeField] private List<TileMap> maps = new List<TileMap>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Active(Player character, int layer = 0)
    {
        character.ApplyMap(this);
        character.MoveModel.SetLayer(layer);
        maps.ForEach(tileMap =>
        {
            if (tileMap.Layer > layer)
            {
                tileMap.GetComponentsInChildren<Tilemap>(true).ToList().ForEach(map =>
                {
                    map.color = new Color(1, 1, 1, 0.5f);
                });
            }
            else
            {
                tileMap.GetComponentsInChildren<Tilemap>(true).ToList().ForEach(map =>
                {
                    map.color = Color.white;
                });
            }
        });
    }
}
