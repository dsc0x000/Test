using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataProvider : Singleton<ItemDataProvider>
{
    private string dataPath = "ItemData";
    private Dictionary<ItemID, ItemData> itemData;

    public void InitializeAndLoad()
    {
        itemData = new Dictionary<ItemID, ItemData>();

        foreach (var data in Resources.LoadAll<ItemData>(dataPath))
            itemData.Add(data.id, data);
    }

    public ItemData GetData(ItemID id)
    {
        if (itemData.TryGetValue(id, out var data))
            return data;

        throw new InvalidOperationException($"Data for item ID {id} does not exist!");
    }
}