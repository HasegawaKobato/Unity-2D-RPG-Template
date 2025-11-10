using UnityEngine;

namespace CardBattle
{
    public enum ResourcesType
    {
        UI,
        HoldingCard,
        SubmitCard,
    }

    public enum CardCategory
    {
        /// <summary>
        /// 待機類型，不能給予對方傷害
        /// </summary>
        Idle,

        /// <summary>
        /// 攻擊類型，需要給予對方傷害
        /// </summary>
        Attack,
    }

    public enum CardType
    {
        None,

        /// <summary>
        /// 一般攻擊
        /// </summary>
        Attack,

        /// <summary>
        /// 防禦
        /// </summary>
        Defence,

        /// <summary>
        /// 休息
        /// </summary>
        TakeBreak,

        /// <summary>
        /// 迴避
        /// </summary>
        Avoid,
    }

    public enum BattleStatus
    {
        /// <summary>
        /// 戰鬥持續中，未分出勝負
        /// </summary>
        None,

        /// <summary>
        /// 玩家獲勝
        /// </summary>
        Win,

        /// <summary>
        /// 玩家失敗
        /// </summary>
        Lose
    }
}
