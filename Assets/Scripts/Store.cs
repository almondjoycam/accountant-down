using UnityEngine;

public class Store : MonoBehaviour
{
    private object[] storeItems;
    [SerializeField] RectTransform storeParent;
    [SerializeField] StoreEntry entryPrefab;
    StoreEntry currentEntry;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storeItems = Resources.LoadAll("", typeof(StoreItem));
        for (int i = 0; i < storeItems.Length; i++)
        {
            StoreItem storeItem = (StoreItem) storeItems[i];
            currentEntry = Instantiate(entryPrefab, storeParent);
            currentEntry.SetItem(storeItem);
        }
    }
}
