using UnityEngine;

namespace Project.World
{
    [CreateAssetMenu(fileName = "New Region", menuName = "Project/World/Region")]
    public class RegionSO : ScriptableObject
    {
        public int MaxMonstersPerSpawn = 1;
        public int MaxMonstersSpawned = 1;
    }
}
