using UnityEngine;

[CreateAssetMenu(fileName = "StoreItem", menuName = "Scriptable Objects/StoreItem")]
public class StoreItem : ScriptableObject
{
    public Sprite image;
    public float cost;
    public string itemName;
    public string itemDescription;
}
