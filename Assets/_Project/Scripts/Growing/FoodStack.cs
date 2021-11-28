namespace Project.Growing
{
    [System.Serializable]
    public class FoodStack
    {
        public FoodSO Food = null;
        public int Amount = 0;

        public FoodStack() { }

        public FoodStack(FoodSO food, int amount)
        {
            Food = food;
            Amount = amount;
        }
    }
}
