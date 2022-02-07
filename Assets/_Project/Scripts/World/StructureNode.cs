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
        [SerializeField] private Collider _collider = null;

        private Structure _builtStructure = null;
        private Transform _previewStructure = null;
        private StructureSO _previewStructureData = null;
        private bool _isShown = true;

        public Structure BuiltStructure => _builtStructure;

        private void Update()
        {
            if (!_builtStructure && !_isShown)
            {
                _isShown = true;
                _renderer.enabled = true;
                _collider.enabled = true;
            }
        }

        public void Build(StructureSO structure, int angleIndex, InventorySO inventory)
        {
            _builtStructure = structure.Build(transform.position, angleIndex, inventory);
            _isShown = false;
            _renderer.enabled = false;
            _collider.enabled = false;
        }

        public Transform StartPreview(StructureSO structure)
        {
            _previewStructure = Instantiate(structure.PreviewPrefab);
            _previewStructure.transform.position = transform.position;
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
