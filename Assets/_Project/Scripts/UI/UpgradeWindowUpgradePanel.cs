using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class UpgradeWindowUpgradePanel
    {
        private VisualElement _panel = null;
        private VisualTreeAsset _listElementTemplate = null;
        private InventorySO _inventory = null;
        private ListView _ingredientsList = null;
        private Button _cancelButton = null;
        private Button _upgradeButton = null;
        private UpgradableItemSO _item = null;
        public System.Action<UpgradableItemSO> OnUpgradeItem { get; set; } = null;
        private float _amountNotEnoughOpacity = 0.5f;
        private Color _amountEnoughColor = new();
        private Color _amountNotEnoughColor = new();
        private bool _inventoryContainsEnoughOfEveryIngredient = false;

        public UpgradeWindowUpgradePanel(VisualElement panel, VisualTreeAsset listElementTemplate,
            System.Action closeWindow, InventorySO inventory)
        {
            _panel = panel;
            _listElementTemplate = listElementTemplate;
            _inventory = inventory;
            _ingredientsList = _panel.Q<ListView>("IngredientsList");
            _cancelButton = _panel.Q<Button>("CancelButton");
            _upgradeButton = _panel.Q<Button>("UpgradeButton");
            _ingredientsList.selectionType = SelectionType.None;
            _ingredientsList.makeItem = MakeListElement;
            _ingredientsList.bindItem = BindListElement;
            _cancelButton.clicked += closeWindow;
            _upgradeButton.clicked += UpgradeItem;

            VisualElement sampleListElement = _listElementTemplate.Instantiate().ElementAt(0);
            Label sampleListElementNameLabel = sampleListElement.Q<Label>("Name");
            Label sampleListElementAmountLabel = sampleListElement.Q<Label>("Amount");
            _amountNotEnoughOpacity = sampleListElementNameLabel.resolvedStyle.opacity;
            _amountEnoughColor = sampleListElementNameLabel.resolvedStyle.color;
            _amountNotEnoughColor = sampleListElementAmountLabel.resolvedStyle.color;

            Hide();
        }

        public void Show()
        {
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public void Hide()
        {
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        public void UpdatePanel(UpgradableItemSO item)
        {
            Show();
            _item = item;
            _ingredientsList.itemsSource = _item.IngredientsNeededToUpgrade;
            _ingredientsList.Rebuild();

            _inventoryContainsEnoughOfEveryIngredient = true;

            for (int i = _item.IngredientsNeededToUpgrade.Length - 1; i >= 0; i--)
            {
                if (!InventoryContainsEnough(_item.IngredientsNeededToUpgrade[i]))
                {
                    _inventoryContainsEnoughOfEveryIngredient = false;
                    break;
                }
            }

            _upgradeButton.style.opacity = _inventoryContainsEnoughOfEveryIngredient
                ? 1 : _amountNotEnoughOpacity;
        }

        private void UpgradeItem()
        {
            if (!_inventoryContainsEnoughOfEveryIngredient) return;
            OnUpgradeItem?.Invoke(_item);
        }

        private bool InventoryContainsEnough(CraftingIngredientStack craftingIngredientStack)
        {
            CraftingIngredientSO craftingIngredient = craftingIngredientStack.Ingredient;

            if (craftingIngredient is ResourceTypeSO resource)
            {
                return _inventory.ResourcesByResourceData.ContainsKey(resource)
                    && _inventory.ResourcesByResourceData[resource].Amount >= craftingIngredientStack.Amount;
            }
            else if (craftingIngredient is ItemSO item)
            {
                return _inventory.ItemsByItemData.ContainsKey(item)
                    && _inventory.ItemsByItemData[item].Amount >= craftingIngredientStack.Amount;
            }

            return false;
        }

        private VisualElement MakeListElement()
        {
            return _listElementTemplate.Instantiate().ElementAt(0);
        }

        private void BindListElement(VisualElement element, int index)
        {
            VisualElement icon = element.Q("Icon");
            Label nameLabel = element.Q<Label>("Name");
            Label amountLabel = element.Q<Label>("Amount");
            CraftingIngredientStack craftingIngredientStack = _item.IngredientsNeededToUpgrade[index];
            CraftingIngredientSO craftingIngredient = craftingIngredientStack.Ingredient;
            bool inventoryContainsEnough = InventoryContainsEnough(craftingIngredientStack);

            icon.style.backgroundImage = new StyleBackground(craftingIngredient.Icon);
            nameLabel.text = craftingIngredient.name;
            nameLabel.style.opacity = inventoryContainsEnough ? 1 : _amountNotEnoughOpacity;
            amountLabel.text = $"{craftingIngredientStack.Amount}";
            amountLabel.style.color = new StyleColor(
                inventoryContainsEnough ? _amountEnoughColor : _amountNotEnoughColor);
        }
    }
}