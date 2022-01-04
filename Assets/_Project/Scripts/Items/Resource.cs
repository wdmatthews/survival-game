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
        protected bool _isBroken = false;

        public void Place(Vector3 position, int angleIndex)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, angleIndex * 90, 0);
            _health = _data.MaxHealth;
        }

        public void TakeDamage(ToolSO tool, InventorySO inventory)
        {
            if (_isBroken || tool.Type != _data.RequiredToolType
                || tool.Level < _data.MinimumToolLevel) return;
            _health = Mathf.Clamp(_health - tool.Damage, 0, _data.MaxHealth);
            if (Mathf.Approximately(_health, 0)) Break(inventory);
        }

        protected void Break(InventorySO inventory)
        {
            _isBroken = true;
            inventory.AddResource(_data.Type, _data.Amount);
            Destroy(gameObject);
        }
    }
}
