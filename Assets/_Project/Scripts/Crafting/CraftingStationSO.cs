using UnityEngine;
using Project.Items;

namespace Project.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Station", menuName = "Project/Crafting/Crafting Station")]
    public class CraftingStationSO : ItemSO
    {
        public CraftingRecipeSO[] Recipes = { };

        public void Craft(CraftingRecipeSO recipe, InventorySO inventory)
        {
            recipe.Ingredients.RemoveFrom(inventory);
            inventory.AddItem(recipe.Product.Item, recipe.Product.Amount);
        }
    }
}
