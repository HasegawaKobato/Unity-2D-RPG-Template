using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CardBattle
{
    public class CardBattleResources
    {
        private Dictionary<ResourcesType, GameObject> activeObjects = new Dictionary<ResourcesType, GameObject>();
        private Dictionary<ResourcesType, GameObject> resourceGameObjects = new Dictionary<ResourcesType, GameObject>();
        private Dictionary<ResourcesType, ResourceRequest> resourceRequests = new Dictionary<ResourcesType, ResourceRequest>();

        public void Init()
        {
            load(ResourcesType.UI);
            load(ResourcesType.HoldingCard);
            load(ResourcesType.SubmitCard);
        }

        public GameObject Get(ResourcesType resourcesType)
        {
            if (resourceGameObjects.ContainsKey(resourcesType) && resourceGameObjects[resourcesType] != null)
            {
                return resourceGameObjects[resourcesType];
            }
            else
            {
                Debug.LogWarning($"此資源尚未讀取，請改使用Load: {resourcesType}");
                return null;
            }
        }

        public T GetUI<T>(ResourcesType resourcesType) where T : Component
        {
            if (!activeObjects.ContainsKey(resourcesType))
            {
                GameObject newObject = GameObject.Instantiate(Get(resourcesType));
                T comp = newObject.GetComponent<T>();
                // comp.transform.SetParent(parent);
                comp.transform.localPosition = Vector3.zero;
                comp.transform.localScale = Vector3.one;
                // comp.GetComponent<RectTransform>().anchorMax = Vector2.one;
                // comp.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                // comp.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                // comp.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                activeObjects.Add(resourcesType, newObject);
            }
            activeObjects[resourcesType].transform.SetAsLastSibling();
            return activeObjects[resourcesType].GetComponent<T>();
        }

        private void load(ResourcesType ResourcesType, Action<ResourcesType, GameObject> callback = null)
        {
            ResourceRequest resourceRequest = Resources.LoadAsync(Path.Combine("Prefabs", Config.UIResoucesPath[ResourcesType]));
            void onCompleted(AsyncOperation asyncOperation)
            {
                if (resourceRequest.asset != null)
                {
                    GameObject mapGameObject = (GameObject)resourceRequest.asset;
                    resourceGameObjects.Add(ResourcesType, mapGameObject);
                    if (resourceRequests.ContainsKey(ResourcesType))
                    {
                        resourceRequests.Remove(ResourcesType);
                    }
                    callback?.Invoke(ResourcesType, mapGameObject);
                }
                else
                {
                    Debug.LogWarning($"找不到資源: {ResourcesType}");
                }
            }
            resourceRequest.completed += onCompleted;
            if (!resourceGameObjects.ContainsKey(ResourcesType))
            {
                if (!resourceRequests.ContainsKey(ResourcesType))
                {
                    resourceRequests.Add(ResourcesType, resourceRequest);
                }
                else
                {
                    Debug.LogWarning($"讀取中，請勿嘗試再次讀取: {ResourcesType}");
                }
            }
            else
            {
                callback.Invoke(ResourcesType, resourceGameObjects[ResourcesType]);
            }
        }

    }
}