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

        protected override void Awake()
        {
            base.Awake();
            _referenceToSelf.Value = this;
            _heartHUD.CreateHearts(Mathf.RoundToInt(_health));
        }

        public void Move(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<Vector2>());
        }

        public void Jump(InputAction.CallbackContext context)
        {
            _shouldJump = context.performed;
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
    }
}
