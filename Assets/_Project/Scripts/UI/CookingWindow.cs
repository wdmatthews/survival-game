using UnityEngine;
using UnityEngine.UIElements;
using Project.Growing;
using Project.Items;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Cooking Window")]
    [DisallowMultipleComponent]
    public class CookingWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _cookableItemListElementTemplate = null;
        [SerializeField] private VisualTreeAsset _ingredientListElementTemplate = null;
        [SerializeField] private InventorySO _inventory = null;
        [SerializeField] private FoodSO[] _cookableFoods = { };

        private CookingWindowItemsList _itemsList = null;
        private CookingWindowDescriptionPanel _descriptionPanel = null;
        private CookingWindowCookPanel _cookPanel = null;
        public bool IsOpen { get; private set; } = false;
        public System.Action<FoodSO> CookFood { set => _cookPanel.OnCookFood = value; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _itemsList = new CookingWindowItemsList(rootVisualElement.Q<ListView>("ItemsList"),
                _cookableItemListElementTemplate, SelectListElement, _cookableFoods);
            _descriptionPanel = new CookingWindowDescriptionPanel(rootVisualElement.Q("DescriptionPanel"));
            _cookPanel = new CookingWindowCookPanel(rootVisualElement.Q("CookPanel"),
                _ingredientListElementTemplate, Close, _inventory);
            Close();
        }

        public void Open()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _itemsList.ClearSelection();
            _descriptionPanel.HideDescription();
            _cookPanel.Hide();
            IsOpen = true;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            IsOpen = false;
        }

        private void SelectListElement(FoodSO food)
        {
            _descriptionPanel.UpdateDescription(food);
            _cookPanel.UpdatePanel(food);
        }
    }
}
