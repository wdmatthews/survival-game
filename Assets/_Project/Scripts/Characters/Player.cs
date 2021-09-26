using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Characters
{
    public class Player : Character
    {
        public void Move(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<Vector2>());
        }

        public void Jump(InputAction.CallbackContext context)
        {
            _shouldJump = context.performed;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed) Interact();
            else if (context.canceled) _shouldInteract = false;
        }
    }
}
