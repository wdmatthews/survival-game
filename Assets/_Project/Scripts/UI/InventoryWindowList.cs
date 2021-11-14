using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class InventoryWindowList
    {
        private ListView _listView = null;
        private VisualTreeAsset _listElementTemplate = null;
        private List<CraftingIngredientStack> _dataStacks = new();
        private System.Action<CraftingIngredientSO, InventoryWindowList> _onSelectElement = null;

        public InventoryWindowList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<CraftingIngredientSO, InventoryWindowList> onSelectElement,
            List<ResourceStack> resourceStacks)
        {
            Initialize(listView, listElementTemplate, onSelectElement);
            UpdateData(resourceStacks);
        }

        public InventoryWindowList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<CraftingIngredientSO, InventoryWindowList> onSelectElement,
            List<ItemStack> itemStacks)
        {
            Initialize(listView, listElementTemplate, onSelectElement);
            UpdateData(itemStacks);
        }

        private void Initialize(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<CraftingIngredientSO, InventoryWindowList> onSelectElement)
        {
            _listView = listView;
            _listElementTemplate = listElementTemplate;
            _onSelectElement = onSelectElement;
            _listView.itemsSource = _dataStacks;
            _listView.onItemsChosen += SelectElements;
            _listView.onSelectionChange += SelectElements;
            _listView.makeItem = MakeListElement;
            _listView.bindItem = BindListElement;
        }

        public void ClearSelection()
        {
            _listView.ClearSelection();
        }

        public void UpdateData(List<ResourceStack> resourceStacks)
        {
            _dataStacks.Clear();

            foreach (ResourceStack resourceStack in resourceStacks)
            {
                _dataStacks.Add(new CraftingIngredientStack(resourceStack.Resource, resourceStack.Amount));
            }
        }

        public void UpdateData(List<ItemStack> itemStacks)
        {
            _dataStacks.Clear();

            foreach (ItemStack itemStack in itemStacks)
            {
                _dataStacks.Add(new CraftingIngredientStack(itemStack.Item, itemStack.Amount));
            }
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
            CraftingIngredientStack stack = _dataStacks[index];
            
            icon.style.backgroundImage = new StyleBackground(stack.Ingredient.Icon);
            nameLabel.text = stack.Ingredient.name;
            amountLabel.text = $"{stack.Amount}";
        }

        private void SelectElements(IEnumerable<object> elementObjects)
        {
            foreach (var elementObject in elementObjects)
            {
                CraftingIngredientStack stack = (CraftingIngredientStack)elementObject;
                _onSelectElement?.Invoke(stack.Ingredient, this);
            }
        }
    }
}
