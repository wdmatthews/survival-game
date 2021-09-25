using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Project/Items/Resource")]
    public class ResourceSO : ScriptableObject
    {
        public ResourceTypeSO Type = null;
        public float MaxHealth = 1;
    }
}
