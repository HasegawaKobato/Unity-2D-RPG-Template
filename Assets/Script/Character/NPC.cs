using UnityEngine;

public class NPC : MapCharacterBase
{
    [SerializeField] private MoveModel moveModel = null;

    private float delayTime = 0;
    private float timer = 0;

    void Awake()
    {
        moveModel.Init(this);
        moveModel.UpdateTargetPosition(transform.position);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
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
