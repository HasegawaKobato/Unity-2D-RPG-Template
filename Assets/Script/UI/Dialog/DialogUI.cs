using System.Collections.Generic;
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
        [SerializeField] private DialogContentLayout contentLayout;

        private CanvasGroup canvasGroup => GetComponent<CanvasGroup>();

        private Dictionary<string, SpriteRenderer> characterMapping = new Dictionary<string, SpriteRenderer>();
        private bool isFadeIn = false;
        private bool isFadeOut = false;
        private float alpha = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            textPrinter.PlayLine($"asdgfoijsfovidejr hto pidjk \nfmogeijg mso wr\nekjroighjsoer tijeor\nifgjas dgfoijsf ovidejrhto pidjkfmogeijg msowrekjro ighjsoertijeori fgjasdgfoi jsfovidejrhtopidjkfmogeijgmsowrekjroighjsoertijeorifgjasdgfoijsfovidejrhtopidjkfmogeijgmsowrekjroighjsoertijeorifgj");
            Character("Test", characterSprite, new List<float>() { 1f, 0.5f });
            updateLayout();
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
                characterMapping[name].sprite = sprite;
            }
            else
            {
                GameObject newObject = new GameObject(name);
                newObject.transform.SetParent(null);
                SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
                DialogCharacter dialogCharacter = newObject.AddComponent<DialogCharacter>();
                spriteRenderer.sprite = sprite;
                characterMapping.Add(name, spriteRenderer);
            }
            characterMapping[name].transform.position = new Vector3(
                Screen.width / characterMapping[name].sprite.pixelsPerUnit * (positionRatio[0] - 0.5f),
                Screen.height / characterMapping[name].sprite.pixelsPerUnit * (positionRatio[1] - 0.5f)
            );
            characterMapping[name].GetComponent<DialogCharacter>().FadeIn();
        }

        private void updateLayout()
        {
            switch (contentLayout)
            {
                case DialogContentLayout.None:
                    avatarRect.gameObject.SetActive(false);
                    break;
                case DialogContentLayout.AvatarLeft:
                    avatarRect.gameObject.SetActive(true);
                    avatarRect.SetAsFirstSibling();
                    break;
                case DialogContentLayout.AvatarRight:
                    avatarRect.gameObject.SetActive(true);
                    avatarRect.SetAsLastSibling();
                    break;
            }
        }
    }
}