using System.Collections.Generic;
using UnityEngine;
using Project.Items;

namespace Project.Building
{
    [AddComponentMenu("Project/Building/Chest")]
    [DisallowMultipleComponent]
    public class Chest : Structure
    {
        public List<CraftingIngredientStack> Stacks = new();
        public Dictionary<CraftingIngredientSO, CraftingIngredientStack> StacksByIngredient = new();

        public void Add(CraftingIngredientSO ingredient, int amount)
        {
            if (StacksByIngredient.ContainsKey(ingredient))
            {
                StacksByIngredient[ingredient].Amount += amount;
            }
            else
            {
                CraftingIngredientStack newStack = new CraftingIngredientStack(ingredient, amount);
                Stacks.Add(newStack);
                StacksByIngredient.Add(ingredient, newStack);
            }
        }

        public void Remove(CraftingIngredientSO ingredient, int amount)
        {
            if (!StacksByIngredient.ContainsKey(ingredient)) return;
            CraftingIngredientStack stack = StacksByIngredient[ingredient];
            stack.Amount -= amount;

            if (stack.Amount <= 0)
            {
                Stacks.Remove(stack);
                StacksByIngredient.Remove(ingredient);
            }
        }
    }
}
