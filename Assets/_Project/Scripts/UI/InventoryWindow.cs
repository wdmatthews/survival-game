using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Inventory Window")]
    [DisallowMultipleComponent]
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private Project.Items.CraftingIngredientSO _craftingIngredient = null;

        private ListView _resourcesList = null;
        private ListView _itemsList = null;
        private InventoryWindowDescriptionPanel _descriptionPanel = null;

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _resourcesList = rootVisualElement.Q<ListView>("ResourcesList");
            _itemsList = rootVisualElement.Q<ListView>("ItemsList");
            _descriptionPanel = new InventoryWindowDescriptionPanel(rootVisualElement.Q("DescriptionPanel"));
            // _uiDocument.enabled = false;
            _descriptionPanel.UpdateDescription(_craftingIngredient, 4);
        }
    }
}
