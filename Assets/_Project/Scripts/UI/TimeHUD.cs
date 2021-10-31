using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Time HUD")]
    [DisallowMultipleComponent]
    public class TimeHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;

        private Label _timeLabel = null;

        private void Awake()
        {
            _timeLabel = _uiDocument.rootVisualElement.Q<Label>("Time");
        }

        public void UpdateTime(bool isDay, int day, float minutesLeft)
        {
            int numMinutes = Mathf.FloorToInt(minutesLeft);
            int numSeconds = Mathf.CeilToInt((minutesLeft - numMinutes) * 60);
            _timeLabel.text = $"{(isDay ? "Day" : "Night")} {day}\n{numMinutes}:{numSeconds.ToString().PadLeft(2, '0')}";
        }
    }
}
