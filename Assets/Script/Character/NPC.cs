using UnityEngine;

public class NPC : MapCharacterBase
{
    public NPCMoveModel MoveModel => moveModel;
    public SpriteModel SpriteModel => spriteModel;
    public NPCEventModel NPCEventModel => npcEventModel;

    [SerializeField] private NPCMoveModel moveModel = null;
    [SerializeField] private SpriteModel spriteModel = null;
    [SerializeField] private NPCEventModel npcEventModel = null;

    private float delayTime = 0;
    private float timer = 0;

    void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        moveModel.Init(this);
        moveModel.UpdateTargetPosition(transform.position);

        spriteModel.Init(this, moveModel);
        NPCEventModel.Init(this, moveModel);
        delayRandomTime();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Time.time - timer >= delayTime)
        {
            moveRandomDirect();
            delayRandomTime();
        }
    }

    public void TriggerEvent(Vector2 eventModelPosition, MoveModel playerMoveModel)
    {
        NPCEventModel.TryTriggerEvent(eventModelPosition, playerMoveModel);
    }

    public override void ApplyMap(MapUnit mapUnit)
    {
        locateMap = mapUnit;
        moveModel.InitTileMap(mapUnit.Maps);
    }

    private void delayRandomTime()
    {
        timer = Time.time;
        delayTime = Random.Range(1f, 2f);
    }

    private void moveRandomDirect()
    {
        MoveDirect direct = (MoveDirect)Random.Range(1, 5);
        moveModel.Move(direct);
    }
}
