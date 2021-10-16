using UnityEngine;
using Project.Items;

namespace Project.Building
{
    [CreateAssetMenu(fileName = "New Structure", menuName = "Project/Building/Structure")]
    public class StructureSO : ScriptableObject
    {
        public Structure Prefab = null;
        public CraftingIngredientStack[] Ingredients = { };

        public Structure Build(Vector3 position, int angleIndex, InventorySO inventory)
        {
            Ingredients.RemoveFrom(inventory);
            Structure structure = Instantiate(Prefab);
            structure.Place(position, angleIndex);
            return structure;
        }
    }
}
