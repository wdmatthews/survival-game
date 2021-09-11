using UnityEngine;

namespace Project.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected float _moveSpeed = 1;
        [SerializeField] protected float _jumpSpeed = 1;
        [SerializeField] protected float _gravityScale = 1;
        [SerializeField] protected float _groundCheckDistance = 0.1f;
        [SerializeField] protected LayerMask _groundLayers = 0;
        [SerializeField] protected CharacterController _controller = null;
        [SerializeField] protected Transform _groundCheckPoint = null;

        protected Vector3 _velocity = new Vector3();
        protected Vector2 _moveDirection = new Vector2();
        protected bool _shouldJump = false;

        protected void FixedUpdate()
        {
            bool isGrounded = Physics.Raycast(_groundCheckPoint.position, -transform.up, _groundCheckDistance, _groundLayers);

            _controller.Move(Time.fixedDeltaTime * _velocity);

            if (!isGrounded) _velocity.y += _gravityScale * Physics.gravity.y;
            else _velocity.y = 0;

            if (_shouldJump && isGrounded)
            {
                _velocity.y = _jumpSpeed;
            }
        }

        protected void Move(Vector2 direction)
        {
            Vector3 isoDirection = MovementDirection.CartesianToIso(direction);
            _velocity = new Vector3(
                _moveSpeed * isoDirection.x,
                _velocity.y,
                _moveSpeed * isoDirection.z
            );
        }
    }
}
