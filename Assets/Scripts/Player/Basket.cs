using System;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public event Action<ItemID> ItemStored;

    [SerializeField] private Transform[] itemSlots;
    private List<Item> storedItems = new List<Item>();

    public Vector3 GetFreeSlotPosition()
    {
        if (storedItems.Count >= itemSlots.Length)
        {
            Debug.LogWarning("All unique slots are occupied!");
            return itemSlots[0].position;
        }
            
        return itemSlots[storedItems.Count].position;
    }

    public void StoreItem(Item itemToStore)
    {
        if (storedItems.Count >= itemSlots.Length)
        {
            Debug.LogWarning("All unique slots are occupied!");
            itemToStore.transform.SetParent(itemSlots[0]);
        }
        else
        {
            itemToStore.transform.SetParent(itemSlots[storedItems.Count]);
        }

        itemToStore.transform.localPosition = Vector3.zero;
        storedItems.Add(itemToStore);

        ItemStored?.Invoke(itemToStore.ID);
    }

    public void Clear()
    {
        foreach (var item in storedItems)
        {
            if (item.gameObject != null)
                Destroy(item.gameObject);
        }
             
        storedItems.Clear();
    }
}