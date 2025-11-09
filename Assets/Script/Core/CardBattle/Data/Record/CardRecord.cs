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

    }

}
