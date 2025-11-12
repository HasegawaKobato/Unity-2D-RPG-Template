using UnityEngine;

public class NPCMoveModel : MoveModel
{
    /// <summary>
    /// MEMO: 目前NPC不會跳圖層，所以可以直接設定該NPC的所在層級
    /// </summary>
    [SerializeField] private int locateLayer;

    public void SetLayer()
    {
        layer = locateLayer;

        if (tag == "PlayerModel")
        {
            applyCharacter.GetComponent<Player>().SpriteModel.spriteRenderer.sortingOrder = layer;
        }
    }

}
