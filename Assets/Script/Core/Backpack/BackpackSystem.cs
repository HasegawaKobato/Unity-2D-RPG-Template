using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BackpackSystem
{
    /// <summary>
    /// TODO: 之後可能會把「背包」、「置物箱」一起放在這裡，但目前只有背包
    /// </summary>
    public class Main
    {
        public static BackpackItem SelectedBackpackItem => selectedBackpackItem;
        public static BackpackItem BackpackItem => activeBackpackItem;
        public static BackpackPanelState PanelState => panelState;

        private static BackpackPanelState panelState = BackpackPanelState.None;

        private static BackpackItem activeBackpackItem = null;
        private static BackpackItem selectedBackpackItem = null;
        private static BackpackPanel backpackPanel = null;

        public static void Init()
        {
            // backpackPanel = ResourcesController.GetUI<BackpackPanel>(ResourcesType.BackpackPanel, DataController.uiTransform);
            backpackPanel.gameObject.SetActive(false);
            panelState = BackpackPanelState.None;
            // InputController.InputActions.UI.Cancel.performed += onClickCancel;
            // InputController.InputActions.UI.Next.performed += onClickNext;
            // InputController.InputActions.UI.Previous.performed += onClickPrevious;
        }

        public static async Task Backpack(BackpackPanelState _panelState)
        {
            panelState = _panelState;
            activeBackpackItem = null;
            InputController.InputActions.Player.Disable();
            InputController.InputActions.UI.Enable();
            await backpackPanel.ShowAsync();
        }

        public static void SetSelectedBackpackItem(BackpackItem item)
        {
            selectedBackpackItem = item;
            backpackPanel.UpdateSelectedItem();
        }

        public static void ActiveBackpackItem(BackpackItem item)
        {
            if (!backpackPanel.canAnimation) return;
            activeBackpackItem = item;
            backpackPanel.Close();
        }

        /// <summary>
        /// 使用當前選擇的道具到自己上
        /// </summary>
        public static void UseItemForSelf()
        {
            // UseItemProcess.Use(activeBackpackItem.ItemType);
        }

        private static async void onClickCancel(InputAction.CallbackContext callback)
        {
            if (!backpackPanel.gameObject.activeSelf || !backpackPanel.canAnimation) return;
            // await backpackPanel.CloseAsync();
            switch (panelState)
            {
                case BackpackPanelState.View:
                    InputController.InputActions.Player.Enable();
                    InputController.InputActions.UI.Disable();
                    break;
                case BackpackPanelState.Battle:
                    break;
            }
        }

        private static void onClickNext(InputAction.CallbackContext callback)
        {
        }

        private static void onClickPrevious(InputAction.CallbackContext callback)
        {
        }

    }

}
