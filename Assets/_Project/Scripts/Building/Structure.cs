using UnityEngine;

namespace Project.Building
{
    [AddComponentMenu("Project/Building/Structure")]
    [DisallowMultipleComponent]
    public class Structure : MonoBehaviour
    {
        [SerializeField] protected StructureSO _data = null;

        protected float _health = 0;
        protected bool _isBroken = false;

        private void Awake()
        {
            _health = _data.MaxHealth;
        }

        public void Place(Vector3 position, int angleIndex)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, angleIndex * 90, 0);
            _health = _data.MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (_isBroken) return;
            _health = Mathf.Clamp(_health - amount, 0, _data.MaxHealth);
            if (Mathf.Approximately(_health, 0)) Break();
        }

        public void Break()
        {
            _isBroken = true;
            Destroy(gameObject);
        }
    }
}
