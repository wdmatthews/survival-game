using Project.Items;

namespace Project.Crafting
{
    [System.Serializable]
    public class CraftingIngredientStack
    {
        public CraftingIngredientSO Ingredient = null;
        public int Amount = 0;

        public CraftingIngredientStack() { }

        public CraftingIngredientStack(CraftingIngredientSO ingredient, int amount)
        {
            Ingredient = ingredient;
            Amount = amount;
        }
    }
}
