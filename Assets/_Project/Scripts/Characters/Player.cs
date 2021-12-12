using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Project.Building;
using Project.Combat;
using Project.Growing;
using Project.Items;
using Project.UI;
using Project.World;

namespace Project.Characters
{
    public class Player : Character
    {
        [SerializeField] private DamageableReferenceSO _referenceToSelf = null;
        [SerializeField] private HeartHUD _heartHUD = null;
        [SerializeField] private HotbarHUD _hotbarHUD = null;
        [SerializeField] private InventoryWindow _inventoryWindow = null;
        [SerializeField] private BuildWindow _buildWindow = null;
        [SerializeField] private UpgradeWindow _upgradeWindow = null;
        [SerializeField] private CookingWindow _cookingWindow = null;
        [SerializeField] private ChestWindow _chestWindow = null;
        [SerializeField] private CampfireWindow _campfireRenameWindow = null;
        [SerializeField] private FastTravelWindow _fastTravelWindow = null;
        [SerializeField] private SpawnPointMessage _spawnPointMessage = null;
        [SerializeField] private MonsterManagerSO _monsterManager = null;

        private bool _isPreviewingStructure = false;
        private StructureSO _previewStructureData = null;
        private Transform _previewStructure = null;
        private int _previewAngleIndex = 0;
        private float _fastTravelCooldownTimer = 0;
        private List<CampfireData> _campfires = new();
        private Vector3 _spawnPoint = new();

        protected override void Awake()
        {
            base.Awake();
            _referenceToSelf.Value = this;
            _spawnPoint = transform.position;
        }

        protected void Start()
        {
            _heartHUD.CreateHearts(Mathf.RoundToInt(_health));
            _inventoryWindow.AddItemToHotbar = AddToHotbar;
            _inventoryWindow.RemoveItemFromHotbar = RemoveFromHotbar;
            _buildWindow.BuildStructure = BuildStructure;
            _upgradeWindow.UpgradeItem = UpgradeItem;
            _cookingWindow.CookFood = CookFood;
            _chestWindow.AddToChest = AddToChest;
            _chestWindow.TakeFromChest = TakeFromChest;
            _campfireRenameWindow.RenameCampfire = RenameCampfire;
            _fastTravelWindow.FastTravel = FastTravel;
            // TESTING //
            _inventory.HotbarItems.Clear();

            foreach (var stack in _inventory.Resources)
            {
                _inventory.ResourcesByResourceData.Add(stack.Resource, stack);
            }

            foreach (var stack in _inventory.Items)
            {
                _inventory.ItemsByItemData.Add(stack.Item, stack);
                AddToHotbar(stack.Item);
            }

            SetHotbarIndex(0);

            for (int i = _inventory.HotbarItems.Count; i < 7; i++)
            {
                _inventory.HotbarItems.Add(null);
                _hotbarHUD.AddSlot();
            }
            // TESTING //
        }

        protected override void Update()
        {
            base.Update();
            _fastTravelCooldownTimer = Mathf.Clamp(_fastTravelCooldownTimer - Time.deltaTime, 0, _characterData.FastTravelCooldownDuration);
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

        protected override void Use()
        {
            base.Use();

            if (_itemInHand && _itemInHand is FoodSO)
            {
                if (!_inventory.ItemsByItemData.ContainsKey(_itemInHand)) _hotbarHUD.SetSlotItem(_hotbarIndex, null);
                else _hotbarHUD.SetSlotItem(_hotbarIndex, _itemInHand.Icon, _inventory.HotbarItems[_hotbarIndex].Amount);
            }
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

        public override void Die()
        {
            base.Die();
            Respawn();
        }

        public void SetHotbarIndex(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetHotbarIndex(int.Parse(context.control.path[10..]) - 1);
            }
        }

        public void CycleToNextHotbarItem(InputAction.CallbackContext context)
        {
            if (context.performed) CycleToNextHotbarItem();
        }

        public void CycleToPreviousHotbarItem(InputAction.CallbackContext context)
        {
            if (context.performed) CycleToPreviousHotbarItem();
        }

        public void ToggleInventoryWindow(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!_inventoryWindow.IsOpen) _inventoryWindow.Open();
            else _inventoryWindow.Close();
        }

