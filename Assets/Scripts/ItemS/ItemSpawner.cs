using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemFactory factory;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (spawnTimer <= 0)
        {
            SpawnRandom();
            spawnTimer = spawnCooldown;
        }

        spawnTimer -= Time.deltaTime;
    }

    private void SpawnRandom()
    {
        ItemID itemID = Item.GetRandomID();
        Item instance = factory.Get(itemID);

        instance.transform.position = spawnpoint.position;
    }
}
