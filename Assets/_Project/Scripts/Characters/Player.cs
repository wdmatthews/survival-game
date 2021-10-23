using UnityEngine;
using UnityEngine.InputSystem;
using Project.Combat;

namespace Project.Characters
{
    public class Player : Character
    {
        [SerializeField] private DamageableReferenceSO _referenceToSelf = null;

        protected override void Awake()
        {
            base.Awake();
            _referenceToSelf.Value = this;
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
    }
}
