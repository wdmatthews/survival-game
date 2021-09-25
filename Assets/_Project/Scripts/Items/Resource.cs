using UnityEngine;

namespace Project.Items
{
    [AddComponentMenu("Project/Items/Resource")]
    [DisallowMultipleComponent]
    public class Resource : MonoBehaviour
    {
        [SerializeField] protected ResourceSO _data = null;
        
        public ResourceSO Data => _data;

        protected float _health = 0;

        protected void Awake()
        {
            _health = _data.MaxHealth;
        }
    }
}
