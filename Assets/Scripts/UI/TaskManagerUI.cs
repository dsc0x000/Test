using UnityEngine;

public sealed class TaskManagerUI : MonoBehaviour, IUIHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private TaskCellUI cellTemplate;

    private void UpdateUI()
    {
        foreach (Transform cell in container)
        {
            if (cell == cellTemplate.transform)
                continue;

            Destroy(cell.gameObject);
        }

        foreach (var item in TaskManager.Instance.GetRequiredItemList())
        {
            if (item.Value <= 0)
                continue;

            TaskCellUI cellUI = Instantiate(cellTemplate, container);
            cellUI.gameObject.SetActive(true);

            ItemData data = ItemDataProvider.Instance.GetData(item.Key);

            cellUI.SetIcon(data.icon);
            cellUI.SetCount(item.Value);
        }
    }

    public void UpdateVisibility(GameState state)
    {
        if (state == GameState.Playing)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        UIManager.Instance.TaskUpdate += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        UIManager.Instance.TaskUpdate -= UpdateUI;
    }
}
