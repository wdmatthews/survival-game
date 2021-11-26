using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class UpgradeWindowDescriptionPanel
    {
        private VisualElement _panel = null;
        private Label _nameLabel = null;
        private VisualElement _icon = null;
        private Label _descriptionLabel = null;

        public UpgradeWindowDescriptionPanel(VisualElement panel)
        {
            _panel = panel;
            _nameLabel = _panel.Q<Label>("Name");
            _icon = _panel.Q("Icon");
            _descriptionLabel = _panel.Q<Label>("Description");
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

        public void UpdateDescription(UpgradableItemSO item)
        {
            ShowDescription();
            _nameLabel.text = item.name;
            _icon.style.backgroundImage = new StyleBackground(item.Icon);
            _descriptionLabel.text = item.Description;
        }
    }
}