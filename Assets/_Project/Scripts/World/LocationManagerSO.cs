using System.Collections.Generic;
using UnityEngine;

namespace Project.World
{
    [CreateAssetMenu(fileName = "Location Manager", menuName = "Project/World/Location Manager")]
    public class LocationManagerSO : ScriptableObject
    {
        [System.NonSerialized] public RegionSO Region = null;
        [System.NonSerialized] public Chunk CurrentChunk = null;
        [System.NonSerialized] public List<Chunk> NearbyChunks = new();
        [System.NonSerialized] public List<Chunk> Chunks = new();

        public void OnNextDay()
        {
            foreach (var chunk in Chunks)
            {
                chunk.RegenerateResources();
                chunk.GrowCrops();
            }
        }
    }
}
