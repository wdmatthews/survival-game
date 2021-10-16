using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Project/Items/Resource")]
    public class ResourceSO : ScriptableObject
    {
        public Resource Prefab = null;
        public ResourceTypeSO Type = null;
        public float MaxHealth = 1;
        public int Amount = 1;
        public ToolTypeSO RequiredToolType = null;
        public int MinimumToolLevel = 1;

        public Resource Place(Vector3 position, int angleIndex)
        {
            Resource resource = Instantiate(Prefab);
            resource.Place(position, angleIndex);
            return resource;
        }
    }
}
