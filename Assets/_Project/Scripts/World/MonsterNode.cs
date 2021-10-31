using UnityEngine;
using Project.Combat;

namespace Project.World
{
    [AddComponentMenu("Project/World/Monster Node")]
    [DisallowMultipleComponent]
    public class MonsterNode : MonoBehaviour
    {
        private Monster _spawnedMonster = null;

        public Monster Spawn(MonsterSO monster, int angleIndex, System.Action<Monster> onDie)
        {
            _spawnedMonster = monster.Spawn(transform.position, angleIndex, onDie);
            return _spawnedMonster;
        }
    }
}
