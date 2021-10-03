namespace Project.Items
{
    public abstract class UpgradableItemSO : ItemSO
    {
        public int Level = 1;
        public UpgradableItemSO ItemAtNextLevel = null;
        public CraftingIngredientStack[] IngredientsNeededToUpgrade = { };

        public void Upgrade(InventorySO inventory)
        {
            for (int i = IngredientsNeededToUpgrade.Length - 1; i >= 0; i--)
            {
                CraftingIngredientStack ingredientStack = IngredientsNeededToUpgrade[i];
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

            inventory.AddItem(ItemAtNextLevel, 1);
            inventory.RemoveItem(this, 1);
        }
    }
}
