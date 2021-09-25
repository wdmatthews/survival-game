using UnityEngine;
using Project.Items;
using Project.Utitilities;

namespace Project.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected float _moveSpeed = 1;
        [SerializeField] protected float _jumpSpeed = 1;
        [SerializeField] protected float _gravityScale = 1;
        [SerializeField] protected float _minYVelocity = -1;
        [SerializeField] protected float _groundCheckDistance = 0.1f;
        [SerializeField] protected LayerMask _groundLayers = 0;
        [SerializeField] protected LayerMask _resourceLayers = 0;
        [SerializeField] protected CharacterController _controller = null;
        [SerializeField] protected Transform _groundCheckPoint = null;
        [SerializeField] protected InventorySO _inventory = null;

        protected Vector3 _velocity = new Vector3();
        protected Vector2 _moveDirection = new Vector2();
        protected bool _shouldJump = false;
        protected Resource _nearbyResource = null;
        [SerializeField] protected ItemSO _itemInHand = null;

        protected void FixedUpdate()
        {
            bool isGrounded = Physics.Raycast(_groundCheckPoint.position, -transform.up,
                _groundCheckDistance, _groundLayers);

            _velocity.y = Mathf.Clamp(_velocity.y, _minYVelocity, _velocity.y);

            _controller.Move(Time.fixedDeltaTime * _velocity);

            if (isGrounded) _velocity.y = 0;
            else _velocity.y += _gravityScale * Physics.gravity.y;

            if (_shouldJump && isGrounded) _velocity.y = _jumpSpeed;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (_resourceLayers.Contains(other.gameObject.layer))
            {
                _nearbyResource = other.GetComponent<Resource>();
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (_resourceLayers.Contains(other.gameObject.layer)
                && _nearbyResource && other.gameObject == _nearbyResource.gameObject)
            {
                _nearbyResource = null;
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

            if (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.y, 0))
            {
                float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
                transform.eulerAngles = new Vector3(0, -45 - angle, 0);
            }
        }

        protected void Interact()
        {
            if (!_itemInHand) return;
            _itemInHand.Use();
            _itemInHand.Use(_nearbyResource);
        }
    }
}
