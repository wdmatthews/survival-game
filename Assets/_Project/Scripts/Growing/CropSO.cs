using UnityEngine;
using Project.Building;

namespace Project.Growing
{
    [CreateAssetMenu(fileName = "New Crop", menuName = "Project/Growing/Crop")]
    public class CropSO : StructureSO
    {
        public int GrowthTime = 1;
        public int HarvestAmount = 9;
        public FoodSO Food = null;
        public MeshFilter[] Stages = { };
        public float[] StagePositions = { };
    }
}
