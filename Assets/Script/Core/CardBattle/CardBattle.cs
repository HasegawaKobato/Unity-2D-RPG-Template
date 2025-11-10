using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace CardBattle
{
    public class Main
    {
        public static CardBattleUI UI => ui;
        public static CardBattleRuntime Runtime => runtime;
        public static CardBattleResources Resources => resources;
        public static CardBattleRecord Record => record;

        private static CardBattleUI ui = null;
        private static CardBattleRuntime runtime = new CardBattleRuntime();
        private static CardBattleResources resources = new CardBattleResources();
        private static CardBattleRecord record = new CardBattleRecord();

        public static void Init()
        {
            resources.Init();
            record.Init();
        }

        public static void StartBattle()
        {
            InputController.InputActions.Player.Disable();
            InputController.InputActions.UI.Enable();

            ui = resources.GetUI<CardBattleUI>(ResourcesType.UI);
            runtime.Init();
            ui.Init();
        }

        /// <summary>
        /// 將手牌放進行動佇列
        /// </summary>
        /// <param name="holdingCard"></param>
        public static void Submit(HoldingCard holdingCard)
        {
            int totalCost = 0;
            ui.SubmitCards.ForEach(card => totalCost += Config.Card[card].cost);
            if (Config.Card[holdingCard.Type].cost + totalCost <= runtime.ActionMaxPoint)
            {
                ui.AddSubmitCard(holdingCard.Type);
                GameObject.Destroy(holdingCard.gameObject);
                ui.UpdateActionPoint();
            }
        }

        /// <summary>
        /// 將行動佇列的牌收回到手牌中
        /// </summary>
        /// <param name="submitCard"></param>
        public static void Recall(SubmitCard submitCard)
        {
            ui.AddHoldingCard(submitCard.Type);
            GameObject.DestroyImmediate(submitCard.gameObject);
            ui.UpdateActionPoint();
        }

        /// <summary>
        /// 回合開始，對戰處理
        /// </summary>
        public static async void RoundFight()
        {
            List<CardType> submitCards = new List<CardType>(ui.SubmitCards);
            List<CardType> enemySubmitCards = new List<CardType>(ui.EnemySubmitCards);
            int maxActionCount = Math.Max(submitCards.Count, enemySubmitCards.Count);

            BattleStatus battleStatus = BattleStatus.None;
            for (int i = 0; i < maxActionCount; i++)
            {
                CardType selfAction = CardType.None;
                if (i < submitCards.Count) selfAction = submitCards[i];
                CardType enemyAction = CardType.None;
                if (i < enemySubmitCards.Count) enemyAction = enemySubmitCards[i];

                runtime.ChangeActinoPoint(-Config.Card[selfAction].cost);
                ui.Action();

                /// MEMO: 由我方先攻，判斷優先順序後再由對方攻擊，並以同樣方式判斷
                /// 1. 是否為無效對象
                /// 2. 是否為減傷對象
                /// 3. 是否為爆擊對象
                /// 若為2、3，則骰一次命中率，確定命中後才能給予傷害

                if (Config.Card[selfAction].invalidCards.Contains(enemyAction))
                {
                    // MEMO: 此行動無效，對方也不會有任何損失，直接進入下個行動，也不計入正常使用
                }
                else if (Config.Card[selfAction].halfCards.Contains(enemyAction) && runtime.EnemyStamina > 0)
                {
                    float hit = UnityEngine.Random.Range(0f, 1f);
                    if (hit <= Config.Card[selfAction].hitRate)
                    {
                        // MEMO: 此行動我方攻擊力減半，但會消耗對方耐力
                        runtime.ChangeEnemyHp(-Config.Card[selfAction].value / 2);
                        runtime.ChangeEnemyStamina(-Config.Card[selfAction].resistDamage);
                    }
                    record.Used(selfAction);
                }
                else if (Config.Card[selfAction].criticleCards.Contains(enemyAction))
                {
                    float hit = UnityEngine.Random.Range(0f, 1f);
                    if (hit <= Config.Card[selfAction].hitRate)
                    {
                        // MEMO: 此行動我方攻擊力2倍
                        runtime.ChangeEnemyHp(-Config.Card[selfAction].value * 2);
                    }
                    record.Used(selfAction);
                }
                else
                {
                    if (Config.Card[selfAction].category == CardCategory.Attack)
                    {
                        float hit = UnityEngine.Random.Range(0f, 1f);
                        if (hit <= Config.Card[selfAction].hitRate)
                        {
                            runtime.ChangeEnemyHp(-Config.Card[selfAction].value);
                        }
                    }
                    record.Used(selfAction);
                }

                ui.UpdateInfo();
                battleStatus = runtime.GetBattleStatus();
                if (battleStatus != BattleStatus.None) break;

                if (Config.Card[enemyAction].invalidCards.Contains(selfAction))
                {
                    // MEMO: 此行動無效，對方也不會有任何損失，直接進入下個行動
                }
                else if (Config.Card[enemyAction].halfCards.Contains(selfAction) && runtime.EnemyStamina > 0)
                {
                    float hit = UnityEngine.Random.Range(0f, 1f);
                    if (hit <= Config.Card[enemyAction].hitRate)
                    {
                        // MEMO: 此行動我方攻擊力減半，但會消耗對方耐力
                        runtime.ChangeHp(-Config.Card[enemyAction].value / 2);
                        runtime.ChangeStamina(-Config.Card[enemyAction].resistDamage);
                    }
                }
                else if (Config.Card[enemyAction].criticleCards.Contains(selfAction))
                {
                    float hit = UnityEngine.Random.Range(0f, 1f);
                    if (hit <= Config.Card[enemyAction].hitRate)
                    {
                        // MEMO: 此行動我方攻擊力2倍
                        runtime.ChangeHp(-Config.Card[enemyAction].value * 2);
                    }
                }
                else
                {
                    if (Config.Card[enemyAction].category == CardCategory.Attack)
                    {
                        float hit = UnityEngine.Random.Range(0f, 1f);
                        if (hit <= Config.Card[enemyAction].hitRate)
                        {
                            runtime.ChangeHp(-Config.Card[enemyAction].value);
                        }
                    }
                }

                ui.UpdateInfo();
                battleStatus = runtime.GetBattleStatus();
                if (battleStatus != BattleStatus.None) break;

                await Task.Delay(1000);
            }

            ActionOver(battleStatus);
        }

        public static void ActionOver(BattleStatus battleStatus)
        {
            switch (battleStatus)
            {
                case BattleStatus.None:
                    runtime.RefreshCard();
                    ui.UpdateCards();
                    break;
                case BattleStatus.Win:
                    break;
                case BattleStatus.Lose:
                    break;
            }
        }

    }
}