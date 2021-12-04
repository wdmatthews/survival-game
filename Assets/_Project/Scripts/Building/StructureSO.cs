using UnityEngine;
using Project.Items;

namespace Project.Building
{
    [CreateAssetMenu(fileName = "New Structure", menuName = "Project/Building/Structure")]
    public class StructureSO : ItemSO
    {
        public float MaxHealth = 1;
        public Structure Prefab = null;
        public Transform PreviewPrefab = null;
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
