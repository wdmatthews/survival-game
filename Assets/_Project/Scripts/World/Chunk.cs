using System.Collections.Generic;
using UnityEngine;
using Project.Combat;
using Project.Items;

namespace Project.World
{
    [AddComponentMenu("Project/World/Chunk")]
    [DisallowMultipleComponent]
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private RegionSO _region = null;
        [SerializeField] private float _minSpawnDistanceFromPlayer = 1;
        [SerializeField] private float _maxSpawnDistanceFromPlayer = 1;
        [SerializeField] private ResourcePercentage[] _resourcePercentages = { };

        [Space]
        [Header("TEMPORARY: REMOVE")]
        [SerializeField] private MonsterManagerSO _monsterManager = null;

        private ResourceNode[] _resourceNodes = { };
        private StructureNode[] _structureNodes = { };
        private MonsterNode[] _monsterNodes = { };
        private int _resourcePercentageCount = 0;
        private int _resourceNodeCount = 0;
        private int _structureNodeCount = 0;
        private int _monsterNodeCount = 0;
        private ResourcePercentage[] _currentResourcePercentages = { };
        private Dictionary<ResourceSO, ResourcePercentage> _currentResourcePercentagesByResourceType
            = new Dictionary<ResourceSO, ResourcePercentage>();
        private List<ResourceNode> _emptyResourceNodes = new List<ResourceNode>();

        private void Awake()
        {
            _resourceNodes = GetComponentsInChildren<ResourceNode>(true);
            _structureNodes = GetComponentsInChildren<StructureNode>(true);
            _monsterNodes = GetComponentsInChildren<MonsterNode>(true);

            _resourcePercentageCount = _resourcePercentages.Length;
            _currentResourcePercentages = new ResourcePercentage[_resourcePercentageCount];

            for (int i = 0; i < _resourcePercentageCount; i++)
            {
                ResourcePercentage resourcePercentage = new ResourcePercentage(_resourcePercentages[i]);
                _currentResourcePercentages[i] = resourcePercentage;
                _currentResourcePercentagesByResourceType.Add(resourcePercentage.Resource, resourcePercentage);
            }

            _resourceNodeCount = _resourceNodes.Length;
            _structureNodeCount = _structureNodes.Length;
            _monsterNodeCount = _monsterNodes.Length;

            // TODO REMOVE
            _monsterManager.CurrentChunk = this;
        }

        public void RegenerateResources()
        {
            CalculateResourcePercentages();

            for (int i = 0; i < _resourcePercentageCount; i++)
            {
                ResourcePercentage resourcePercentage = _currentResourcePercentages[i];
                int currentNumber = Mathf.FloorToInt(resourcePercentage.Percentage * _resourceNodeCount);
                int requiredNumber = Mathf.FloorToInt(_resourcePercentages[i].Percentage * _resourceNodeCount);

                if (currentNumber < requiredNumber)
                {
                    GenerateResources(resourcePercentage.Resource, requiredNumber - currentNumber);
                }
            }
        }

        private void CalculateResourcePercentages()
        {
            _emptyResourceNodes.Clear();

            for (int i = 0; i < _resourcePercentageCount; i++)
            {
                _currentResourcePercentages[i].Percentage = 0;
            }

            for (int i = 0; i < _resourceNodeCount; i++)
            {
                ResourceNode resourceNode = _resourceNodes[i];

                if (resourceNode.PlacedResource)
                {
                    _currentResourcePercentagesByResourceType[resourceNode.PlacedResource.Data].Percentage += 1;
                }
                else
                {
                    _emptyResourceNodes.Add(resourceNode);
                }
            }

            for (int i = 0; i < _resourcePercentageCount; i++)
            {
                _currentResourcePercentages[i].Percentage /= _resourceNodeCount;
            }
        }

        private void GenerateResources(ResourceSO resource, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int emptyNodeCount = _emptyResourceNodes.Count;
                if (emptyNodeCount == 0) return;
                int resourceNodeIndex = Random.Range(0, emptyNodeCount);
                ResourceNode resourceNode = _emptyResourceNodes[resourceNodeIndex];
                _emptyResourceNodes.RemoveAt(resourceNodeIndex);
                resourceNode.Place(resource, Random.Range(0, 4));
            }
        }

        public Monster SpawnMonster(MonsterSO monsterData, Vector3 playerPosition)
        {
            MonsterNode monsterNode = null;
            float distanceFromPlayer = 0;
            int spawnAttemptsLeft = _monsterNodeCount;

            while ((distanceFromPlayer < _minSpawnDistanceFromPlayer || distanceFromPlayer > _maxSpawnDistanceFromPlayer)
                && spawnAttemptsLeft >= 0)
            {
                monsterNode = _monsterNodes[Random.Range(0, _monsterNodeCount)];
                distanceFromPlayer = Vector2.Distance(
                    new Vector2(monsterNode.transform.position.x, monsterNode.transform.position.z),
                    new Vector2(playerPosition.x, playerPosition.z));
                spawnAttemptsLeft--;
            }

            if (!monsterNode || distanceFromPlayer < _minSpawnDistanceFromPlayer
                || distanceFromPlayer > _maxSpawnDistanceFromPlayer) return null;
            return monsterNode.Spawn(monsterData, Random.Range(0, 4));
        }
    }
}
