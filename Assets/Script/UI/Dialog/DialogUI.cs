using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace KBT
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Text nameText;
        [SerializeField] private TextPrinter textPrinter;
        [SerializeField] private RectTransform avatarRect;

        private CanvasGroup canvasGroup => GetComponent<CanvasGroup>();

        private Dictionary<string, DialogCharacter> characterMapping = new Dictionary<string, DialogCharacter>();
        private bool isFadeIn = false;
        private bool isFadeOut = false;
        private bool waitForClick = false;
        private bool isShacking = false;
        private float shackTimer = 0;
        private float shackTime = 0.5f;
        private float alpha = 0;
        private Vector3 tmpPosition = Vector3.zero;

        private int test = 0;

        void Awake()
        {
            textPrinter.onPrintEnd.AddListener(onPrintEnd);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Auth("AI");
            Text($"這是一段很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長很長的文字", 3);
            Character("Test", characterSprite, new List<float>() { 0.8f, 0.5f });
        }

        // Update is called once per frame
        void Update()
        {
            if (isFadeIn)
            {
                alpha += Time.deltaTime;
                canvasGroup.alpha = alpha;
                if (alpha < 1)
                {
                    isFadeIn = false;
                }
            }

            if (isFadeOut)
            {
                alpha -= Time.deltaTime;
                canvasGroup.alpha = alpha;
                if (alpha > 0)
                {
                    isFadeOut = false;
                    gameObject.SetActive(false);
                }
            }

            if (isShacking)
            {
                transform.position = tmpPosition + new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));
                if (Time.time - shackTimer >= shackTime)
                {
                    transform.position = tmpPosition;
                    isShacking = false;
                }
            }

            if (waitForClick)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (test == 0)
                    {
                        waitForClick = false;
                        ShackCharacter("Test", 0.3f);
                        Text($"這是五個字", 3);
                        Character("Test", characterSprite, new List<float>() { 0.5f, 0.5f });
                    }
                    else if (test >= 1)
                    {
                        ShackUI(0.3f);
                    }
                    test++;
                }
            }

        }


        public void FadeIn()
        {
            alpha = 0;
            canvasGroup.alpha = alpha;
            isFadeIn = true;
            gameObject.SetActive(true);
        }

        public void FadeOut()
        {
            isFadeOut = true;
        }

        public void Character(string name, Sprite sprite, List<float> positionRatio)
        {
            if (characterMapping.ContainsKey(name))
            {
                characterMapping[name].spriteRenderer.sprite = sprite;

                Vector3 targetPosition = new Vector3(
                    Screen.width / characterMapping[name].spriteRenderer.sprite.pixelsPerUnit * (positionRatio[0] - 0.5f),
                    Screen.height / characterMapping[name].spriteRenderer.sprite.pixelsPerUnit * (positionRatio[1] - 0.5f)
                );

                characterMapping[name].MoveTo(targetPosition);
            }
            else
            {
                GameObject newObject = new GameObject(name);
                newObject.transform.SetParent(null);
                SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
                DialogCharacter dialogCharacter = newObject.AddComponent<DialogCharacter>();
                spriteRenderer.sprite = sprite;
                characterMapping.Add(name, dialogCharacter);

                Vector3 targetPosition = new Vector3(
                    Screen.width / characterMapping[name].spriteRenderer.sprite.pixelsPerUnit * (positionRatio[0] - 0.5f),
                    Screen.height / characterMapping[name].spriteRenderer.sprite.pixelsPerUnit * (positionRatio[1] - 0.5f)
                );
                characterMapping[name].FadeIn(targetPosition);
            }
        }

        public void ShackCharacter(string name, float _shackTime)
        {
            characterMapping[name].Shack(_shackTime);
        }

        public void ShackUI(float _shackTime)
        {
            if (isShacking) return;
            characterMapping.Keys.ToList().ForEach(charName =>
            {
                ShackCharacter(charName, _shackTime);
            });
            shackTime = _shackTime;
            tmpPosition = transform.position;
            shackTimer = Time.time;
            isShacking = true;
        }

        public void Auth(string _auth)
        {
            nameText.text = _auth;
        }

        public void Text(string _text)
        {
            textPrinter.PlayLine(_text);
        }

        public void Text(string _text, float inSeconds)
        {
            textPrinter.PlayLine(_text, inSeconds);
        }

        private void onPrintEnd()
        {
            waitForClick = true;
        }
    }
}