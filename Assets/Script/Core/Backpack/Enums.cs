
namespace BackpackSystem
{
    /// <summary>
    /// 這次開啟背包的用途
    /// </summary>
    public enum BackpackPanelState
    {
        None,

        /// <summary>
        /// 戰鬥用，可以使用道具（只有允許戰鬥時使用的道具才能用）
        /// </summary>
        Battle,

        /// <summary>
        /// 檢視用，可以使用道具（只有允許地圖行動時使用的道具才能用）
        /// </summary>
        View,
    }

    public enum ItemType
    {
        None
    }
}