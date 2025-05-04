using UnityEngine;

public class TeleportEvent : TileEventProcess
{
    [SerializeField] private Vector2 targetPosition;

    [Tooltip("-1表示照舊，不指定")]
    [SerializeField] private int targetLayer = -1;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Teleport()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (targetLayer != -1)
        {
            player.moveModel.SetLayer(targetLayer);
        }
        player.transform.position = targetPosition;
    }

}
