using UnityEngine;

namespace Project.Items
{
    public abstract class CraftingIngredientSO : ScriptableObject
    {
        public Sprite Icon = null;
        public string Description = "";
        public bool CanBeInHotbar = false;
        public bool CanBeInChest = false;
    }
}
