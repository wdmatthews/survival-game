using UnityEngine;
using Project.Items;

namespace Project.World
{
    [AddComponentMenu("Project/World/Resource Node")]
    [DisallowMultipleComponent]
    public class ResourceNode : MonoBehaviour
    {
        private Resource _placedResource = null;

        public Resource PlacedResource => _placedResource;

        public void Place(ResourceSO resource, int angleIndex)
        {
            _placedResource = resource.Place(transform.position, angleIndex);
        }
    }
}
