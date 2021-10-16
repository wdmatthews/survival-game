using UnityEngine;
using Project.Building;
using Project.Items;

namespace Project.World
{
    [AddComponentMenu("Project/World/Structure Node")]
    [DisallowMultipleComponent]
    public class StructureNode : MonoBehaviour
    {
        private Structure _builtStructure = null;

        public Structure BuiltStructure => _builtStructure;

        public void Build(StructureSO structure, int angleIndex, InventorySO inventory)
        {
            _builtStructure = structure.Build(transform.position, angleIndex, inventory);
        }
    }
}
