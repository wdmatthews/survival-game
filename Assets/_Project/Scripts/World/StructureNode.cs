using UnityEngine;
using Project.Building;
using Project.Items;

namespace Project.World
{
    [AddComponentMenu("Project/World/Structure Node")]
    [DisallowMultipleComponent]
    public class StructureNode : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer = null;

        private Structure _builtStructure = null;
        private Transform _previewStructure = null;
        private StructureSO _previewStructureData = null;

        public Structure BuiltStructure => _builtStructure;

        public void Build(StructureSO structure, int angleIndex, InventorySO inventory)
        {
            _builtStructure = structure.Build(transform.position, angleIndex, inventory);
            gameObject.SetActive(false);
        }

        public Transform StartPreview(StructureSO structure)
        {
            _previewStructure = Instantiate(structure.PreviewPrefab, transform);
            _previewStructureData = structure;
            _renderer.enabled = false;
            return _previewStructure;
        }

        public void EndPreview()
        {
            _previewStructureData = null;
            Destroy(_previewStructure.gameObject);
            _renderer.enabled = true;
        }

        public void ConfirmPreview(int angleIndex, InventorySO inventory)
        {
            StructureSO structure = _previewStructureData;
            EndPreview();
            Build(structure, angleIndex, inventory);
        }
    }
}
