using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventModel : CharacterModelBase
{
    private MoveModel moveModel;

    private List<RaycastHit2D> results = new List<RaycastHit2D>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Init(MapCharacterBase characterBase, MoveModel _moveModel)
    {
        base.Init(characterBase);
        moveModel = _moveModel;

    }

    public void TryTriggerEvent()
    {
        moveModel.Rgd2D.Cast(moveModel.GetDirect(moveModel.Direction), results, moveModel.PerStepDistance);

        // NPC事件優先處理
        int npcTriggerIndex = results.FindIndex(result => result.collider.transform.parent.gameObject.tag == "NPC");
        if (npcTriggerIndex != -1)
        {
            results[npcTriggerIndex].collider.transform.parent.GetComponent<NPC>().TriggerEvent(transform.position, moveModel);
        }
        else
        {
            int triggerIndex = results.FindIndex(result =>
                result.collider.gameObject.tag == "TileEvent" && moveModel.TileMapSetting.movableTilemap.transform.Equals(result.collider.transform.parent)
            );
            if (triggerIndex != -1)
            {
                results[triggerIndex].collider.GetComponent<TileEvent>().TriggerEvent(transform.position, moveModel.Direction);
            }
        }
    }

}
