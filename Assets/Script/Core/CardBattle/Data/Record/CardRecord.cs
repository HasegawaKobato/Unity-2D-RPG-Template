using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class CardRecord
    {
        public List<CardDataRecord> baseData = new List<CardDataRecord>();

        public List<CardType> ownCards = new List<CardType>();
        public int maxSubmitCard = 3;
        public int maxHoldingCard = 5;

        public CardRecord() { }
        
        public CardRecord(CardRecord record)
        {
            baseData = new List<CardDataRecord>(record.baseData);
            ownCards = new List<CardType>(record.ownCards);
            maxSubmitCard = record.maxSubmitCard;
            maxHoldingCard = record.maxHoldingCard;
        }

    }

}
