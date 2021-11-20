using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;

namespace Project.UI
{
    public class BuildWindowDescriptionPanel
    {
        private VisualElement _panel = null;
        private Label _nameLabel = null;
        private VisualElement _icon = null;
        private Label _descriptionLabel = null;

        public BuildWindowDescriptionPanel(VisualElement panel)
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

        public void UpdateDescription(StructureSO structure)
        {
            ShowDescription();
            _nameLabel.text = structure.name;
            _icon.style.backgroundImage = new StyleBackground(structure.Icon);
            _descriptionLabel.text = structure.Description;
        }
    }
}