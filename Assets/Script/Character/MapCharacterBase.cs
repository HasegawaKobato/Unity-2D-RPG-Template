using UnityEngine;

public class MapCharacterBase : MonoBehaviour
{
    public MapUnit LocateMap => locateMap;

    protected MapUnit locateMap = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void ApplyMap(MapUnit mapUnit)
    {
        locateMap = mapUnit;
    }
}
