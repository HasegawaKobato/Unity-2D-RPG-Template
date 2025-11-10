using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class CardData
    {
        public CardCategory category = CardCategory.Idle;

        /// <summary>
        /// 卡片基礎數值。
        /// </summary>
        public int value = 0;

        /// <summary>
        /// 消耗的行動值
        /// </summary>
        public int cost = 0;

        /// <summary>
        /// 命中率，只有攻擊類型的卡牌有用
        /// </summary>
        public float hitRate = 1f;

        /// <summary>
        /// 爆擊對象，當對手出的對峙卡在清單中，則我方的卡片效果x2
        /// </summary>
        public List<CardType> criticleCards = new List<CardType>();

        /// <summary>
        /// 無效對象，當對手出的對峙卡在清單中，則我方的卡片效果完全失效
        /// </summary>
        public List<CardType> invalidCards = new List<CardType>();

        /// <summary>
        /// 被減傷對象，當對手出的對峙卡在清單中，則我方的卡片效果會與對方的卡片數值相減，同時消耗對方的耐力
        /// </summary>
        public List<CardType> halfCards = new List<CardType>();

        /// <summary>
        /// 當卡片被減傷時，消耗對方的耐力值
        /// </summary>
        public int resistDamage = 1;
    }
}