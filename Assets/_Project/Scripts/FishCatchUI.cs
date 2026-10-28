using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FishCatchUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fishIcon;
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text fishDescription;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        gameObject.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    public void ShowFish(ItemData fish)
    {
        if (fish == null)
            return;

        fishIcon.sprite = fish.icon;
        fishIcon.enabled = fish.icon != null;

        fishName.text = fish.itemName;

        fishDescription.text = fish.description;

        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}