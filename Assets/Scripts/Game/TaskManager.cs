using System;
using System.Collections.Generic;
using System.Linq;

public class TaskManager : Singleton<TaskManager>, IDisposable
{
    public event Action StoredCorrectItem, StoredWrongItem;
    public event Action TaskGenerated, TaskComplete, TaskFailed;

    private Player player;
    private int minItemToCollectCount;
    private int maxItemToCollectCount;

    public int AttemptsTotal { get; private set; }
    public int AttemptsRemaining { get; private set; }

    private Dictionary<ItemID, int> itemsToCollect;
    private bool isDisposed;

    public void Initialize(Properties props)
    {
        minItemToCollectCount = props.minItemToCollectCount;
        maxItemToCollectCount = props.maxItemToCollectCount;
        AttemptsTotal = props.attempts;
        player = props.player;

        itemsToCollect = new Dictionary<ItemID, int>();

        ItemID[] itemIDs = (ItemID[])Enum.GetValues(typeof(ItemID));
        foreach (var itemID in itemIDs)
            itemsToCollect.Add(itemID, 0);

        player.ItemStored += Player_OnItemStored;
        GameManager.Instance.GameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GenerateNewTask()
    {
        Reset();

        ItemID[] possibleItems = (ItemID[])Enum.GetValues(typeof(ItemID));
        ItemID[] targetItems = new ItemID[UnityEngine.Random.Range(0, possibleItems.Length) + 1];
        int itemsToCollectCount = UnityEngine.Random.Range(minItemToCollectCount, maxItemToCollectCount + 1);

        int itemSum = 0;
        for (int i = 0; i < targetItems.Length - 1; i++)
        {
            ItemID randomItem = possibleItems[UnityEngine.Random.Range(0, possibleItems.Length)];

            bool alreadyExist = false;
            for (int j = 0; j < targetItems.Length; j++)
            {
                if (randomItem == targetItems[j])
                    alreadyExist = true;
            }

            if (alreadyExist)
            {
                i--;
                continue;
            }

            int itemToCollectCount = UnityEngine.Random.Range(1, itemsToCollectCount - itemSum + 1);
            itemsToCollect[randomItem] = itemToCollectCount;
            itemSum += itemToCollectCount;
        }

        itemsToCollect[targetItems[targetItems.Length - 1]] = itemsToCollectCount - itemSum;

        TaskGenerated?.Invoke();
    }

    private void Reset()
    {
        AttemptsRemaining = AttemptsTotal;

        foreach (var itemID in itemsToCollect.Keys.ToArray())
            itemsToCollect[itemID] = 0;
    }

    public Dictionary<ItemID, int> GetRequiredItemList()
    {
        return new Dictionary<ItemID, int>(itemsToCollect);
    }

    private void Player_OnItemStored(ItemID itemID)
    {
        if (itemsToCollect[itemID] <= 0)
        {
            AttemptsRemaining--;
            StoredWrongItem?.Invoke();
        }
        else
        {
            itemsToCollect[itemID]--;
            StoredCorrectItem?.Invoke();
        }

        CheckTask();
    }

    private void CheckTask()
    {
        if (AttemptsRemaining <= 0)
        {
            TaskFailed?.Invoke();
            return;
        }

        bool isIncomplete = false;
        foreach (var item in itemsToCollect)
        {
            if (item.Value > 0)
                isIncomplete = true;
        }

        if (isIncomplete == false)
            TaskComplete?.Invoke();
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
            GenerateNewTask();
    }

    public void Dispose()
    {
        if (isDisposed)
            return;

        player.ItemStored -= Player_OnItemStored;
        GameManager.Instance.GameStateChanged -= GameManager_OnGameStateChanged;
        isDisposed = true;
    }

    public struct Properties
    {
        public int minItemToCollectCount;
        public int maxItemToCollectCount;
        public int attempts;
        public Player player;
    }
}