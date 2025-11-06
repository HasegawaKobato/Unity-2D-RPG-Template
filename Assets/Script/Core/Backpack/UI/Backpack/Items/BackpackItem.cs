using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BackpackSystem
{
    public class BackpackItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text countText;

        private ItemType itemType = ItemType.None;

        public ItemType ItemType => itemType;

        public void OnClick()
        {
            Main.ActiveBackpackItem(this);
        }

        private void onSelected()
        {
            Main.SetSelectedBackpackItem(this);
        }

        private void onDeselected()
        {
            Main.SetSelectedBackpackItem(null);
        }

        public void Init(ItemType _itemType)
        {
            itemType = _itemType;
            // int count = OwnItemController.GetCount(itemType);
            // TODO: 放上道具圖
            // Sprite sprite = ResourcesController.GetItem(itemType, ItemSizeType.Size48);
            // icon.sprite = sprite;
            // nameText.text = OwnItemController.GetI18nName(itemType);
            // countText.text = $"{count}";
        }
    }
}