        protected override void SetHotbarIndex(int index)
        {
            if (_hotbarIndex >= 0) _hotbarHUD.SetSlotSelected(_hotbarIndex, false);
            base.SetHotbarIndex(index);
            _hotbarHUD.SetSlotSelected(index, true);
        }

        protected override void AddToHotbar(ItemSO item, int hotbarIndex = -1)
        {
            base.AddToHotbar(item, hotbarIndex);
            int index = hotbarIndex;

            if (hotbarIndex < 0)
            {
                index = _inventory.HotbarItems.Count - 1;
                _hotbarHUD.AddSlot();
            }

            _hotbarHUD.SetSlotItem(index, item.Icon, _inventory.HotbarItems[index].Amount);
        }

        protected override void RemoveFromHotbar(int hotbarIndex)
        {
            base.RemoveFromHotbar(hotbarIndex);
            _hotbarHUD.SetSlotItem(hotbarIndex, null);
        }

        protected override void Interact()
        {
            base.Interact();

            if (_nearbyChest) _chestWindow.Open(_nearbyChest);
            else if (_nearbyWorkstation)
            {
                string workstationName = _nearbyWorkstation.Name;
                if (workstationName == "Cooking Spit") _cookingWindow.Open();
                else if (workstationName == "Anvil") _upgradeWindow.Open();
            }
            else if (_nearbyStructureNode) _buildWindow.Open();
            else if (_nearbyTent) SetSpawnPoint(_nearbyTent);
            else if (_nearbyCampfire) _campfireRenameWindow.Open(GetCampfire(_nearbyCampfire.name));
        }

        protected override void OnMovedAwayFromStructureNode()
        {
            base.OnMovedAwayFromStructureNode();
            _buildWindow.Close();

            if (_isPreviewingStructure)
            {
                _nearbyStructureNode.EndPreview();
                EndPreview();
            }
        }

        protected override void OnMovedAwayFromWorkstation()
        {
            base.OnMovedAwayFromWorkstation();
            string workstationName = _nearbyWorkstation.Name;
            if (workstationName == "Cooking Spit") _cookingWindow.Close();
            else if (workstationName == "Anvil") _upgradeWindow.Close();
        }

        protected override void OnMovedAwayFromChest()
        {
            base.OnMovedAwayFromChest();
            _chestWindow.Close();
        }

        public void NextAngleIndex(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isPreviewingStructure) return;
            _previewAngleIndex += 1;
            if (_previewAngleIndex >= 4) _previewAngleIndex = 0;
            _previewStructure.localEulerAngles = new Vector3(0, _previewAngleIndex * 90, 0);
        }

