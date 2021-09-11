using UnityEngine;

namespace Project.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1;
        [SerializeField] protected CharacterController _controller = null;

        protected Vector2 _moveDirection = new Vector2();

        protected void FixedUpdate()
        {
            if (!Mathf.Approximately(_moveDirection.x, 0)
                || !Mathf.Approximately(_moveDirection.y, 0))
            {
                Move(_moveDirection);
            }
        }

        protected void Move(Vector2 direction)
        {
            Vector3 isoDirection = MovementDirection.CartesianToIso(direction);
            _controller.Move(Time.fixedDeltaTime * _moveSpeed * isoDirection);
        }
    }
}
