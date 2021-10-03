namespace Project.Items
{
    public abstract class UpgradableItemSO : ItemSO
    {
        public int Level = 1;
        public UpgradableItemSO ItemAtNextLevel = null;
        public CraftingIngredientStack[] IngredientsNeededToUpgrade = { };

        public void Upgrade(InventorySO inventory)
        {
            IngredientsNeededToUpgrade.RemoveFrom(inventory);
            inventory.AddItem(ItemAtNextLevel, 1);
            inventory.RemoveItem(this, 1);
        }
    }
}
