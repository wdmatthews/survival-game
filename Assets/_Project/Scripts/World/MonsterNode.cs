using UnityEngine;
using Project.Combat;

namespace Project.World
{
    [AddComponentMenu("Project/World/Monster Node")]
    [DisallowMultipleComponent]
    public class MonsterNode : MonoBehaviour
    {
        private Monster _spawnedMonster = null;

        public Monster Spawn(MonsterSO monster, int angleIndex)
        {
            _spawnedMonster = monster.Spawn(transform.position, angleIndex);
            return _spawnedMonster;
        }
    }
}
