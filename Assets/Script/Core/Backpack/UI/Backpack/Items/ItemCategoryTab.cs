using TMPro;
using UnityEngine;

public class ItemCategoryTab : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string itemCategory)
    {
        // nameText.text = LocalizationController.GetValue($"ItemCategoryType.{itemCategory}");
    }
}
