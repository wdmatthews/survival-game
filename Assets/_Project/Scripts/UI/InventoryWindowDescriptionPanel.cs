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
        private List<Button> _hotbarAssignmentButtons = new();
        private Color _hotbarButtonSelectedBorderColor = new();
        private Color _hotbarButtonDefaultBorderColor = new();
        private float _hotbarButtonSelectedOpacity = 0;
        private int _firstHotbarButtonIndex = -1;

        public InventoryWindowDescriptionPanel(VisualElement panel)
        {
            _panel = panel;
            _nameLabel = _panel.Q<Label>("Name");
            _icon = _panel.Q("Icon");
            _descriptionLabel = _panel.Q<Label>("Description");
            List<VisualElement> buttons = new(_panel.Q("HotbarButtons").Children());

            foreach (var buttonElement in buttons)
            {
                Button button = buttonElement.Q<Button>("HotbarButton");
                _hotbarAssignmentButtons.Add(button);

                if (_firstHotbarButtonIndex < 0)
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
            _nameLabel.text = craftingIngredient.name;
            _icon.style.backgroundImage = new StyleBackground(craftingIngredient.Icon);
            _descriptionLabel.text = craftingIngredient.Description;
            UpdateHotbarIndex(hotbarIndex);
        }

        public void UpdateHotbarIndex(int hotbarIndex = -1)
        {
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
    }
}