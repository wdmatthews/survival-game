using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Campfire Window")]
    [DisallowMultipleComponent]
    public class CampfireWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;

        private CampfireData _campfire = null;
        private TextField _nameField = null;
        private Button _cancelButton = null;
        private Button _renameButton = null;
        public System.Action<CampfireData, string> RenameCampfire { get; set; }
        public System.Action OnClose { get; set; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _nameField = rootVisualElement.Q<TextField>("Name");
            _cancelButton = rootVisualElement.Q<Button>("CancelButton");
            _renameButton = rootVisualElement.Q<Button>("RenameButton");
            _cancelButton.clicked += Close;
            _renameButton.clicked += Rename;
            Close();
        }

        public void Open(CampfireData campfire)
        {
            _campfire = campfire;
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _nameField.value = _campfire.Name;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            OnClose?.Invoke();
        }

        private void Rename()
        {
            RenameCampfire?.Invoke(_campfire, _nameField.value);
        }
    }
}
