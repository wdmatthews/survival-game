using UnityEngine;
using Project.Items;

namespace Project.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Station", menuName = "Project/Crafting/Crafting Station")]
    public class CraftingStationSO : ScriptableObject
    {
        public CraftingRecipeSO[] Recipes = { };

        public void Craft(CraftingRecipeSO recipe, InventorySO inventory)
        {
            for (int i = recipe.Ingredients.Length - 1; i >= 0; i--)
            {
                CraftingIngredientStack ingredientStack = recipe.Ingredients[i];
                CraftingIngredientSO ingredient = ingredientStack.Ingredient;

                if (ingredient is ResourceTypeSO resource)
                {
                    inventory.RemoveResource(resource, ingredientStack.Amount);
                }
                else if (ingredient is ItemSO item)
                {
                    inventory.RemoveItem(item, ingredientStack.Amount);
                }
            }

            inventory.AddItem(recipe.Product.Item, recipe.Product.Amount);
        }
    }
}
