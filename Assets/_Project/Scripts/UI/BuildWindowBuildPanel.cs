using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;
using Project.Items;

namespace Project.UI
{
    public class BuildWindowBuildPanel
    {
        private VisualElement _panel = null;
        private VisualTreeAsset _listElementTemplate = null;
        private InventorySO _inventory = null;
        private ListView _ingredientsList = null;
        private Button _cancelButton = null;
        private Button _buildButton = null;
        private StructureSO _structure = null;
        public System.Action<StructureSO> OnBuildStructure { get; set; } = null;
        private float _amountNotEnoughOpacity = 0.5f;
        private Color _amountEnoughColor = new();
        private Color _amountNotEnoughColor = new();
        private bool _inventoryContainsEnoughOfEveryIngredient = false;

        public BuildWindowBuildPanel(VisualElement panel, VisualTreeAsset listElementTemplate,
            System.Action closeBuildWindow, InventorySO inventory)
        {
            _panel = panel;
            _listElementTemplate = listElementTemplate;
            _inventory = inventory;
            _ingredientsList = _panel.Q<ListView>("IngredientsList");
            _cancelButton = _panel.Q<Button>("CancelButton");
            _buildButton = _panel.Q<Button>("BuildButton");
            _ingredientsList.selectionType = SelectionType.None;
            _ingredientsList.makeItem = MakeListElement;
            _ingredientsList.bindItem = BindListElement;
            _cancelButton.clicked += closeBuildWindow;
            _buildButton.clicked += BuildStructure;

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

        public void UpdatePanel(StructureSO structure)
        {
            Show();
            _structure = structure;
            _ingredientsList.itemsSource = _structure.Ingredients;
            _ingredientsList.Rebuild();

            _inventoryContainsEnoughOfEveryIngredient = true;

            for (int i = _structure.Ingredients.Length - 1; i >= 0; i--)
            {
                if (!InventoryContainsEnough(_structure.Ingredients[i]))
                {
                    _inventoryContainsEnoughOfEveryIngredient = false;
                    break;
                }
            }

            _buildButton.style.opacity = _inventoryContainsEnoughOfEveryIngredient
                ? 1 : _amountNotEnoughOpacity;
        }

        private void BuildStructure()
        {
            if (!_inventoryContainsEnoughOfEveryIngredient) return;
            OnBuildStructure?.Invoke(_structure);
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
            CraftingIngredientStack craftingIngredientStack = _structure.Ingredients[index];
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