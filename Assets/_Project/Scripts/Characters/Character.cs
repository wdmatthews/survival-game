using System.Collections.Generic;
using UnityEngine;
using Project.Combat;
using Project.Crafting;
using Project.Items;
using Project.Utilities;

namespace Project.Characters
{
    public class Character : Damageable
    {
        [Space]
        [Header("Object References")]
        [SerializeField] protected CharacterController _controller = null;
        [SerializeField] protected Transform _groundCheckPoint = null;
        [SerializeField] protected InventorySO _inventory = null;
        [SerializeField] protected CraftingStationSO _craftingStation = null;

        protected CharacterSO _characterData = null;
        protected Vector3 _velocity = new Vector3();
        protected Vector2 _moveDirection = new Vector2();
        protected bool _shouldJump = false;
        protected Resource _nearbyResource = null;
        protected List<MonoBehaviour> _nearbyMonsters = new List<MonoBehaviour>();
        protected ItemSO _itemInHand = null;
        protected int _hotbarIndex = 0;
        protected float _itemUseCooldownTimer = 0;
        protected bool _shouldUse = false;

        protected override void Awake()
        {
            base.Awake();
            _characterData = (CharacterSO)_data;
        }

        protected override void Update()
        {
            base.Update();

            if (_shouldUse && _itemInHand && _itemInHand.CooldownDuration > Mathf.Epsilon)
            {
                if (Mathf.Approximately(_itemUseCooldownTimer, 0)) Interact();
                else _itemUseCooldownTimer = Mathf.Clamp(_itemUseCooldownTimer - Time.deltaTime,
                    0, _itemInHand.CooldownDuration);
            }
        }

        protected void FixedUpdate()
        {
            bool isGrounded = Physics.Raycast(_groundCheckPoint.position, -transform.up,
                _characterData.GroundCheckDistance, _characterData.GroundLayers);

            _velocity.y = Mathf.Clamp(_velocity.y, _characterData.MinYVelocity, _velocity.y);

            _controller.Move(Time.fixedDeltaTime * _velocity);

            if (isGrounded) _velocity.y = 0;
            else _velocity.y += _characterData.GravityScale * Physics.gravity.y;

            if (_shouldJump && isGrounded) _velocity.y = _characterData.JumpSpeed;
        }

        protected void OnTriggerEnter(Collider other)
        {
            int colliderLayer = other.gameObject.layer;

            if (_characterData.ResourceLayers.Contains(colliderLayer))
            {
                _nearbyResource = other.GetComponent<Resource>();
            }
            else if (_characterData.MonsterLayers.Contains(colliderLayer)
                && !(other is SphereCollider))
            {
                Monster monster = other.GetComponent<Monster>();
                if (!_nearbyMonsters.Contains(monster)) _nearbyMonsters.Add(monster);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            int colliderLayer = other.gameObject.layer;

            if (_characterData.ResourceLayers.Contains(colliderLayer)
                && _nearbyResource && other.gameObject == _nearbyResource.gameObject)
            {
                _nearbyResource = null;
            }
            else if (_characterData.MonsterLayers.Contains(colliderLayer))
            {
                _nearbyMonsters.Remove(other.GetComponent<Monster>());
            }
        }

        protected void Move(Vector2 direction)
        {
            Vector3 isoDirection = MovementDirection.CartesianToIso(direction);
            _velocity = new Vector3(
                _characterData.MoveSpeed * isoDirection.x,
                _velocity.y,
                _characterData.MoveSpeed * isoDirection.z
            );

            if (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.y, 0))
            {
                float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
                transform.eulerAngles = new Vector3(0, -45 - angle, 0);
            }
        }

        protected void Use()
        {
            _shouldUse = true;
            if (!_itemInHand) return;
            _itemUseCooldownTimer = _itemInHand.CooldownDuration;
            _itemInHand.Use();
            _itemInHand.Use(this);
            _itemInHand.Use(_nearbyResource, _inventory);
            _itemInHand.Use(_nearbyMonsters);
        }

        protected void Interact()
        {

        }

        protected virtual void SetHotbarIndex(int index)
        {
            _hotbarIndex = index;
            _itemInHand = index >= 0 ? _inventory.HotbarItems[_hotbarIndex]?.Item : null;
        }

        protected virtual void CycleToNextHotbarItem()
        {
            int index = _hotbarIndex + 1;
            if (index >= _inventory.HotbarItems.Count) index = 0;
            SetHotbarIndex(index);
        }

        protected virtual void CycleToPreviousHotbarItem()
        {
            int index = _hotbarIndex - 1;
            if (index < 0) index = _inventory.HotbarItems.Count - 1;
            SetHotbarIndex(index);
        }

        protected virtual void AddToHotbar(ItemSO item, int hotbarIndex = -1)
        {
            _inventory.AddHotbarItem(item, hotbarIndex);
        }

        protected virtual void RemoveFromHotbar(int hotbarIndex)
        {
            if (_hotbarIndex == hotbarIndex && _hotbarIndex >= 0)
            {
                RemoveItemFromHand();
            }

            _inventory.RemoveHotbarItem(_inventory.HotbarItems[hotbarIndex].Item, hotbarIndex);
        }

        protected virtual void RemoveItemFromHand()
        {
            _itemInHand = null;
        }
    }
}
