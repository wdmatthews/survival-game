using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class InventoryWindowDescriptionPanel
    {
        private VisualElement _panel = null;
        private Label _nameLabel = null;
        private VisualElement _icon = null;
        private Label _descriptionLabel = null;
        private VisualElement _hotbarButtonsPanel = null;
        private List<Button> _hotbarAssignmentButtons = new();
        private Color _hotbarButtonSelectedBorderColor = new();
        private Color _hotbarButtonDefaultBorderColor = new();
        private float _hotbarButtonSelectedOpacity = 0;
        private int _firstHotbarButtonIndex = -1;
        private int _currentHotbarIndex = -1;
        private System.Action<CraftingIngredientSO, int, int> _onAssignItemToHotbarIndex = null;
        private CraftingIngredientSO _craftingIngredient = null;

        public InventoryWindowDescriptionPanel(VisualElement panel,
            System.Action<CraftingIngredientSO, int, int> onAssignItemToHotbarIndex)
        {
            _panel = panel;
            _onAssignItemToHotbarIndex = onAssignItemToHotbarIndex;
            _nameLabel = _panel.Q<Label>("Name");
            _icon = _panel.Q("Icon");
            _descriptionLabel = _panel.Q<Label>("Description");
            _hotbarButtonsPanel = _panel.Q("HotbarPanel");
            List<VisualElement> buttons = new(_panel.Q("HotbarButtons").Children());
            int buttonCount = buttons.Count;

            for (int i = 0; i < buttonCount; i++)
            {
                Button button = buttons[i].Q<Button>("HotbarButton");
                button.clicked += () => AssignItemToHotbarIndex(button);
                _hotbarAssignmentButtons.Add(button);

                if (i == 0)
                {
                    _firstHotbarButtonIndex = int.Parse(button.text) - 1;
                    _hotbarButtonSelectedBorderColor = button.resolvedStyle.borderTopColor;
                    _hotbarButtonDefaultBorderColor = button.resolvedStyle.backgroundColor;
                    _hotbarButtonSelectedOpacity = button.resolvedStyle.opacity;
                }
            }

            HideDescription();
        }

        public void ShowDescription()
        {
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public void HideDescription()
        {
            _panel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        public void UpdateDescription(CraftingIngredientSO craftingIngredient, int hotbarIndex = -1)
        {
            ShowDescription();
            _craftingIngredient = craftingIngredient;
            _nameLabel.text = craftingIngredient.name;
            _icon.style.backgroundImage = new StyleBackground(craftingIngredient.Icon);
            _descriptionLabel.text = craftingIngredient.Description;

            if (craftingIngredient.CanBeInHotbar)
            {
                _hotbarButtonsPanel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                UpdateHotbarIndex(hotbarIndex);
            }
            else
            {
                _hotbarButtonsPanel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }

        public void UpdateHotbarIndex(int hotbarIndex = -1)
        {
            _currentHotbarIndex = hotbarIndex;

            for (int i = _hotbarAssignmentButtons.Count - 1; i >= 0; i--)
            {
                bool isSelected = hotbarIndex == i + _firstHotbarButtonIndex;
                Button button = _hotbarAssignmentButtons[i];
                Color borderColor = isSelected
                    ? _hotbarButtonSelectedBorderColor : _hotbarButtonDefaultBorderColor;
                button.style.borderTopColor = borderColor;
                button.style.borderBottomColor = borderColor;
                button.style.borderLeftColor = borderColor;
                button.style.borderRightColor = borderColor;
                button.style.opacity = new StyleFloat(isSelected ? _hotbarButtonSelectedOpacity : 1);
            }
        }

        private void AssignItemToHotbarIndex(Button button)
        {
            int newHotbarIndex = int.Parse(button.text) - 1;
            if (_currentHotbarIndex == newHotbarIndex) return;
            _onAssignItemToHotbarIndex?.Invoke(_craftingIngredient, newHotbarIndex, _currentHotbarIndex);
        }
    }
}