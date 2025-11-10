using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class Config
    {
        public static Dictionary<ResourcesType, string> UIResoucesPath => new Dictionary<ResourcesType, string>()
        {
            {ResourcesType.UI, "CardBattle/UI/CardBattleUI"},
            {ResourcesType.HoldingCard, "CardBattle/UI/HoldingCard"},
            {ResourcesType.SubmitCard, "CardBattle/UI/SubmitCard"},
        };

        public static Dictionary<CardType, CardData> Card => new Dictionary<CardType, CardData>()
        {
            { CardType.None, new CardData() },

            { CardType.Attack, new CardData() {
                category = CardCategory.Attack,
                value = 2,
                cost = 2,
                hitRate = 1,
                halfCards = new List<CardType>() { CardType.Defence },
                criticleCards = new List<CardType>() { CardType.TakeBreak },
                invalidCards = new List<CardType>() { CardType.Avoid },
                resistDamage = 1,
            }},
            { CardType.Defence, new CardData() {
                value = 1,
                cost = 1,
                resistDamage = 1,
            }},
            { CardType.TakeBreak, new CardData() {
                value = 5,
                cost = 2,
            }},
            { CardType.Avoid, new CardData() {
                value = 1,
                cost = 2,
            }},
        };
    }

}
