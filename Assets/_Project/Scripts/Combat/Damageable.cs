using UnityEngine;

namespace Project.Combat
{
    public abstract class Damageable : MonoBehaviour
    {
        [SerializeField] protected DamageableSO _data = null;

        protected float _health = 0;
        protected bool _isDead = false;
        protected float _timeLeftUntilHealthRegeneration = 0;
        protected float _healthRegenerationCooldownTimer = 0;

        protected virtual void Awake()
        {
            _health = _data.MaxHealth;
        }

        protected virtual void Update()
        {
            if (Mathf.Approximately(_health, _data.MaxHealth)
                || Mathf.Approximately(_data.TimeBeforeHealthRegeneration, 0)) return;

            if (Mathf.Approximately(_timeLeftUntilHealthRegeneration, 0))
            {
                if (Mathf.Approximately(_healthRegenerationCooldownTimer, 0))
                {
                    Heal(_data.HealthRegenerationAmount);
                    _healthRegenerationCooldownTimer = _data.HealthRegenerationCooldown;
                }
                else
                {
                    _healthRegenerationCooldownTimer = Mathf.Clamp(
                        _healthRegenerationCooldownTimer - Time.deltaTime,
                        0, _data.HealthRegenerationCooldown);
                }
            }
            else
            {
                _timeLeftUntilHealthRegeneration = Mathf.Clamp(
                    _timeLeftUntilHealthRegeneration - Time.deltaTime,
                    0, _data.TimeBeforeHealthRegeneration);
            }
        }

        public virtual void TakeDamage(float amount)
        {
            if (_isDead) return;
            _health = Mathf.Clamp(_health - amount, 0, _data.MaxHealth);
            _timeLeftUntilHealthRegeneration = _data.TimeBeforeHealthRegeneration;
            _healthRegenerationCooldownTimer = 0;
            if (Mathf.Approximately(_health, 0)) Die();
        }

        public virtual void Heal(float amount)
        {
            _health = Mathf.Clamp(_health + amount, 0, _data.MaxHealth);
        }

        public virtual void Die()
        {
            _isDead = true;
        }
    }
}
