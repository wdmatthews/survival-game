using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Project.Building;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Fast Travel Window")]
    [DisallowMultipleComponent]
    public class FastTravelWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _campfireListElement = null;

        private List<CampfireData> _campfires = null;
        private ListView _campfireList = null;
        private Button _cancelButton = null;
        private Button _travelButton = null;
        public System.Action<CampfireData> FastTravel { get; set; }
        public System.Action OnClose { get; set; }

        private void Awake()
        {
            VisualElement rootVisualElement = _uiDocument.rootVisualElement;
            _campfireList = rootVisualElement.Q<ListView>("CampfireList");
            _cancelButton = rootVisualElement.Q<Button>("CancelButton");
            _travelButton = rootVisualElement.Q<Button>("TravelButton");
            _campfireList.makeItem = MakeListElement;
            _campfireList.bindItem = BindListElement;
            _cancelButton.clicked += Close;
            _travelButton.clicked += Travel;
            Close();
        }

        public void Open(List<CampfireData> campfires)
        {
            _campfires = campfires;
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            _campfireList.itemsSource = campfires;
            _campfireList.selectedIndex = 0;
        }

        public void Close()
        {
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            OnClose?.Invoke();
        }

        private void Travel()
        {
            FastTravel?.Invoke(_campfires[_campfireList.selectedIndex]);
        }

        private VisualElement MakeListElement()
        {
            return _campfireListElement.Instantiate().ElementAt(0);
        }

        private void BindListElement(VisualElement element, int index)
        {
            Label nameLabel = element.Q<Label>("Name");
            CampfireData campfire = _campfires[index];
            nameLabel.text = campfire.Name;
        }
    }
}
