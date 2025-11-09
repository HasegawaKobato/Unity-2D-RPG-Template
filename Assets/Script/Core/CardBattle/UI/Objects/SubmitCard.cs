using TMPro;
using UnityEngine;

namespace CardBattle
{
    public class SubmitCard : MonoBehaviour
    {
        public CardType Type => type;

        [SerializeField] private TMP_Text nameText;

        private CardType type = CardType.None;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(CardType _type)
        {
            type = _type;
            nameText.text = type.ToString();
        }

        public void OnClick()
        {
            Main.Recall(this);
        }
    }
}