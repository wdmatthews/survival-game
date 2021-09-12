using System.Collections.Generic;
using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Project/Items/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public List<ItemStack> Stacks = new List<ItemStack>();
        public List<ItemStack> HotbarStacks = new List<ItemStack>();
        public Dictionary<ItemSO, ItemStack> StacksByItemData = new Dictionary<ItemSO, ItemStack>();
        public Dictionary<ItemSO, ItemStack> HotbarStacksByItemData = new Dictionary<ItemSO, ItemStack>();

        public void AddItem(ItemSO item, int amount)
        {
            if (StacksByItemData.ContainsKey(item))
            {
                StacksByItemData[item].Amount += amount;
            }
            else
            {
                ItemStack newStack = new ItemStack(item, amount);
                Stacks.Add(newStack);
                StacksByItemData.Add(item, newStack);
            }
        }

        public void RemoveItem(ItemSO item, int amount)
        {
            if (StacksByItemData.ContainsKey(item))
            {
                ItemStack stack = StacksByItemData[item];
                stack.Amount -= amount;

                if (stack.Amount <= 0)
                {
                    Stacks.Remove(stack);
                    StacksByItemData.Remove(item);
                }
            }
        }

        public void AddHotbarItem(ItemSO item)
        {
            ItemStack stack = StacksByItemData[item];
            HotbarStacks.Add(stack);
            HotbarStacksByItemData.Add(item, stack);
        }

        public void RemoveHotbarItem(ItemSO item)
        {
            ItemStack stack = HotbarStacksByItemData[item];
            HotbarStacks.Remove(stack);
            HotbarStacksByItemData.Remove(item);
        }
    }
}
