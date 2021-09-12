namespace Project.Items
{
    [System.Serializable]
    public class ItemStack
    {
        public ItemSO Item = null;
        public int Amount = 0;

        public ItemStack(ItemSO item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}
