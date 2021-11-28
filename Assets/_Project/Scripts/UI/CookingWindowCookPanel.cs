using UnityEngine;
using UnityEngine.UIElements;
using Project.Growing;
using Project.Items;

namespace Project.UI
{
    public class CookingWindowCookPanel
    {
        private VisualElement _panel = null;
        private VisualTreeAsset _listElementTemplate = null;
        private InventorySO _inventory = null;
        private ListView _ingredientsList = null;
        private Button _cancelButton = null;
        private Button _cookButton = null;
        private FoodSO _item = null;
        public System.Action<FoodSO> OnCookFood { get; set; } = null;
        private float _amountNotEnoughOpacity = 0.5f;
        private Color _amountEnoughColor = new();
        private Color _amountNotEnoughColor = new();
        private bool _inventoryContainsEnoughOfEveryIngredient = false;

        public CookingWindowCookPanel(VisualElement panel, VisualTreeAsset listElementTemplate,
            System.Action closeWindow, InventorySO inventory)
        {
            _panel = panel;
            _listElementTemplate = listElementTemplate;
            _inventory = inventory;
            _ingredientsList = _panel.Q<ListView>("IngredientsList");
            _cancelButton = _panel.Q<Button>("CancelButton");
            _cookButton = _panel.Q<Button>("CookButton");
            _ingredientsList.selectionType = SelectionType.None;
            _ingredientsList.makeItem = MakeListElement;
            _ingredientsList.bindItem = BindListElement;
            _cancelButton.clicked += closeWindow;
            _cookButton.clicked += CookFood;

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

        public void UpdatePanel(FoodSO item)
        {
            Show();
            _item = item;
            _ingredientsList.itemsSource = _item.FoodNeededToCook;
            _ingredientsList.Rebuild();

            _inventoryContainsEnoughOfEveryIngredient = true;

            for (int i = _item.FoodNeededToCook.Length - 1; i >= 0; i--)
            {
                if (!InventoryContainsEnough(_item.FoodNeededToCook[i]))
                {
                    _inventoryContainsEnoughOfEveryIngredient = false;
                    break;
                }
            }

            _cookButton.style.opacity = _inventoryContainsEnoughOfEveryIngredient
                ? 1 : _amountNotEnoughOpacity;
        }

        private void CookFood()
        {
            if (!_inventoryContainsEnoughOfEveryIngredient) return;
            OnCookFood?.Invoke(_item);
        }

        private bool InventoryContainsEnough(FoodStack foodStack)
        {
            FoodSO food = foodStack.Food;
            return _inventory.ItemsByItemData.ContainsKey(food)
                    && _inventory.ItemsByItemData[food].Amount >= foodStack.Amount;
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
            FoodStack foodStack = _item.FoodNeededToCook[index];
            FoodSO food = foodStack.Food;
            bool inventoryContainsEnough = InventoryContainsEnough(foodStack);

            icon.style.backgroundImage = new StyleBackground(food.Icon);
            nameLabel.text = food.name;
            nameLabel.style.opacity = inventoryContainsEnough ? 1 : _amountNotEnoughOpacity;
            amountLabel.text = $"{foodStack.Amount}";
            amountLabel.style.color = new StyleColor(
                inventoryContainsEnough ? _amountEnoughColor : _amountNotEnoughColor);
        }
    }
}