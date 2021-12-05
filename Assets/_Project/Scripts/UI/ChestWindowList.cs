using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Items;

namespace Project.UI
{
    public class ChestWindowList
    {
        private ListView _listView = null;
        private VisualTreeAsset _listElementTemplate = null;
        private List<CraftingIngredientStack> _dataStacks = new();
        private System.Action<CraftingIngredientStack, ChestWindowList> _onSelectElement = null;

        public ChestWindowList(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<CraftingIngredientStack, ChestWindowList> onSelectElement,
            List<CraftingIngredientStack> stacks)
        {
            Initialize(listView, listElementTemplate, onSelectElement);
            UpdateData(stacks);
        }

        private void Initialize(ListView listView, VisualTreeAsset listElementTemplate,
            System.Action<CraftingIngredientStack, ChestWindowList> onSelectElement)
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

        public void UpdateData(List<CraftingIngredientStack> stacks)
        {
            _dataStacks.Clear();
            _dataStacks.AddRange(stacks);
            _listView.Rebuild();
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
                _onSelectElement?.Invoke(stack, this);
            }
        }
    }
}
