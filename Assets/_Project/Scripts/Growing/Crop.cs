using UnityEngine;
using Project.Building;
using Project.Items;

namespace Project.Growing
{
    [AddComponentMenu("Project/Growing/Crop")]
    [DisallowMultipleComponent]
    public class Crop : Structure
    {
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
            _growTimer--;
            if (_growTimer > 0) return;
            _isReadyToHarvest = true;
        }

        public void Harvest(InventorySO inventory)
        {
            if (!_isReadyToHarvest) return;
            _isReadyToHarvest = false;
            _growTimer = _cropData.GrowthTime;
            inventory.AddItem(_cropData.Food, 1);
        }
    }
}
