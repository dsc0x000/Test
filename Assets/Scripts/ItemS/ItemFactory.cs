using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "ScriptableObjects/ItemFactory")]
public class ItemFactory : ScriptableObject
{
    [SerializeField] private Item[] itemPrefabs;

    public Item Get(ItemID itemID)
    {
        foreach (var prefab in itemPrefabs)
            if (prefab.ID == itemID)
                return Instantiate(prefab);

        Debug.LogError($"There's no prefab with ID {itemID}!");
        return null;
    }
}
