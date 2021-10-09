using UnityEngine;

namespace Project.Combat
{
    public abstract class DamageableSO : ScriptableObject
    {
        [Header("Health Settings")]
        public float MaxHealth = 1;
        public float TimeBeforeHealthRegeneration = 1;
        public float HealthRegenerationAmount = 1;
        public float HealthRegenerationCooldown = 1;
    }
}
