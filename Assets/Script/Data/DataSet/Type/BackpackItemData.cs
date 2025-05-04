using System.Collections.Generic;
using UnityEngine;

public enum BackpackItemCategory
{
    None,
    Normal,
}

public class BackpackItemData
{
    /// <summary>
    /// 道具ID，可用於多語系
    /// </summary>
    public string id = "";

    /// <summary>
    /// 道具類別，可同時屬於複數類別
    /// </summary>
    public List<BackpackItemCategory> categories = new List<BackpackItemCategory>();

    /// <summary>
    /// 買價
    /// </summary>
    public int buyPrice = 0;

    /// <summary>
    /// 賣價
    /// </summary>
    public int sellPrice = 0;
}
