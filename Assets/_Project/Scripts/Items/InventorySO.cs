using System.Collections.Generic;
using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Project/Items/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public List<ResourceStack> Resources = new();
        public List<ItemStack> Items = new();
        public List<ItemStack> HotbarItems = new();
        public Dictionary<ResourceTypeSO, ResourceStack> ResourcesByResourceData = new();
        public Dictionary<ItemSO, ItemStack> ItemsByItemData = new();
        public Dictionary<ItemSO, ItemStack> HotbarItemsByItemData = new();

        [System.NonSerialized] private List<CraftingIngredientStack> _stacks = new();

        public void AddResource(ResourceTypeSO resource, int amount)
        {
            if (ResourcesByResourceData.ContainsKey(resource))
            {
                ResourcesByResourceData[resource].Amount += amount;
            }
            else
            {
                ResourceStack newStack = new ResourceStack(resource, amount);
                Resources.Add(newStack);
                ResourcesByResourceData.Add(resource, newStack);
            }
        }

        public void RemoveResource(ResourceTypeSO resource, int amount)
        {
            if (!ResourcesByResourceData.ContainsKey(resource)) return;
            ResourceStack stack = ResourcesByResourceData[resource];
            stack.Amount -= amount;

            if (stack.Amount <= 0)
            {
                Resources.Remove(stack);
                ResourcesByResourceData.Remove(resource);
            }
        }

        public void AddItem(ItemSO item, int amount)
        {
            if (ItemsByItemData.ContainsKey(item))
            {
                ItemsByItemData[item].Amount += amount;
            }
            else
            {
                ItemStack newStack = new ItemStack(item, amount);
                Items.Add(newStack);
                ItemsByItemData.Add(item, newStack);
            }
        }

        public void RemoveItem(ItemSO item, int amount)
        {
            if (!ItemsByItemData.ContainsKey(item)) return;
            ItemStack stack = ItemsByItemData[item];
            stack.Amount -= amount;

            if (stack.Amount <= 0)
            {
                if (HotbarItemsByItemData.ContainsKey(item)) RemoveHotbarItem(item);
                Items.Remove(stack);
                ItemsByItemData.Remove(item);
            }
        }

        public void AddHotbarItem(ItemSO item, int index = -1)
        {
            ItemStack stack = ItemsByItemData[item];
            if (index < 0) HotbarItems.Add(stack);
            else HotbarItems[index] = stack;
            HotbarItemsByItemData.Add(item, stack);
        }

        public void RemoveHotbarItem(ItemSO item, int index = -1)
        {
            ItemStack stack = HotbarItemsByItemData[item];
            if (index < 0) HotbarItems.Remove(stack);
            else HotbarItems[index] = null;
            HotbarItemsByItemData.Remove(item);
        }

        public int GetHotbarItemIndex(ItemSO item)
        {
            for (int i = HotbarItems.Count - 1; i >= 0; i--)
            {
                if (HotbarItems[i] != null && HotbarItems[i].Item == item) return i;
            }

            return -1;
        }

        public List<CraftingIngredientStack> GetChestCompatibleStacks()
        {
            _stacks.Clear();

            foreach (ResourceStack resource in Resources)
            {
                _stacks.Add(new CraftingIngredientStack(resource.Resource, resource.Amount));
            }

            foreach (ItemStack item in Items)
            {
                if (!item.Item.CanBeInChest) continue;
                _stacks.Add(new CraftingIngredientStack(item.Item, item.Amount));
            }

            return _stacks;
        }
    }
}
