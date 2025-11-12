using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum MapType
{
    Sample,
}

public class MapController
{
    private static Dictionary<MapType, string> mapResourcePath = new Dictionary<MapType, string>(){
        {MapType.Sample, "Map/Sample"},
    };

    private static Dictionary<MapType, GameObject> mapGameObjects = new Dictionary<MapType, GameObject>();
    private static Dictionary<MapType, ResourceRequest> resourceRequests = new Dictionary<MapType, ResourceRequest>();

    private static Dictionary<MapType, MapUnit> activeMapObjects = new Dictionary<MapType, MapUnit>();

    public static float percentage
    {
        get
        {
            if (resourceRequests.Count > 0)
            {
                float totalProgress = 0;
                foreach (var resourceRequest in resourceRequests.Values)
                {
                    totalProgress += resourceRequest.progress;
                }
                return totalProgress / resourceRequests.Count;
            }
            else
            {
                return 1;
            }
        }
    }

    public static MapUnit Get(MapType mapType)
    {
        if (!activeMapObjects.ContainsKey(mapType))
        {
            if (!mapGameObjects.ContainsKey(mapType))
            {
                UnityEngine.Object asset = Resources.Load(Path.Combine("Prefabs", mapResourcePath[mapType]));
                GameObject mapGameObject = (GameObject)asset;
                mapGameObjects.Add(mapType, mapGameObject);
            }
            GameObject newMapObject = GameObject.Instantiate(mapGameObjects[mapType]);
            MapUnit mapUnit = newMapObject.GetComponent<MapUnit>();
            activeMapObjects.Add(mapType, mapUnit);
        }
        activeMapObjects[mapType].transform.position = Vector3.zero;
        return activeMapObjects[mapType];
    }

    public static void GetMapAsync(MapType mapType, Action<MapType, GameObject> callback = null)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync(Path.Combine("Prefabs", mapResourcePath[mapType]));
        void onCompleted(AsyncOperation asyncOperation)
        {
            GameObject mapGameObject = (GameObject)resourceRequest.asset;
            mapGameObjects.Add(mapType, mapGameObject);
            if (resourceRequests.ContainsKey(mapType))
            {
                resourceRequests.Remove(mapType);
            }
            callback?.Invoke(mapType, mapGameObject);
        }
        resourceRequest.completed += onCompleted;
        if (!mapGameObjects.ContainsKey(mapType))
        {
            if (!resourceRequests.ContainsKey(mapType))
            {
                resourceRequests.Add(mapType, resourceRequest);
            }
            else
            {
                Debug.LogWarning($"讀取中，請勿嘗試再次讀取: {mapType}");
            }
        }
        else
        {
            callback?.Invoke(mapType, mapGameObjects[mapType]);
        }
    }

    public static void InitAllAsync()
    {
        mapResourcePath.Keys.ToList().ForEach(mapType =>
        {
            GetMapAsync(mapType);
        });
    }
}
