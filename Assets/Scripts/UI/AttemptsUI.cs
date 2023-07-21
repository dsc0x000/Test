using System.Security;
using UnityEngine;
using UnityEngine.UI;

public sealed class AttemptsUI : MonoBehaviour, IUIHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private Image cellTemplate;

    private void UpdateUI()
    {
        foreach (Transform cell in container)
        {
            if (cell == cellTemplate.transform)
                continue;

            Destroy(cell.gameObject);
        }

        for (int i = 0, j = 0; i < TaskManager.Instance.AttemptsTotal; i++)
        {
            Image cell = Instantiate(cellTemplate, container);
            cell.gameObject.SetActive(true);

            if (j < TaskManager.Instance.AttemptsRemaining)
            {
                cell.color = Color.white;
                j++;
            }
            else
            {
                cell.color = Color.red;
            }
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
        UpdateUI();
        UIManager.Instance.AttemptsUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        UIManager.Instance.AttemptsUpdate -= UpdateUI;
    }
}
