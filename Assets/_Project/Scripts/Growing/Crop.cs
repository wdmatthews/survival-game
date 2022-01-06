using UnityEngine;
using Project.Building;
using Project.Items;

namespace Project.Growing
{
    [AddComponentMenu("Project/Growing/Crop")]
    [DisallowMultipleComponent]
    public class Crop : Structure
    {
        [SerializeField] protected MeshFilter[] _meshFilters;
        [SerializeField] protected MeshRenderer[] _renderers;

        protected CropSO _cropData = null;
        protected int _growTimer = 0;
        protected bool _isReadyToHarvest = false;

        protected void Awake()
        {
            _cropData = (CropSO)_data;
            _growTimer = _cropData.GrowthTime;
        }

        public void Grow()
        {
            if (_isReadyToHarvest) return;

            if (_growTimer > 0)
            {
                _growTimer--;
                UpdateModels();
            }

            if (_growTimer == 0)
            {
                _isReadyToHarvest = true;
            }
        }

        public void Harvest(InventorySO inventory)
        {
            if (!_isReadyToHarvest) return;
            _isReadyToHarvest = false;
            _growTimer = _cropData.GrowthTime;
            inventory.AddItem(_cropData.Food, _cropData.HarvestAmount);
            UpdateModels();
        }

        private void UpdateModels()
        {
            MeshFilter meshFilter = _cropData.Stages[_cropData.GrowthTime - _growTimer];
            Mesh mesh = meshFilter.sharedMesh;
            Material[] materials = meshFilter.GetComponent<MeshRenderer>().sharedMaterials;

            for (int i = _meshFilters.Length - 1; i >= 0; i--)
            {
                _meshFilters[i].mesh = mesh;
                _renderers[i].materials = materials;
            }
        }
    }
}
