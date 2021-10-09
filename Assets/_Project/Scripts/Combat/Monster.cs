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
    }
}
