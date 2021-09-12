using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Project/Items/Tool")]
    public class ToolSO : ItemSO
    {
        public override void Use()
        {
            Debug.Log($"{name} used!", this);
        }
    }
}
