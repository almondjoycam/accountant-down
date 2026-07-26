using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreEntry : MonoBehaviour
{
    private UniversalOverlayScript stats;
    private Button buyButton;
    private StoreItem item = null;

    void Start()
    {
        buyButton = GetComponentInChildren<Button>();
        buyButton.onClick.AddListener(AttemptPurchase);
        stats = FindAnyObjectByType<UniversalOverlayScript>();
    }

    public void SetItem(StoreItem item)
    {
        this.item = item;
        transform.Find("Title").GetComponent<TextMeshProUGUI>().text = item.itemName;
        transform.Find("Description").GetComponent<TextMeshProUGUI>().text = item.itemDescription;
        GetComponentInChildren<Image>().sprite = item.image;
        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.cost:2f}";
        if (stats.GetMoney() < item.cost)
        {
            buyButton.interactable = false;
        }
    }

    void AttemptPurchase()
    {
        stats.ChangeMoney(-item.cost);
    }
}
