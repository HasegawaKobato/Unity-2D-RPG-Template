using System.Collections.Generic;
using UnityEngine;

namespace CardBattle
{
    public class CardBattleRuntime
    {
        public int EnemyHp => enemyHp;
        public int EnemyMaxHp => enemyMaxHp;
        public int EnemyStamina => enemyStamina;
        public int EnemyMaxStamina => enemyMaxStamina;
        public int ActionPoint => actionPoint;
        public int ActionMaxPoint => actionMaxPoint;
        public int Hp => hp;
        public int MaxHp => maxHp;
        public int Stamina => stamina;
        public int MaxStamina => maxStamina;
        public List<CardType> EnemyCard => enemyCard;
        public List<CardType> HoldingCard => holdingCard;

        private int enemyHp = 0;
        private int enemyMaxHp = 0;
        private int enemyStamina = 0;
        private int enemyMaxStamina = 0;
        private int actionPoint = 0;
        private int actionMaxPoint = 0;
        private int hp = 0;
        private int maxHp = 0;
        private int stamina = 0;
        private int maxStamina = 0;
        private List<CardType> enemyCard = new List<CardType>();
        private List<CardType> holdingCard = new List<CardType>();

        public void Init()
        {
            // TODO: 這裡到時候要從外側輸入敵人資料進來
            enemyMaxHp = 10;
            enemyHp = 10;
            enemyStamina = 1;
            enemyMaxStamina = 1;

            // TODO: 這裡到時候要讀取角色資料
            actionPoint = 5;
            actionMaxPoint = 5;
            hp = 10;
            maxHp = 10;
            stamina = 1;
            maxStamina = 1;

            RefreshCard();
        }

        public void RefreshCard()
        {
            // MEMO: 目前手牌是從卡池紀錄中複製一份並隨機排序
            holdingCard = new List<CardType>();
            List<CardType> tmpOwnCard = new List<CardType>(Main.Record.OwnCards);
            while (tmpOwnCard.Count > 0 && holdingCard.Count < 3)
            {
                int index = Random.Range(0, tmpOwnCard.Count);
                holdingCard.Add(tmpOwnCard[index]);
                tmpOwnCard.RemoveAt(index);
            }

            // TODO: 這裡到時候要從外部傳入的敵人資料中複製一份並隨機排序
            enemyCard = new List<CardType>();
            List<CardType> tmpEnemyCard = new List<CardType>()
            {
                CardType.Attack, CardType.Defence, CardType.TakeBreak, CardType.Avoid
            };
            while (tmpEnemyCard.Count > 0 && enemyCard.Count < 3)
            {
                int index = Random.Range(0, tmpEnemyCard.Count);
                enemyCard.Add(tmpEnemyCard[index]);
                tmpEnemyCard.RemoveAt(index);
            }
        }

        public void ChangeEnemyHp(int delta)
        {
            enemyHp += delta;
            if (enemyHp < 0) enemyHp = 0;
            if (enemyHp > enemyMaxHp) enemyHp = enemyMaxHp;
        }

        public void ChangeEnemyStamina(int delta)
        {
            enemyStamina += delta;
            if (enemyStamina < 0) enemyStamina = 0;
            if (enemyStamina > enemyMaxStamina) enemyStamina = enemyMaxStamina;
        }

        public void ChangeHp(int delta)
        {
            hp += delta;
            if (hp < 0) hp = 0;
            if (hp > MaxHp) hp = MaxHp;
        }

        public void ChangeStamina(int delta)
        {
            stamina += delta;
            if (stamina < 0) stamina = 0;
            if (stamina > MaxStamina) stamina = MaxStamina;
        }

        public void ChangeActinoPoint(int delta)
        {
            actionPoint += delta;
            if (actionPoint < 0) actionPoint = 0;
            if (actionPoint > actionMaxPoint) actionPoint = actionMaxPoint;
        }
        
        public BattleStatus GetBattleStatus()
        {
            if (enemyHp <= 0) return BattleStatus.Win;
            if (hp <= 0) return BattleStatus.Lose;
            return BattleStatus.None;
        }

    }
}