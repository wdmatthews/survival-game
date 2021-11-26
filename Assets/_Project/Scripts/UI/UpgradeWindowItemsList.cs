using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class UpgradeWindowItemsList
    {
        private ListView _listView = null;
        private VisualTreeAsset _listElementTemplate = null;
        private List<UpgradableItemSO> _items = new();
        private System.Action<UpgradableItemSO> _onSelectElement = null;

        public UpgradeWindowItemsList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<UpgradableItemSO> onSelectElement, InventorySO inventory)
        {
            SetItems(inventory);
            _listView = listView;
            _listElementTemplate = listElementTemplate;
            _onSelectElement = onSelectElement;
            _listView.itemsSource = _items;
            _listView.onItemsChosen += SelectElements;
            _listView.onSelectionChange += SelectElements;
            _listView.makeItem = MakeListElement;
            _listView.bindItem = BindListElement;
        }

        public void SetItems(InventorySO inventory)
        {
            _items.Clear();

            foreach (ItemStack itemStack in inventory.Items)
            {
                if (itemStack.Item is UpgradableItemSO upgradableItem
                    && upgradableItem.ItemAtNextLevel) _items.Add(upgradableItem);
            }
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
            UpgradableItemSO item = _items[index];

            icon.style.backgroundImage = new StyleBackground(item.Icon);
            nameLabel.text = item.ItemAtNextLevel.name;
        }

        private void SelectElements(IEnumerable<object> elementObjects)
        {
            foreach (var elementObject in elementObjects)
            {
                UpgradableItemSO item = (UpgradableItemSO)elementObject;
                _onSelectElement?.Invoke(item);
            }
        }
    }
}
