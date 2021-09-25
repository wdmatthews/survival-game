using UnityEngine;

namespace Project.Items
{
    [AddComponentMenu("Project/Items/Item")]
    [DisallowMultipleComponent]
    public class Item : MonoBehaviour
    {
        [SerializeField] protected ItemSO _data = null;

        public ItemSO Data => _data;
    }
}
