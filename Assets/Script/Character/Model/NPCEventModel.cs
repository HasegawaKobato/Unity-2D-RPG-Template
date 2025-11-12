using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum NPCEventTriggerType
{
    /// <summary>
    /// 不與玩家重疊，且須面向NPC後手動觸發。
    /// </summary>
    Default,

    /// <summary>
    /// 觸碰NPC後觸發。
    /// </summary>
    Touch,

    /// <summary>
    /// 以上兩者都可以
    /// </summary>
    Both,
}

public class NPCEventModel : CharacterModelBase
{
    [SerializeField] private NPCEventTriggerType triggerType = NPCEventTriggerType.Default;
    [SerializeField, Tooltip("玩家主動觸發")] private UnityEvent onDefaultTrigger = null;
    [SerializeField, Tooltip("接觸觸發")] private UnityEvent onTouchTrigger = null;

    private MoveModel moveModel;
    private List<RaycastHit2D> results = new List<RaycastHit2D>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Init(MapCharacterBase characterBase, MoveModel _moveModel)
    {
        base.Init(characterBase);
        moveModel = _moveModel;

        moveModel.onColiisionEnter.AddListener(onColiisionEnter);
    }

    public void TryTriggerEvent(Vector2 eventModelPosition, MoveModel playerMoveModel)
    {
        if (moveModel.IsMoving) return;
        if (triggerType == NPCEventTriggerType.Default || triggerType == NPCEventTriggerType.Both)
        {
            if (Vector2.Distance(transform.position, eventModelPosition) == 1)
            {
                switch (playerMoveModel.Direction)
                {
                    case MoveDirect.Left:
                        moveModel.ChangeDirection(MoveDirect.Right);
                        break;
                    case MoveDirect.Right:
                        moveModel.ChangeDirection(MoveDirect.Left);
                        break;
                    case MoveDirect.Up:
                        moveModel.ChangeDirection(MoveDirect.Down);
                        break;
                    case MoveDirect.Down:
                        moveModel.ChangeDirection(MoveDirect.Up);
                        break;
                }
                onDefaultTrigger?.Invoke();
            }
        }
    }

    private void onColiisionEnter(Collision2D collision)
    {
        if (triggerType == NPCEventTriggerType.Touch || triggerType == NPCEventTriggerType.Both)
        {
            if (collision.collider.transform.parent.gameObject.tag == "Player")
            {
                if (Vector2.Distance(transform.position, collision.collider.transform.parent.GetComponent<Player>().EventModel.transform.position) < 1)
                {
                    onTouchTrigger?.Invoke();
                }
            }
        }
    }

}
