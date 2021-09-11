using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Characters
{
    public class Player : Character
    {
        public void Move(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }
}
