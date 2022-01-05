using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;
using Project.Items;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Build Window")]
    [DisallowMultipleComponent]
    public class BuildWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _structureListElementTemplate = null;
        [SerializeField] private VisualTreeAsset _ingredientListElementTemplate = null;
        [SerializeField] private InventorySO _inventory = null;
        [SerializeField] private List<StructureSO> _structures = new();

        private BuildWindowStructureList _structuresList = null;
        private BuildWindowDescriptionPanel _descriptionPanel = null;
        private BuildWindowBuildPanel _buildPanel = null;
        public bool IsOpen { get; private set; } = false;
        public System.Action<StructureSO> BuildStructure { set => _buildPanel.OnBuildStructure = value; }
        public System.Action OnClose { get; set; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _structuresList = new BuildWindowStructureList(rootVisualElement.Q<ListView>("StructuresList"),
                _structureListElementTemplate, SelectListElement, _structures);
            _descriptionPanel = new BuildWindowDescriptionPanel(rootVisualElement.Q("DescriptionPanel"));
            _buildPanel = new BuildWindowBuildPanel(rootVisualElement.Q("BuildPanel"),
                _ingredientListElementTemplate, Close, _inventory);
            Close();
        }

        public void Open()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _structuresList.ClearSelection();
            _descriptionPanel.HideDescription();
            _buildPanel.Hide();
            IsOpen = true;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            IsOpen = false;
            OnClose?.Invoke();
        }

        private void SelectListElement(StructureSO structure)
        {
            _descriptionPanel.UpdateDescription(structure);
            _buildPanel.UpdatePanel(structure);
        }
    }
}
