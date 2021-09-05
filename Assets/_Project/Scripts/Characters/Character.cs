using UnityEngine;

namespace Project.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected CharacterController _controller = null;

        protected void Move(Vector2 direction)
        {
            Vector3 isoDirection = MovementDirection.CartesianToIso(direction);
            _controller.Move(isoDirection);
        }
    }
}
