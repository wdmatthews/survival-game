using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;

namespace Project.UI
{
    public class BuildWindowStructureList
    {
        private ListView _listView = null;
        private VisualTreeAsset _listElementTemplate = null;
        private List<StructureSO> _structures = new();
        private System.Action<StructureSO> _onSelectElement = null;

        public BuildWindowStructureList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<StructureSO> onSelectElement, List<StructureSO> structures)
        {
            _structures = structures;
            _listView = listView;
            _listElementTemplate = listElementTemplate;
            _onSelectElement = onSelectElement;
            _listView.itemsSource = _structures;
            _listView.onItemsChosen += SelectElements;
            _listView.onSelectionChange += SelectElements;
            _listView.makeItem = MakeListElement;
            _listView.bindItem = BindListElement;
        }

        public void ClearSelection()
        {
            _listView.ClearSelection();
        }

        private VisualElement MakeListElement()
        {
            return _listElementTemplate.Instantiate().ElementAt(0);
        }

        private void BindListElement(VisualElement element, int index)
        {
            VisualElement icon = element.Q("Icon");
            Label nameLabel = element.Q<Label>("Name");
            StructureSO structure = _structures[index];

            icon.style.backgroundImage = new StyleBackground(structure.Icon);
            nameLabel.text = structure.name;
        }

        private void SelectElements(IEnumerable<object> elementObjects)
        {
            foreach (var elementObject in elementObjects)
            {
                StructureSO structure = (StructureSO)elementObject;
                _onSelectElement?.Invoke(structure);
            }
        }
    }
}
