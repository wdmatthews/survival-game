using UnityEngine;
using Project.Items;

namespace Project.World
{
    [System.Serializable]
    public class ResourcePercentage
    {
        public ResourceSO Resource = null;
        [Range(0, 1)] public float Percentage = 0.5f;

        public ResourcePercentage() { }

        public ResourcePercentage(ResourceSO resource, float percentage)
        {
            Resource = resource;
            Percentage = percentage;
        }

        public ResourcePercentage(ResourcePercentage resourcePercentage)
        {
            Resource = resourcePercentage.Resource;
            Percentage = resourcePercentage.Percentage;
        }
    }
}
