using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattle
{
    public class CardBattleUI : MonoBehaviour
    {
        public int SubmitCount => submitContent.childCount;
        public int HoldingCount => holdingContent.childCount;
        public List<CardType> SubmitCards
        {
            get
            {
                List<CardType> cards = new List<CardType>();
                for (int i = 0; i < submitContent.childCount; i++)
                {
                    cards.Add(submitContent.GetChild(i).GetComponent<SubmitCard>().Type);
                }
                return cards;
            }
        }
        public List<CardType> EnemySubmitCards
        {
            get
            {
                List<CardType> cards = new List<CardType>();
                for (int i = 0; i < enemySubmitContent.childCount; i++)
                {
                    cards.Add(enemySubmitContent.GetChild(i).GetComponent<SubmitCard>().Type);
                }
                return cards;
            }
        }

        [SerializeField] private RectTransform enemySubmitContent;
        [SerializeField] private RectTransform submitContent;
        [SerializeField] private RectTransform holdingContent;
        [SerializeField] private Slider enemyHPSlider;
        [SerializeField] private Slider enemyStaminaSlider;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private TMP_Text enemyHPText;
        [SerializeField] private TMP_Text enemyStaminaText;
        [SerializeField] private TMP_Text actionPointText;
        [SerializeField] private TMP_Text staminaText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text descriptionText;

        private List<GameObject> submitPool = new List<GameObject>();
        private List<GameObject> holdingPool = new List<GameObject>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            UpdateInfo();
            updateCards();
        }

        /// <summary>
        /// 確認行動
        /// </summary>
        public void OnClickConfirm()
        {
            Main.RoundFight();
        }

        public void AddHoldingCard(CardType cardType)
        {
            GameObject newGameObject = getHoldingCard();
            newGameObject.SetActive(true);
            newGameObject.transform.SetParent(holdingContent);
            newGameObject.transform.localScale = Vector3.one;
            HoldingCard newItem = newGameObject.GetComponent<HoldingCard>();
            newItem.Init(cardType);
        }

        public void AddSubmitCard(CardType cardType)
        {
            GameObject newGameObject = getSubmitCard();
            newGameObject.SetActive(true);
            newGameObject.transform.SetParent(submitContent);
            newGameObject.transform.localScale = Vector3.one;
            newGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            SubmitCard newItem = newGameObject.GetComponent<SubmitCard>();
            newItem.Init(cardType);
        }

        public void AddEnemySubmitCard(CardType cardType)
        {
            GameObject newGameObject = getSubmitCard();
            newGameObject.SetActive(true);
            newGameObject.transform.SetParent(enemySubmitContent);
            newGameObject.transform.localScale = Vector3.one;
            newGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            SubmitCard newItem = newGameObject.GetComponent<SubmitCard>();
            newItem.Init(cardType);
        }

        public void ActionOver()
        {
            if (submitContent.childCount > 0)
            {
                Destroy(submitContent.GetChild(0).gameObject);
            }
            if (enemySubmitContent.childCount > 0)
            {
                Destroy(enemySubmitContent.GetChild(0).gameObject);
            }
        }

        public void UpdateInfo()
        {
            enemyHPSlider.maxValue = Main.Runtime.EnemyMaxHp;
            enemyHPSlider.value = Main.Runtime.EnemyHp;
            enemyStaminaSlider.maxValue = Main.Runtime.EnemyMaxStamina;
            enemyStaminaSlider.value = Main.Runtime.EnemyStamina;
            hpSlider.maxValue = Main.Runtime.MaxHp;
            hpSlider.value = Main.Runtime.Hp;
            staminaSlider.maxValue = Main.Runtime.MaxStamina;
            staminaSlider.value = Main.Runtime.Stamina;

            enemyHPText.text = $"{Main.Runtime.EnemyHp}";
            enemyStaminaText.text = $"{Main.Runtime.EnemyStamina}";
            hpText.text = $"{Main.Runtime.Hp}";
            staminaText.text = $"{Main.Runtime.Stamina}";
            actionPointText.text = $"{Main.Runtime.ActionPoint}";
            descriptionText.text = "";
        }

        private void updateCards()
        {
            recycleSubmitCard();
            recycleHoldingCard();
            // TODO: 這裡到時候的出牌數量要以紀錄為主
            Main.Runtime.HoldingCard.ForEach(card =>
            {
                AddHoldingCard(card);
            });
            Main.Runtime.EnemyCard.ForEach(card =>
            {
                AddEnemySubmitCard(card);
            });
        }

        private void recycleSubmitCard()
        {
            int count = enemySubmitContent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform recycleCard = enemySubmitContent.GetChild(0).transform;
                recycleCard.gameObject.SetActive(false);
                submitPool.Add(recycleCard.gameObject);
                recycleCard.SetParent(null);
            }

            count = submitContent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform recycleCard = submitContent.GetChild(0).transform;
                recycleCard.gameObject.SetActive(false);
                submitPool.Add(recycleCard.gameObject);
                recycleCard.SetParent(null);
            }
        }

        private GameObject getSubmitCard()
        {
            GameObject resultObject;
            if (submitPool.Count > 0)
            {
                resultObject = submitPool[0];
                submitPool.RemoveAt(0);
                return resultObject;
            }
            else
            {
                resultObject = Instantiate(Main.Get(ResourcesType.SubmitCard));
                return resultObject;
            }
        }

        private void recycleHoldingCard()
        {
            int count = holdingContent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform recycleCard = holdingContent.GetChild(0).transform;
                recycleCard.gameObject.SetActive(false);
                holdingPool.Add(recycleCard.gameObject);
                recycleCard.SetParent(null);
            }
        }

        private GameObject getHoldingCard()
        {
            GameObject resultObject;
            if (holdingPool.Count > 0)
            {
                resultObject = holdingPool[0];
                holdingPool.RemoveAt(0);
                return resultObject;
            }
            else
            {
                resultObject = Instantiate(Main.Get(ResourcesType.HoldingCard));
                return resultObject;
            }
        }

    }
}