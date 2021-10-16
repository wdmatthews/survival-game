using UnityEngine;

namespace Project.Combat
{
    [AddComponentMenu("Project/Combat/Monster")]
    [DisallowMultipleComponent]
    public class Monster : Damageable
    {
        protected MonsterSO _monsterData = null;

        protected override void Awake()
        {
            base.Awake();
            _monsterData = (MonsterSO)_data;
        }

        public void Spawn(Vector3 position, int angleIndex)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, angleIndex * 90, 0);
            _health = _data.MaxHealth;
        }
    }
}
