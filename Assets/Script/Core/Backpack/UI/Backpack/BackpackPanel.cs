using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BackpackSystem
{
    public class BackpackPanel : BasePanel
    {
        [SerializeField] private RectTransform itemContent;
        [SerializeField] private RectTransform itemCategoryContent;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text ownItemText;
        [SerializeField] private Image bigIcon;

        // private string categoryType = ItemCategoryType.Consume.ToString();
        private List<string> categories = new List<string>();

        private List<GameObject> pool = new List<GameObject>();
        private List<GameObject> categoryPool = new List<GameObject>();

        void Awake()
        {
        }

        public override void Open(params object[] args)
        {
            base.Open(args);
            // categoryType = ItemCategoryType.Consume.ToString();

            // moneyText.text = DataController.gameRecord.money.ToString();

            // InputController.InputActions.UI.Next.performed += onClickNext;
            // InputController.InputActions.UI.Previous.performed += onClickPrevious;

            categories = new List<string>();
            // Enum.GetNames(typeof(ItemCategoryType)).ToList().ForEach(name =>
            // {
            //     if (name != ItemCategoryType.None.ToString())
            //     {
            //         if (name.Contains("_"))
            //         {
            //             if (!categories.Contains(name.Split("_")[0]))
            //             {
            //                 categories.Add(name.Split("_")[0]);
            //             }
            //         }
            //         else
            //         {
            //             if (!categories.Contains(name))
            //             {
            //                 categories.Add(name);
            //             }
            //         }
            //     }
            // });
            updateCategory();
            updateItems();
        }

        public override void Close()
        {
            base.Close();
            // InputController.InputActions.UI.Next.performed -= onClickNext;
            // InputController.InputActions.UI.Previous.performed -= onClickPrevious;
        }

        public void OnCategoryChanged(string categoryStr)
        {
            // categoryType = categoryStr;
            updateItems();
        }

        public void UpdateSelectedItem()
        {
            if (Main.SelectedBackpackItem != null)
            {
                bigIcon.gameObject.SetActive(true);
                // bigIcon.sprite = ResourcesController.GetItem(selectedBackpackItem.ItemType, ItemSizeType.Size256);
                // nameText.text = OwnItemController.GetI18nName(Main.SelectedBackpackItem.ItemType);
                // descriptionText.text = OwnItemController.GetI18nDescription(Main.SelectedBackpackItem.ItemType);
                // ownItemText.text = $"x {OwnItemController.GetCount(Main.SelectedBackpackItem.ItemType)}";
            }
            else
            {
                bigIcon.gameObject.SetActive(false);
                nameText.text = "";
                descriptionText.text = "";
                ownItemText.text = "";
            }
        }

        private void updateCategory()
        {
            recycleCategoryItem();
            categories.ForEach(category =>
            {
                GameObject newGameObject = getCategoryItem();
                newGameObject.SetActive(true);
                newGameObject.transform.SetParent(itemCategoryContent);
                newGameObject.transform.localScale = Vector3.one;
                ItemCategoryTab newItem = newGameObject.GetComponent<ItemCategoryTab>();
                newItem.Init(category);
            });
        }

        private void updateItems()
        {
            recycleItem();
            // DataController.itemRecord.ownItem.ForEach(item =>
            // {
            //     if (ItemDataSet.data[item.id].type.ToString().Contains(categoryType))
            //     {
            //         GameObject newGameObject = getItem();
            //         newGameObject.SetActive(true);
            //         newGameObject.transform.SetParent(itemContent);
            //         newGameObject.transform.localScale = Vector3.one;
            //         BackpackItem newItem = newGameObject.GetComponent<BackpackItem>();
            //         newItem.Init(item.id);
            //     }
            // });
            if (itemContent.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(itemContent.GetChild(0).gameObject);
            }
            else
            {
                resetInfo();
            }
        }

        private void onClickNext(InputAction.CallbackContext callback)
        {
            // int index = categories.IndexOf(categoryType);
            // int newIndex = index + 1;
            // if (newIndex >= categories.Count)
            // {
            //     newIndex = 0;
            // }
            // OnCategoryChanged(categories[newIndex].ToString());
        }

        private void onClickPrevious(InputAction.CallbackContext callback)
        {
            // int index = categories.IndexOf(categoryType);
            // int newIndex = index - 1;
            // if (newIndex < 0)
            // {
            //     newIndex = categories.Count - 1;
            // }
            // OnCategoryChanged(categories[newIndex].ToString());
        }

        private void resetInfo()
        {
            bigIcon.gameObject.SetActive(false);
            nameText.text = "";
            descriptionText.text = "";
            ownItemText.text = "";
            Main.SetSelectedBackpackItem(null);
        }

        private void recycleItem()
        {
            int count = itemContent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform recycleItem = itemContent.GetChild(0).transform;
                recycleItem.gameObject.SetActive(false);
                pool.Add(recycleItem.gameObject);
                recycleItem.SetParent(null);
            }
        }

        private GameObject getItem()
        {
            GameObject resultObject = null;
            if (pool.Count > 0)
            {
                resultObject = pool[0];
                pool.RemoveAt(0);
                return resultObject;
            }
            else
            {
                // resultObject = InstantiateesourcesController.Get(ResourcesType.BackpackItem));
                return resultObject;
            }
        }

        private void recycleCategoryItem()
        {
            int count = itemCategoryContent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform recycleItem = itemCategoryContent.GetChild(0).transform;
                recycleItem.gameObject.SetActive(false);
                categoryPool.Add(recycleItem.gameObject);
                recycleItem.SetParent(null);
            }
        }

        private GameObject getCategoryItem()
        {
            GameObject resultObject = null;
            if (categoryPool.Count > 0)
            {
                resultObject = categoryPool[0];
                categoryPool.RemoveAt(0);
                return resultObject;
            }
            else
            {
                // resultObject = Instantiate(ResourcesController.Get(ResourcesType.ItemCategoryTab));
                return resultObject;
            }
        }

    }
}