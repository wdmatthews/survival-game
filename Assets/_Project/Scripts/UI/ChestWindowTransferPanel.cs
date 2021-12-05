using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class ChestWindowTransferPanel
    {
        private VisualElement _panel = null;
        private Label _nameLabel = null;
        private TextField _amountField = null;
        private Button _cancelButton = null;
        private Button _transferButton = null;
        private CraftingIngredientStack _stack = null;
        private bool _itemIsFromInventory = false;
        private int _amount = 0;
        public System.Action<CraftingIngredientSO, int> AddToChest { get; set; }
        public System.Action<CraftingIngredientSO, int> TakeFromChest { get; set; }

        public ChestWindowTransferPanel(VisualElement panel, System.Action closeWindow)
        {
            _panel = panel;
            _nameLabel = _panel.Q<Label>("Name");
            _amountField = _panel.Q<TextField>("Amount");
            _cancelButton = _panel.Q<Button>("CancelButton");
            _transferButton = _panel.Q<Button>("TransferButton");
            _amountField.RegisterValueChangedCallback(UpdateAmount);
            _cancelButton.clicked += closeWindow;
            _transferButton.clicked += Transfer;
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

        public void UpdateInformation(bool itemIsFromInventory, CraftingIngredientStack stack)
        {
            Show();
            _stack = stack;
            _itemIsFromInventory = itemIsFromInventory;
            _amount = 0;
            _nameLabel.text = $"{stack.Ingredient.name} to {(itemIsFromInventory ? "Chest" : "Inventory")}";
            _amountField.value = $"{_amount}";
        }

        private void UpdateAmount(ChangeEvent<string> changeEvent)
        {
            string newAmountString = _amountField.value;
            if (int.TryParse(newAmountString, out _amount)) _amount = Mathf.Clamp(_amount, 1, _stack.Amount);
            else _amount = 1;
            if (newAmountString.Length > 0) _amountField.SetValueWithoutNotify($"{_amount}");
        }

        private void Transfer()
        {
            if (_itemIsFromInventory) AddToChest(_stack.Ingredient, _amount);
            else TakeFromChest(_stack.Ingredient, _amount);
        }
    }
}
