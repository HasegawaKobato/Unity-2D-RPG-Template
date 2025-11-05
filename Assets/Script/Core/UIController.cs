using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQueueData
{
    public BasePanel resourcesType;
    public object[] args;
}

public class UIController
{
    public static Queue<UIQueueData> uIQueues = new Queue<UIQueueData>();
    private static bool isBusy = false;

    public static void Init()
    {
        isBusy = false;
    }

    public static void Add(BasePanel resourcesType, params object[] args)
    {
        bool hasData = uIQueues.ToList().Find(queue => queue.resourcesType == resourcesType) != null;

        if (!hasData)
        {
            UIQueueData uIQueueData = new UIQueueData()
            {
                resourcesType = resourcesType,
                args = args
            };
            uIQueues.Enqueue(uIQueueData);

            show();
        }
    }

    private static async void show()
    {
        if (isBusy) return;
        while (uIQueues.Count > 0)
        {
            isBusy = true;
            UIQueueData queueData = uIQueues.Dequeue();
            await queueData.resourcesType.ShowAsync();
            isBusy = false;
        }
    }
    private static void onPanelClosed(BasePanel basePanel)
    {
        isBusy = false;
        basePanel.OnCloseEnd.RemoveAllListeners();
        if (uIQueues.Count > 0)
        {
            show();
        }
    }
}
