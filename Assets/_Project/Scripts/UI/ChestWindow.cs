using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;
using Project.Items;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Chest Window")]
    [DisallowMultipleComponent]
    public class ChestWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _listElementTemplate = null;
        [SerializeField] private InventorySO _inventory = null;

        private Chest _chest = null;
        private ChestWindowList _inventoryList = null;
        private ChestWindowList _chestList = null;
        private ChestWindowTransferPanel _transferPanel = null;
        private bool _inventoryItemIsSelected = false;
        public System.Action<CraftingIngredientSO, int> AddToChest { set => _transferPanel.AddToChest = value; }
        public System.Action<CraftingIngredientSO, int> TakeFromChest { set => _transferPanel.TakeFromChest = value; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _inventoryList = new ChestWindowList(rootVisualElement.Q<ListView>("InventoryList"),
                _listElementTemplate, SelectListElement, _inventory.GetChestCompatibleStacks());
            _chestList = new ChestWindowList(rootVisualElement.Q<ListView>("ChestList"),
                _listElementTemplate, SelectListElement, new List<CraftingIngredientStack>());
            _transferPanel = new ChestWindowTransferPanel(rootVisualElement.Q("TransferPanel"),
                Close);
            Close();
        }

        public void Open(Chest chest)
        {
            _chest = chest;
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _inventoryList.ClearSelection();
            _chestList.ClearSelection();
            _transferPanel.Hide();
            _inventoryList.UpdateData(_inventory.GetChestCompatibleStacks());
            _chestList.UpdateData(_chest.Stacks);
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        private void SelectListElement(CraftingIngredientStack stack,
            ChestWindowList list)
        {
            _inventoryItemIsSelected = list == _inventoryList;
            if (_inventoryItemIsSelected) _chestList.ClearSelection();
            else _inventoryList.ClearSelection();
            _transferPanel.UpdateInformation(_inventoryItemIsSelected, stack);
        }
    }
}
