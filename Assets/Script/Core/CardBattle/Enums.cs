using UnityEngine;

namespace CardBattle
{
    public enum ResourcesType
    {
        UI,
        HoldingCard,
        SubmitCard,
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
}
