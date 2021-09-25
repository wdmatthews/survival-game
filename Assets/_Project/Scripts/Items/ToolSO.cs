using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Project/Items/Tool")]
    public class ToolSO : ItemSO
    {
        public override void Use(Resource resource)
        {
            if (!resource) return;
            Debug.Log("Break " + resource, resource);
        }
    }
}
