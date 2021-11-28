using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Inventory Window")]
    [DisallowMultipleComponent]
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _listElementTemplate = null;
        [SerializeField] private InventorySO _inventory = null;

        private InventoryWindowList _resourcesList = null;
        private InventoryWindowList _itemsList = null;
        private InventoryWindowDescriptionPanel _descriptionPanel = null;
        public System.Action<ItemSO, int> AddItemToHotbar { get; set; } = null;
        public System.Action<int> RemoveItemFromHotbar { get; set; } = null;
        public bool IsOpen { get; private set; } = false;

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _resourcesList = new InventoryWindowList(rootVisualElement.Q<ListView>("ResourcesList"),
                _listElementTemplate, SelectListElement, _inventory.Resources);
            _itemsList = new InventoryWindowList(rootVisualElement.Q<ListView>("ItemsList"),
                _listElementTemplate, SelectListElement, _inventory.Items);
            _descriptionPanel = new InventoryWindowDescriptionPanel(rootVisualElement.Q("DescriptionPanel"),
                AssignItemToHotbarIndex);
            Close();
        }

        public void Open()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _resourcesList.ClearSelection();
            _itemsList.ClearSelection();
            _descriptionPanel.HideDescription();
            _resourcesList.UpdateData(_inventory.Resources);
            _itemsList.UpdateData(_inventory.Items);
            IsOpen = true;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            IsOpen = false;
        }

        private void SelectListElement(CraftingIngredientSO craftingIngredient,
            InventoryWindowList list)
        {
            if (list == _resourcesList) _itemsList.ClearSelection();
            else _resourcesList.ClearSelection();
            int hotbarIndex = -1;

            for (int i = _inventory.HotbarItems.Count - 1; i >= 0; i--)
            {
                if (_inventory.HotbarItems[i] != null
                    && _inventory.HotbarItems[i].Item == craftingIngredient)
                {
                    hotbarIndex = i;
                    break;
                }
            }

            _descriptionPanel.UpdateDescription(craftingIngredient, hotbarIndex);
        }

        private void AssignItemToHotbarIndex(CraftingIngredientSO item, int newIndex, int oldIndex)
        {
            _descriptionPanel.UpdateHotbarIndex(newIndex);
            if (oldIndex >= 0) RemoveItemFromHotbar.Invoke(oldIndex);
            AddItemToHotbar.Invoke((ItemSO)item, newIndex);
        }
    }
}
