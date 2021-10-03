namespace Project.Items
{
    public static class CraftingIngredientStackArrayExtensions
    {
        public static void AddTo(this CraftingIngredientStack[] ingredients, InventorySO inventory)
        {
            for (int i = ingredients.Length - 1; i >= 0; i--)
            {
                CraftingIngredientStack ingredientStack = ingredients[i];
                CraftingIngredientSO ingredient = ingredientStack.Ingredient;

                if (ingredient is ResourceTypeSO resource)
                {
                    inventory.AddResource(resource, ingredientStack.Amount);
                }
                else if (ingredient is ItemSO item)
                {
                    inventory.AddItem(item, ingredientStack.Amount);
                }
            }
        }

        public static void RemoveFrom(this CraftingIngredientStack[] ingredients, InventorySO inventory)
        {
            for (int i = ingredients.Length - 1; i >= 0; i--)
            {
                CraftingIngredientStack ingredientStack = ingredients[i];
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
        }
    }
}