        public void PreviousAngleIndex(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isPreviewingStructure) return;
            _previewAngleIndex -= 1;
            if (_previewAngleIndex < 0) _previewAngleIndex = 3;
            _previewStructure.localEulerAngles = new Vector3(0, _previewAngleIndex * 90, 0);
        }

        private void BuildStructure(StructureSO structure)
        {
            _buildWindow.Close();
            _isPreviewingStructure = true;
            _previewAngleIndex = 0;
            _previewStructureData = structure;
            _previewStructure = _nearbyStructureNode.StartPreview(structure);
        }

        private void EndPreview()
        {
            _isPreviewingStructure = false;
            _previewStructureData = null;
            _previewStructure = null;
            _nearbyStructureNode = null;
        }

        public void CancelBuild(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isPreviewingStructure) return;
            _nearbyStructureNode.EndPreview();
            EndPreview();
        }

        public void ConfirmBuild(InputAction.CallbackContext context)
        {
            if (!context.performed || !_isPreviewingStructure) return;
            _nearbyStructureNode.ConfirmPreview(_previewAngleIndex, _inventory);

            if (_previewStructureData.name == "Campfire")
            {
                CampfireData campfire = new CampfireData("Unnamed Campfire",
                    _previewStructure.position + _previewStructure.right + new Vector3(0, 0.5f, 0));
                _campfires.Add(campfire);
                _nearbyStructureNode.BuiltStructure.name = campfire.ID;
            }

            EndPreview();
        }

        private void UpgradeItem(UpgradableItemSO itemToUpgrade)
        {
            foreach (CraftingIngredientStack stack in itemToUpgrade.IngredientsNeededToUpgrade)
            {
                if (stack.Ingredient is ResourceTypeSO resource)
                {
                    _inventory.RemoveResource(resource, stack.Amount);
                }
                else if (stack.Ingredient is ItemSO item
                    && item != itemToUpgrade)
                {
                    _inventory.RemoveItem(item, stack.Amount);
                }
            }

            int itemHotbarIndex = _inventory.GetHotbarItemIndex(itemToUpgrade);
            RemoveFromHotbar(itemHotbarIndex);
            _inventory.RemoveItem(itemToUpgrade, 1);
            _inventory.AddItem(itemToUpgrade.ItemAtNextLevel, 1);
            AddToHotbar(itemToUpgrade.ItemAtNextLevel, itemHotbarIndex);
            _upgradeWindow.Close();
        }

        private void RemoveItemFromInventory(ItemSO item, int amount)
        {
            int itemHotbarIndex = _inventory.GetHotbarItemIndex(item);

            if (itemHotbarIndex < 0 || _inventory.ItemsByItemData[item].Amount > amount)
            {
                _inventory.RemoveItem(item, amount);

                if (itemHotbarIndex >= 0)
                {
                    _hotbarHUD.SetSlotItem(itemHotbarIndex, item.Icon, _inventory.HotbarItems[itemHotbarIndex].Amount);
                }
            }
            else
            {
                RemoveFromHotbar(itemHotbarIndex);
                _inventory.RemoveItem(item, amount);
            }
        }

        private void CookFood(FoodSO itemToCook)
        {
            foreach (FoodStack stack in itemToCook.FoodNeededToCook)
            {
                RemoveItemFromInventory(stack.Food, stack.Amount);
            }

            _inventory.AddItem(itemToCook, 1);
            _cookingWindow.Close();
        }
        
        private void AddToChest(CraftingIngredientSO ingredient, int amount)
        {
            if (ingredient is ResourceTypeSO resource) _inventory.RemoveResource(resource, amount);
            else if (ingredient is ItemSO item)
            {
                RemoveItemFromInventory(item, amount);
            }

            _nearbyChest.Add(ingredient, amount);
            _chestWindow.Open(_nearbyChest);
        }

        private void TakeFromChest(CraftingIngredientSO ingredient, int amount)
        {
            _nearbyChest.Remove(ingredient, amount);
            if (ingredient is ResourceTypeSO resource) _inventory.AddResource(resource, amount);
            else if (ingredient is ItemSO item) _inventory.AddItem(item, amount);
            _chestWindow.Open(_nearbyChest);
        }

        public void FastTravel(InputAction.CallbackContext context)
        {
            if (!context.performed || !Mathf.Approximately(_fastTravelCooldownTimer, 0)) return;
            _fastTravelWindow.Open(_campfires);
        }

        private void FastTravel(CampfireData campfire)
        {
            _controller.enabled = false;
            transform.position = campfire.Position;
            _controller.enabled = true;
            _velocity = new Vector3();
            _fastTravelCooldownTimer = _characterData.FastTravelCooldownDuration;
            _fastTravelWindow.Close();
        }

        private CampfireData GetCampfire(string id)
        {
            foreach (CampfireData campfire in _campfires)
            {
                if (campfire.ID == id) return campfire;
            }

            return null;
        }

        private void RenameCampfire(CampfireData campfire, string newName)
        {
            campfire.Name = newName;
            _campfireRenameWindow.Close();
        }

        private void SetSpawnPoint(Transform tent)
        {
            _spawnPoint = tent.position + tent.right + new Vector3(0, 0.5f, 0);
            _spawnPointMessage.Open();
        }

        private void Respawn()
        {
            int hotbarItemCount = _inventory.HotbarItems.Count;

            for (int i = 0; i < hotbarItemCount; i++)
            {
                _hotbarHUD.SetSlotItem(i, null);
            }

            _isDead = false;
            _health = _data.MaxHealth;
            _heartHUD.OnHealthChanged(_health);
            _controller.enabled = false;
            transform.position = _spawnPoint;
            _controller.enabled = true;
            _velocity = new Vector3();
            _inventory.Empty();

            for (int i = _inventory.InitialItems.Count - 1; i >= 0; i--)
            {
                ItemStack stack = _inventory.InitialItems[i];
                _inventory.AddItem(stack.Item, stack.Amount);
                AddToHotbar(stack.Item, i);
            }

            _monsterManager.KillAllMonsters();
        }
    }
}
