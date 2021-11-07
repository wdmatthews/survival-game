using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class HotbarSlotUI
    {
        private VisualElement _slot = null;
        private VisualElement _icon = null;
        private Label _label = null;
        private Color _selectedColor = new Color();
        private Color _defaultColor = new Color();

        public HotbarSlotUI(VisualElement slot, VisualElement icon, Label label)
        {
            _slot = slot;
            _icon = icon;
            _label = label;
            _selectedColor = _slot.resolvedStyle.borderTopColor;
            _defaultColor = _slot.resolvedStyle.backgroundColor;
        }

        public void SetItem(Sprite icon = null, int amount = 0)
        {
            if (icon)
            {
                _icon.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                SetItemAmount(amount);
            }
            else
            {
                _icon.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                _label.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }

        public void SetItemAmount(int amount)
        {
            _label.text = $"{amount}";

            if (amount > 0)
            {
                _label.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                _label.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }

        public void SetSelected(bool isSelected)
        {
            StyleColor color = new StyleColor(isSelected ? _selectedColor : _defaultColor);
            _slot.style.borderTopColor = color;
            _slot.style.borderBottomColor = color;
            _slot.style.borderLeftColor = color;
            _slot.style.borderRightColor = color;
        }
    }
}
