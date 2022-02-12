using System.Collections.Generic;
using UnityEngine;
using Project.Building;
using Project.Combat;
using Project.Growing;
using Project.Items;
using Project.Utilities;
using Project.World;

namespace Project.Characters
{
    public class Character : Damageable
    {
        [Space]
        [Header("Object References")]
        [SerializeField] protected CharacterController _controller = null;
        [SerializeField] protected Transform _groundCheckPoint = null;
        [SerializeField] protected Transform _hand = null;
        [SerializeField] protected InventorySO _inventory = null;
        [SerializeField] protected Animator _animator = null;

        protected CharacterSO _characterData = null;
        protected Vector3 _velocity = new Vector3();
        protected Vector2 _moveDirection = new Vector2();
        protected bool _shouldJump = false;
        protected Resource _nearbyResource = null;
        protected List<MonoBehaviour> _nearbyMonsters = new List<MonoBehaviour>();
        protected ItemSO _itemInHand = null;
        protected Transform _objectInHand = null;
        protected int _hotbarIndex = 0;
        protected bool _shouldUse = false;
        protected bool _justStartedUsing = false;
        protected StructureNode _nearbyStructureNode = null;
        protected List<Crop> _nearbyCrops = new List<Crop>();
        protected Workstation _nearbyWorkstation = null;
        protected Chest _nearbyChest = null;
        protected Transform _nearbyCampfire = null;
        protected Transform _nearbyTent = null;

        protected override void Awake()
        {
            base.Awake();
            _characterData = (CharacterSO)_data;
        }

        protected override void Update()
        {
            base.Update();

            _animator.SetBool("Is Moving", !Mathf.Approximately(_velocity.x, 0) || !Mathf.Approximately(_velocity.z, 0));

            if (_shouldUse && _itemInHand)
            {
                if (_justStartedUsing) _justStartedUsing = false;
                else Use();
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
            else if (_characterData.StructureNodeLayers.Contains(colliderLayer))
            {
                _nearbyStructureNode = other.GetComponent<StructureNode>();
            }
            else if (_characterData.CropLayers.Contains(colliderLayer))
            {
                Crop crop = other.GetComponent<Crop>();
                if (!_nearbyCrops.Contains(crop)) _nearbyCrops.Add(crop);
            }
            else if (_characterData.WorkstationLayers.Contains(colliderLayer))
            {
                _nearbyWorkstation = other.GetComponent<Workstation>();
            }
            else if (_characterData.ChestLayers.Contains(colliderLayer))
            {
                _nearbyChest = other.GetComponent<Chest>();
            }
            else if (_characterData.CampfireLayers.Contains(colliderLayer))
            {
                _nearbyCampfire = other.transform;
            }
            else if (_characterData.TentLayers.Contains(colliderLayer))
            {
                _nearbyTent = other.transform;
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
            else if (_characterData.StructureNodeLayers.Contains(colliderLayer)
                && _nearbyStructureNode && other.gameObject == _nearbyStructureNode.gameObject)
            {
                OnMovedAwayFromStructureNode();
                _nearbyStructureNode = null;
            }
            else if (_characterData.CropLayers.Contains(colliderLayer))
            {
                _nearbyCrops.Remove(other.GetComponent<Crop>());
            }
            else if (_characterData.WorkstationLayers.Contains(colliderLayer))
            {
                OnMovedAwayFromWorkstation();
                _nearbyWorkstation = null;
            }
            else if (_characterData.ChestLayers.Contains(colliderLayer))
            {
                OnMovedAwayFromChest();
                _nearbyChest = null;
            }
            else if (_characterData.CampfireLayers.Contains(colliderLayer)
                && _nearbyCampfire && other.transform == _nearbyCampfire)
            {
                _nearbyCampfire = null;
            }
            else if (_characterData.TentLayers.Contains(colliderLayer)
                && _nearbyTent && other.transform == _nearbyTent)
            {
                _nearbyTent = null;
            }
        }

        protected void Move(Vector2 direction, bool updateAngle = true)
        {
            Vector3 isoDirection = MovementDirection.CartesianToIso(direction);
            _velocity = new Vector3(
                _characterData.MoveSpeed * isoDirection.x,
                _velocity.y,
                _characterData.MoveSpeed * isoDirection.z
            );

            if (updateAngle &&
                (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.y, 0)))
            {
                Aim(direction);
            }
        }

        protected void Aim(Vector2 direction, float offsetAngle = -45)
        {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            transform.eulerAngles = new Vector3(0, offsetAngle - angle, 0);
        }

        protected virtual void Use()
        {
            if (_animator.GetCurrentAnimatorStateInfo(1).IsName("Use")) return;

            _shouldUse = true;
            _justStartedUsing = true;

            foreach (var crop in _nearbyCrops)
            {
                crop.Harvest(_inventory);
            }

            if (!_itemInHand) return;
            _animator.ResetTrigger("Use");
            _animator.SetTrigger("Use");
            _itemInHand.Use();
            _itemInHand.Use(this);
            _itemInHand.Use(_nearbyResource, _inventory);
            
            if (_itemInHand is FoodSO) _inventory.RemoveItem(_itemInHand, 1);
        }

        public void UseInAnimation()
        {
            if (!_itemInHand) return;
            _itemInHand.Use(_nearbyMonsters);
        }

        protected virtual void Interact()
        {
            
        }

        protected virtual void SetHotbarIndex(int index)
        {
            int oldIndex = _hotbarIndex;
            _hotbarIndex = index;
            _itemInHand = index >= 0 ? _inventory.HotbarItems[_hotbarIndex]?.Item : null;
            if (!_hand) return;
            bool hadObjectInHand = _objectInHand;

            if (oldIndex != _hotbarIndex && hadObjectInHand)
            {
                Destroy(_objectInHand.gameObject);
            }

            if ((oldIndex != _hotbarIndex || !hadObjectInHand) && _itemInHand && _itemInHand.PhysicalItem)
            {
                _objectInHand = Instantiate(_itemInHand.PhysicalItem, _hand);
            }
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

        protected virtual void OnMovedAwayFromStructureNode()
        {

        }

        protected virtual void OnMovedAwayFromWorkstation()
        {

        }

        protected virtual void OnMovedAwayFromChest()
        {

        }
    }
}
