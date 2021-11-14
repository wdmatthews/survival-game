using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    [AddComponentMenu("Project/UI/Hotbar HUD")]
    [DisallowMultipleComponent]
    public class HotbarHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument = null;
        [SerializeField] private VisualTreeAsset _hotbarSlotTemplate = null;

        private VisualElement _hotbar = null;
        private List<HotbarSlotUI> _slots = new List<HotbarSlotUI>();

        private void Awake()
        {
            _hotbar = _uiDocument.rootVisualElement.Q("Hotbar");
        }

        public void AddSlot()
        {
            VisualElement slotElement = _hotbarSlotTemplate.Instantiate().ElementAt(0);
            HotbarSlotUI slot = new HotbarSlotUI(slotElement,
                slotElement.Q("ItemIcon"), slotElement.Q<Label>("AmountLabel"));
            slot.SetItem();
            slot.SetSelected(false);
            _slots.Add(slot);
            _hotbar.Add(slotElement);
        }

        public void SetSlotItem(int index, Sprite icon = null, int amount = 0)
        {
            _slots[index].SetItem(icon, amount);
        }

        public void SetSlotItemAmount(int index, int amount)
        {
            _slots[index].SetItemAmount(amount);
        }

        public void SetSlotSelected(int index, bool isSelected)
        {
            _slots[index].SetSelected(isSelected);
        }
    }
}
