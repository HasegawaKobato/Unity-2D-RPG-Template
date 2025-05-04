using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventTriggerType
{
    /// <summary>
    /// 不與事件重疊，且須面向事件。
    /// </summary>
    Default,

    /// <summary>
    /// 須與事件重疊，無論是否面向事件。
    /// </summary>
    Overlap,

    /// <summary>
    /// 以上兩者都可以
    /// </summary>
    Both,
}

public class TileEvent : MonoBehaviour
{
    [SerializeField] private EventTriggerType triggerType = EventTriggerType.Default;
    [Tooltip("僅在Default或Both有效。不與事件重疊時，可接受的玩家面向。None為全接受")]
    [SerializeField] private List<MoveDirect> defaultValidDirect = new List<MoveDirect>() { MoveDirect.None };
    [SerializeField] private UnityEvent onDefaultTrigger = null;
    [SerializeField] private UnityEvent onOverlapTrigger = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerEvent(Vector2 eventModelPosition, MoveDirect playerDirection)
    {
        if (triggerType == EventTriggerType.Default || triggerType == EventTriggerType.Both)
        {
            if (Vector2.Distance(transform.position, eventModelPosition) == 1)
            {
                List<MoveDirect> validMoveDirect = new List<MoveDirect>();
                if (defaultValidDirect.Count > 0)
                {
                    if (defaultValidDirect.Contains(MoveDirect.None))
                    {
                        validMoveDirect = new List<MoveDirect>() { MoveDirect.Left, MoveDirect.Right, MoveDirect.Up, MoveDirect.Down };
                    }
                    else
                    {
                        validMoveDirect = new List<MoveDirect>(defaultValidDirect);
                    }
                }
                if (validMoveDirect.Contains(playerDirection))
                {
                    onDefaultTrigger?.Invoke();
                }
            }
        }
        if (triggerType == EventTriggerType.Overlap || triggerType == EventTriggerType.Both)
        {
            if (Vector2.Distance(transform.position, eventModelPosition) == 0)
            {
                onOverlapTrigger?.Invoke();
            }
        }
    }
}
