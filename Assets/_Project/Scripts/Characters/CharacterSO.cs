using UnityEngine;
using Project.Combat;

namespace Project.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Project/Characters/Character")]
    public class CharacterSO : DamageableSO
    {
        [Space]
        [Header("Physics Settings")]
        public float MoveSpeed = 1;
        public float JumpSpeed = 1;
        public float GravityScale = 1;
        public float MinYVelocity = -1;
        public float GroundCheckDistance = 0.1f;
        public LayerMask GroundLayers = 0;
        public LayerMask ResourceLayers = 0;
        public LayerMask MonsterLayers = 0;
        public LayerMask StructureNodeLayers = 0;
        public LayerMask CropLayers = 0;
    }
}
