using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventModel : CharacterModelBase
{
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

    }

    public void TryTriggerEvent()
    {
        moveModel.Rgd2D.Cast(moveModel.GetDirect(moveModel.Direction), results, moveModel.PerStepDistance);
        int triggerIndex = results.FindIndex(result =>
            result.collider.gameObject.tag == "TileEvent" && moveModel.tileMapSetting.movableTilemap.transform.Equals(result.collider.transform.parent)
        );
        Debug.Log(triggerIndex);
        if (triggerIndex != -1)
        {
            results[triggerIndex].collider.GetComponent<TileEvent>().TriggerEvent(transform.position, moveModel.Direction);
        }
    }

}
