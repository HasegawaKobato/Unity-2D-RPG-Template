using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class CardBattleRecord
    {
        public List<CardDataRecord> BaseData => cardRecord.baseData;
        public List<CardType> OwnCards => cardRecord.ownCards;
        public int MaxSubmitCard => cardRecord.maxSubmitCard;
        public int MaxHoldingCard => cardRecord.maxHoldingCard;

        private CardRecord cardRecord = new CardRecord();

        public void Init()
        {
            initCardRecord();
        }

        public void Load(CardRecord record)
        {
            initCardRecord(record);
        }

        public void Used(CardType cardType)
        {
            if (cardType == CardType.None) return;
            CardDataRecord dataRecord = cardRecord.baseData.Find(card => card.type == cardType);
            if (dataRecord == null)
            {
                dataRecord = new CardDataRecord() { type = cardType };
            }
            dataRecord.proficiency++;
        }

        private void initCardRecord(CardRecord record = null)
        {
            if (record == null)
            {
                cardRecord = new CardRecord();
            }
            else
            {
                cardRecord = new CardRecord(record);
            }
        }
    }
}