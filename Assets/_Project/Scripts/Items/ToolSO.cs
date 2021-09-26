using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Project/Items/Tool")]
    public class ToolSO : ItemSO
    {
        public ToolTypeSO Type = null;
        public float Damage = 1;
        public int Level = 1;

        public override void Use(Resource resource, InventorySO inventory)
        {
            if (!resource) return;
            resource.TakeDamage(this, inventory);
        }
    }
}
