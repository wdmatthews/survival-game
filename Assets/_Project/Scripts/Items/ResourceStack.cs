namespace Project.Items
{
    [System.Serializable]
    public class ResourceStack
    {
        public ResourceTypeSO Resource = null;
        public int Amount = 0;

        public ResourceStack() { }

        public ResourceStack(ResourceTypeSO resource, int amount)
        {
            Resource = resource;
            Amount = amount;
        }
    }
}
