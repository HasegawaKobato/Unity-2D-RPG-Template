using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class CardDataRecord
    {
        public CardType type = CardType.None;

        /// <summary>
        /// 該卡牌種類的等級，可以用熟練度升級
        /// </summary>
        public int level = 1;

        /// <summary>
        /// 熟練度，成功使用一次就會+1
        /// </summary>
        public int proficiency = 0;

        /// <summary>
        /// 特訓點數，額外加乘的值，可用來提升卡牌威力。
        /// </summary>
        public int trainPoint = 0;
    }
}
