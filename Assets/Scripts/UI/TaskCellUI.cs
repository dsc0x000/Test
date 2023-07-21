using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskCellUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetIcon(Sprite icon) => this.icon.sprite = icon;
    public void SetCount(int count) => countText.text = count.ToString();
}
