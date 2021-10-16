using UnityEngine;

namespace Project.Combat
{
    [CreateAssetMenu(fileName = "New Monster", menuName = "Project/Combat/Monster")]
    public class MonsterSO : DamageableSO
    {
        public Monster Prefab = null;

        public Monster Spawn(Vector3 position, int angleIndex)
        {
            Monster monster = Instantiate(Prefab);
            monster.Spawn(position, angleIndex);
            return monster;
        }
    }
}
