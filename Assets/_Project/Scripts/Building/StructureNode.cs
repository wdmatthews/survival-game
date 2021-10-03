using UnityEngine;
using Project.Items;

namespace Project.Building
{
    [AddComponentMenu("Project/Building/Structure Node")]
    [DisallowMultipleComponent]
    public class StructureNode : MonoBehaviour
    {
        public void Build(StructureSO structure, int angleIndex, InventorySO inventory)
        {
            structure.Build(transform.position, angleIndex, inventory);
        }
    }
}
