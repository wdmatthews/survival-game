using UnityEngine;

namespace Project.Combat
{
    [CreateAssetMenu(fileName = "New Monster", menuName = "Project/Combat/Monster")]
    public class MonsterSO : DamageableSO
    {
        [Space]
        [Header("Monster Data")]
        public float Damage = 1;
        public float AttackCooldown = 1;
        public float StopDistanceFromTarget = 1;
        public float DistanceToTargetFences = 1;
        public float FenceLineDetectionDistance = 1;
        public float FenceLineDetectionOffset = -0.5f;
        public LayerMask CharacterLayers = 0;
        public LayerMask FenceLayers = 0;

        [Space]
        [Header("Monster Object References")]
        public Monster Prefab = null;
        public DamageableReferenceSO ReferenceToPlayer = null;

        public Monster Spawn(Vector3 position, int angleIndex, System.Action<Monster> die)
        {
            Monster monster = Instantiate(Prefab);
            monster.Spawn(position, angleIndex, die);
            return monster;
        }
    }
}
