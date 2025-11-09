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
            enemyMaxHp = 100;
            enemyHp = 100;
            enemyStamina = 1;
            enemyMaxStamina = 1;

            // TODO: 這裡到時候要讀取角色資料
            actionPoint = 3;
            actionMaxPoint = 3;
            hp = 100;
            maxHp = 100;
            stamina = 1;
            maxStamina = 1;

            // TODO: 這裡到時候要從卡池紀錄中複製一份並隨機排序
            holdingCard = new List<CardType>();
            List<CardType> tmpOwnCard = new List<CardType>()
            {
                CardType.Attack, CardType.Defence, CardType.TakeBreak, CardType.Avoid
            };
            while (tmpOwnCard.Count > 0)
            {
                int index = Random.Range(0, tmpOwnCard.Count);
                holdingCard.Add(tmpOwnCard[index]);
                tmpOwnCard.RemoveAt(index);
            }

            // TODO: 這裡到時候要從卡池紀錄中複製一份並隨機排序
            enemyCard = new List<CardType>();
            List<CardType> tmpEnemyCard = new List<CardType>()
            {
                CardType.Attack, CardType.Defence, CardType.TakeBreak, CardType.Avoid
            };
            while (tmpEnemyCard.Count > 0)
            {
                int index = Random.Range(0, tmpEnemyCard.Count);
                enemyCard.Add(tmpEnemyCard[index]);
                tmpEnemyCard.RemoveAt(index);
            }
        }

        public void ChangeEnemyHp(int delta)
        {
            enemyHp += delta;
        }

        public void ChangeEnemyStamina(int delta)
        {
            enemyStamina += delta;
        }
        
        public void ChangeActinoPoint(int delta)
        {
            actionPoint += delta;
        }
    }
}