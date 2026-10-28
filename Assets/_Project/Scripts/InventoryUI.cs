using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class InventoryTab
    {
        public Image button;
        public GameObject panel;

        [Header("Sprites")]
        public Sprite normalSprite;
        public Sprite selectedSprite;
    }

    [Header("Tabs")]
    [SerializeField] private InventoryTab backpackTab;
    [SerializeField] private InventoryTab equipmentTab;
    [SerializeField] private InventoryTab craftTab;
    [SerializeField] private InventoryTab catchTab;
    [SerializeField] private InventoryTab journalTab;

    private void Start()
    {
        OpenBackpack();
    }

    public void OpenInventory()
    {
        gameObject.SetActive(true);
        OpenBackpack();
    }

    public void OpenBackpack()
    {
        SelectTab(backpackTab);
    }

    public void OpenEquipment()
    {
        SelectTab(equipmentTab);
    }

    public void OpenCraft()
    {
        SelectTab(craftTab);
    }

    public void OpenCatch()
    {
        SelectTab(catchTab);
    }
    public void OpenJournal()
    {
        SelectTab(journalTab);
    }

    private void SelectTab(InventoryTab selectedTab)
    {
        InventoryTab[] tabs =
        {
            backpackTab,
            equipmentTab,
            craftTab,
            catchTab,
            journalTab
        };

        foreach (InventoryTab tab in tabs)
        {
            tab.panel.SetActive(false);
            tab.button.sprite = tab.normalSprite;
        }

        selectedTab.panel.SetActive(true);
        selectedTab.button.sprite = selectedTab.selectedSprite;
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }
}