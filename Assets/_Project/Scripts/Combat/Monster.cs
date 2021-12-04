using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Project.Building;
using Project.Utilities;

namespace Project.Combat
{
    [AddComponentMenu("Project/Combat/Monster")]
    [DisallowMultipleComponent]
    public class Monster : Damageable
    {
        [SerializeField] protected NavMeshAgent _navigationAgent = null;

        protected MonsterSO _monsterData = null;
        protected Transform _playerTransform = null;
        protected Transform _moveTarget = null;
        protected List<MonoBehaviour> _nearbyCharacters = new List<MonoBehaviour>();
        protected List<Structure> _nearbyFences = new List<Structure>();
        protected bool _attackIsCoolingDown = false;
        protected float _attackCooldownTimer = 0;
        protected System.Action<Monster> _onDie = null;
        protected bool _isTargetingFence = false;

        protected override void Awake()
        {
            base.Awake();
            _monsterData = (MonsterSO)_data;
            _attackCooldownTimer = _monsterData.AttackCooldown;
        }

        protected void Start()
        {
            _playerTransform = _monsterData.ReferenceToPlayer.Value.transform;
            _moveTarget = _playerTransform;
        }

        protected override void Update()
        {
            base.Update();

            int playerCount = _nearbyCharacters.Count;
            int fenceCount = _nearbyFences.Count;

            if (_isTargetingFence && !_moveTarget)
            {
                _isTargetingFence = false;
                _moveTarget = _playerTransform;
            }

            if (!_isTargetingFence && _navigationAgent.remainingDistance > _monsterData.DistanceToTargetFences)
            {
                RaycastHit hit;
                Vector3 direction = (_playerTransform.position - transform.position).normalized;
                direction.y = 0;
                Physics.Raycast(transform.position + new Vector3(0, _monsterData.FenceLineDetectionOffset, 0),
                    direction, out hit,
                    _monsterData.FenceLineDetectionDistance, _monsterData.FenceLayers);
                
                if (hit.collider)
                {
                    _isTargetingFence = true;
                    _moveTarget = hit.collider.transform;
                }
            }

            transform.LookAt(_moveTarget);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            float distanceToTarget = Vector3.Distance(transform.position, _moveTarget.position);

            if (distanceToTarget > _monsterData.StopDistanceFromTarget)
            {
                _navigationAgent.SetDestination(_moveTarget.position);
            }
            else
            {
                _navigationAgent.velocity = new Vector3();
            }

            if (_attackIsCoolingDown)
            {
                if (Mathf.Approximately(_attackCooldownTimer, 0)) _attackIsCoolingDown = false;
                else _attackCooldownTimer = Mathf.Clamp(_attackCooldownTimer - Time.deltaTime,
                    0, _monsterData.AttackCooldown);
            }
            else if (playerCount > 0 || fenceCount > 0) StartAttack();
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
            else if (_monsterData.FenceLayers.Contains(colliderLayer))
            {
                Structure fence = other.GetComponent<Structure>();
                if (!_nearbyFences.Contains(fence)) _nearbyFences.Add(fence);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            int colliderLayer = other.gameObject.layer;

            if (_monsterData.CharacterLayers.Contains(colliderLayer))
            {
                _nearbyCharacters.Remove(other.GetComponent<Damageable>());
            }
            else if (_monsterData.FenceLayers.Contains(colliderLayer))
            {
                _nearbyFences.Remove(other.GetComponent<Structure>());
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
            DamageFences();
            DamageCharacters();
        }

        public void DamageFences()
        {
            for (int i = _nearbyFences.Count - 1; i >= 0; i--)
            {
                Structure fence = _nearbyFences[i];
                fence.TakeDamage(_monsterData.Damage);
            }
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
