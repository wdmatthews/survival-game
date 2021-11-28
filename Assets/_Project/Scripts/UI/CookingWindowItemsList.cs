using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Growing;

namespace Project.UI
{
    public class CookingWindowItemsList
    {
        private ListView _listView = null;
        private VisualTreeAsset _listElementTemplate = null;
        private List<FoodSO> _items = new();
        private System.Action<FoodSO> _onSelectElement = null;

        public CookingWindowItemsList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<FoodSO> onSelectElement, FoodSO[] cookableFoods)
        {
            _listView = listView;
            _listElementTemplate = listElementTemplate;
            _onSelectElement = onSelectElement;
            _items = new List<FoodSO>(cookableFoods);
            _listView.itemsSource = _items;
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
            FoodSO item = _items[index];

            icon.style.backgroundImage = new StyleBackground(item.Icon);
            nameLabel.text = item.name;
        }

        private void SelectElements(IEnumerable<object> elementObjects)
        {
            foreach (var elementObject in elementObjects)
            {
                FoodSO item = (FoodSO)elementObject;
                _onSelectElement?.Invoke(item);
            }
        }
    }
}
