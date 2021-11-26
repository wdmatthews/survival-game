using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Upgrade Window")]
    [DisallowMultipleComponent]
    public class UpgradeWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _upgradeItemListElementTemplate = null;
        [SerializeField] private VisualTreeAsset _ingredientListElementTemplate = null;
        [SerializeField] private InventorySO _inventory = null;

        private UpgradeWindowItemsList _itemsList = null;
        private UpgradeWindowDescriptionPanel _descriptionPanel = null;
        private UpgradeWindowUpgradePanel _upgradePanel = null;
        public bool IsOpen { get; private set; } = false;
        public System.Action<UpgradableItemSO> UpgradeItem { set => _upgradePanel.OnUpgradeItem = value; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _itemsList = new UpgradeWindowItemsList(rootVisualElement.Q<ListView>("ItemsList"),
                _upgradeItemListElementTemplate, SelectListElement, _inventory);
            _descriptionPanel = new UpgradeWindowDescriptionPanel(rootVisualElement.Q("DescriptionPanel"));
            _upgradePanel = new UpgradeWindowUpgradePanel(rootVisualElement.Q("UpgradePanel"),
                _ingredientListElementTemplate, Close, _inventory);
            Close();
        }

        public void Open()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _itemsList.ClearSelection();
            _itemsList.SetItems(_inventory);
            _descriptionPanel.HideDescription();
            _upgradePanel.Hide();
            IsOpen = true;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            IsOpen = false;
        }

        private void SelectListElement(UpgradableItemSO item)
        {
            _descriptionPanel.UpdateDescription(item.ItemAtNextLevel);
            _upgradePanel.UpdatePanel(item);
        }
    }
}
