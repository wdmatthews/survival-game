using UnityEngine;

namespace Project.Combat
{
    public abstract class DamageableSO : ScriptableObject
    {
        [Header("Health Settings")]
        public float MaxHealth = 1;
        public float TimeBeforeHealthRegeneration = 0;
        public float HealthRegenerationAmount = 0;
        public float HealthRegenerationCooldown = 0;
    }
}
