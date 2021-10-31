using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Project.Utilities;

namespace Project.Combat
{
    [AddComponentMenu("Project/Combat/Monster")]
    [DisallowMultipleComponent]
    public class Monster : Damageable
    {
        [SerializeField] protected NavMeshAgent _navigationAgent = null;

        protected MonsterSO _monsterData = null;
        protected Transform _moveTarget = null;
        protected List<MonoBehaviour> _nearbyCharacters = new List<MonoBehaviour>();
        protected bool _attackIsCoolingDown = false;
        protected float _attackCooldownTimer = 0;
        protected System.Action<Monster> _onDie = null;

        protected override void Awake()
        {
            base.Awake();
            _monsterData = (MonsterSO)_data;
            _attackCooldownTimer = _monsterData.AttackCooldown;
        }

        protected void Start()
        {
            _moveTarget = _monsterData.ReferenceToPlayer.Value.transform;
        }

        protected override void Update()
        {
            base.Update();

            float distanceToTarget = Vector3.Distance(transform.position, _moveTarget.position);

            if (distanceToTarget > _monsterData.StopDistanceFromTarget)
            {
                _navigationAgent.SetDestination(_moveTarget.position);
            }
            else
            {
                _navigationAgent.velocity = new Vector3();
            }

            int playerCount = _nearbyCharacters.Count;

            if (_attackIsCoolingDown)
            {
                if (Mathf.Approximately(_attackCooldownTimer, 0)) _attackIsCoolingDown = false;
                else _attackCooldownTimer = Mathf.Clamp(_attackCooldownTimer - Time.deltaTime,
                    0, _monsterData.AttackCooldown);
            }
            else if (playerCount > 0) StartAttack();
        }

        protected void OnTriggerEnter(Collider other)
        {
            int colliderLayer = other.gameObject.layer;

            if (_monsterData.CharacterLayers.Contains(colliderLayer)
                && !(other is SphereCollider))
            {
                Damageable character = other.GetComponent<Damageable>();
                if (!_nearbyCharacters.Contains(character)) _nearbyCharacters.Add(character);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            int colliderLayer = other.gameObject.layer;

            if (_monsterData.CharacterLayers.Contains(colliderLayer))
            {
                _nearbyCharacters.Remove(other.GetComponent<Damageable>());
            }
        }

        public void Spawn(Vector3 position, int angleIndex, System.Action<Monster> onDie)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, angleIndex * 90, 0);
            _health = _data.MaxHealth;
            _onDie = onDie;
        }

        public void StartAttack()
        {
            DamageCharacters();
        }

        public void DamageCharacters()
        {
            for (int i = _nearbyCharacters.Count - 1; i >= 0; i--)
            {
                Damageable character = (Damageable)_nearbyCharacters[i];
                character.TakeDamage(_monsterData.Damage);
            }

            FinishAttack();
        }

        public void FinishAttack()
        {
            _attackIsCoolingDown = true;
            _attackCooldownTimer = _monsterData.AttackCooldown;
        }

        public override void Die()
        {
            base.Die();
            _onDie?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
