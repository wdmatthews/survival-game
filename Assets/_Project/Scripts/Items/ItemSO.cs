namespace Project.Items
{
    public abstract class ItemSO : CraftingIngredientSO
    {
        public float CooldownDuration = 0;

        public virtual void Use() { }
        public virtual void Use(Resource resource, InventorySO inventory) { }
    }
}
