namespace Project.Items
{
    public abstract class ItemSO : CraftingIngredientSO
    {
        public float CooldownDuration = 1;

        public virtual void Use() { }
        public virtual void Use(Resource resource, InventorySO inventory) { }
    }
}
