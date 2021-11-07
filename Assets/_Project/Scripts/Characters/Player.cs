using UnityEngine;
using UnityEngine.InputSystem;
using Project.Combat;
using Project.UI;

namespace Project.Characters
{
    public class Player : Character
    {
        [SerializeField] private DamageableReferenceSO _referenceToSelf = null;
        [SerializeField] private HeartHUD _heartHUD = null;
        [SerializeField] private HotbarHUD _hotbarHUD = null;

        protected override void Awake()
        {
            base.Awake();
            _referenceToSelf.Value = this;
        }

        protected void Start()
        {
            _heartHUD.CreateHearts(Mathf.RoundToInt(_health));
            // TESTING //
            _inventory.HotbarItems.Clear();

            foreach (var stack in _inventory.Items)
            {
                _inventory.ItemsByItemData.Add(stack.Item, stack);
                AddToHotbar(stack.Item);
            }

            SetHotbarIndex(0);
            // TESTING //
        }

        public void Move(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<Vector2>());
        }

        public void Jump(InputAction.CallbackContext context)
        {
            _shouldJump = context.performed;
            // TESTING //
            if (context.performed)
            {
                RemoveFromHotbar(_hotbarIndex);
            }
            // TESTING //
        }

        public void Use(InputAction.CallbackContext context)
        {
            if (context.performed) Use();
            else if (context.canceled) _shouldUse = false;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed) Interact();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            _heartHUD.OnHealthChanged(_health);
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
            _heartHUD.OnHealthChanged(_health);
        }

        public void SetHotbarIndex(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetHotbarIndex(int.Parse(context.control.path[10..]) - 1);
            }
        }

        public void CycleToNextHotbarItem(InputAction.CallbackContext context)
        {
            if (context.performed) CycleToNextHotbarItem();
        }

        public void CycleToPreviousHotbarItem(InputAction.CallbackContext context)
        {
            if (context.performed) CycleToPreviousHotbarItem();
        }

        protected override void SetHotbarIndex(int index)
        {
            if (_hotbarIndex >= 0) _hotbarHUD.SetSlotSelected(_hotbarIndex, false);
            base.SetHotbarIndex(index);
            _hotbarHUD.SetSlotSelected(index, true);
        }

        protected override void AddToHotbar(Items.ItemSO item, int hotbarIndex = -1)
        {
            base.AddToHotbar(item, hotbarIndex);
            int index = hotbarIndex;

            if (hotbarIndex < 0)
            {
                index = _inventory.HotbarItems.Count - 1;
                _hotbarHUD.AddSlot();
            }

            _hotbarHUD.SetSlotItem(index, item.Icon, _inventory.HotbarItems[index].Amount);
        }

        protected override void RemoveFromHotbar(int hotbarIndex)
        {
            base.RemoveFromHotbar(hotbarIndex);
            _hotbarHUD.SetSlotItem(hotbarIndex, null);
        }
    }
}
