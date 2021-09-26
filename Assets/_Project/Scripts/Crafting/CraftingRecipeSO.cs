using UnityEngine;
using Project.Items;

namespace Project.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Project/Crafting/Crafting Recipe")]
    public class CraftingRecipeSO : ScriptableObject
    {
        public CraftingIngredientStack[] Ingredients = { };
        public ItemStack Product = new ItemStack(null, 0);

        public bool CanBeCrafted(InventorySO inventory)
        {
            for (int i = Ingredients.Length - 1; i >= 0; i--)
            {
                CraftingIngredientStack ingredientStack = Ingredients[i];
                CraftingIngredientSO ingredient = ingredientStack.Ingredient;

                if (ingredient is ResourceTypeSO resource)
                {
                    if (!inventory.ResourcesByResourceData.ContainsKey(resource)
                        || inventory.ResourcesByResourceData[resource].Amount < ingredientStack.Amount) return false;
                }
                else if (ingredient is ItemSO item)
                {
                    if (!inventory.ItemsByItemData.ContainsKey(item)
                        || inventory.ItemsByItemData[item].Amount < ingredientStack.Amount) return false;
                }
            }

            return true;
        }
    }
}
