using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Heart HUD")]
    [DisallowMultipleComponent]
    public class HeartHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _heartTemplate = null;
        [SerializeField] private Sprite _fullSprite = null;
        [SerializeField] private Sprite _halfSprite = null;
        [SerializeField] private Sprite _emptySprite = null;

        private VisualElement _heartBar = null;
        private List<VisualElement> _hearts = new List<VisualElement>();
        private int _heartCount = 0;

        private void Awake()
        {
            _heartBar = _uiDocument.rootVisualElement.Q("HeartBar");
        }

        public void CreateHearts(int count)
        {
            _heartCount = count;

            for (int i = 0; i < count; i++)
            {
                VisualElement heart = _heartTemplate.Instantiate().ElementAt(0);
                heart.style.backgroundImage = new StyleBackground(_fullSprite);
                _heartBar.Add(heart);
                _hearts.Add(heart);
            }
        }

        public void OnHealthChanged(float health)
        {
            int fullHeartCount = Mathf.FloorToInt(health);

            for (int i = 0; i < _heartCount; i++)
            {
                VisualElement heart = _hearts[i];
                Sprite heartSprite = null;
                if (i < fullHeartCount) heartSprite = _fullSprite;
                else if (i < health) heartSprite = _halfSprite;
                else heartSprite = _emptySprite;
                heart.style.backgroundImage = new StyleBackground(heartSprite);
            }
        }
    }
}
