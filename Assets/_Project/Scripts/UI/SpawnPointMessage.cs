using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Spawn Point Message")]
    [DisallowMultipleComponent]
    public class SpawnPointMessage : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private float _messageDuration = 1;

        private Label _messageLabel = null;
        private bool _isShown = false;
        private float _timer = 0;

        private void Awake()
        {
            _messageLabel = _uiDocument.rootVisualElement.Q<Label>("Message");
            Close();
        }

        private void Update()
        {
            if (!_isShown) return;
            _timer = Mathf.Clamp(_timer - Time.deltaTime, 0, _messageDuration);
            _messageLabel.style.opacity = new StyleFloat(_timer / _messageDuration);
            if (Mathf.Approximately(_timer, 0)) Close();
        }

        public void Open()
        {
            _isShown = true;
            _timer = _messageDuration;
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void Close()
        {
            _isShown = false;
            _uiDocument.rootVisualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
    }
}
