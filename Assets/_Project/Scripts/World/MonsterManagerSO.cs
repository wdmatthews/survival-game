using System.Collections.Generic;
using UnityEngine;
using Project.Combat;
using Project.Time;

namespace Project.World
{
    [CreateAssetMenu(fileName = "Monster Manager", menuName = "Project/World/Monster Manager")]
    public class MonsterManagerSO : ScriptableObject
    {
        public float SpawnCooldown = 1;
        public MonsterSO[] Monsters = { };
        public TimeManagerSO TimeManager = null;
        public DamageableReferenceSO PlayerReference = null;

        [System.NonSerialized] public RegionSO Region = null;
        [System.NonSerialized] public Chunk CurrentChunk = null;
        [System.NonSerialized] public List<Chunk> NearbyChunks = new List<Chunk>();
        [System.NonSerialized] private float _spawnTimer = 0;
        [System.NonSerialized] private List<Monster> _aliveMonsters = new List<Monster>();

        public void OnUpdate()
        {
            if (TimeManager.IsDay || _aliveMonsters.Count == Region.MaxMonstersSpawned) return;
            if (Mathf.Approximately(_spawnTimer, 0)) SpawnMonsters();
            else _spawnTimer = Mathf.Clamp(_spawnTimer - UnityEngine.Time.deltaTime,
                0, SpawnCooldown);
        }

        public int CalculateMonsterSpawnCount(int maxMonsterCount)
        {
            float nightProgress = TimeManager.MinutesLeftBeforeNextDay / TimeManager.MinutesPerNight;
            return Mathf.RoundToInt(maxMonsterCount - (maxMonsterCount - 1) * 2 * Mathf.Abs(0.5f - nightProgress));
        }

        public void SpawnMonsters()
        {
            _spawnTimer = SpawnCooldown;

            int monsterSpawnCount = CalculateMonsterSpawnCount(Region.MaxMonstersPerSpawn);

            for (int i = 0; i < monsterSpawnCount; i++)
            {
                if (_aliveMonsters.Count == Region.MaxMonstersSpawned) break;
                SpawnMonster();
            }
        }

        public void SpawnMonster()
        {
            MonsterSO monsterData = Monsters[Random.Range(0, Monsters.Length)];
            Vector3 playerPosition = PlayerReference.Value.transform.position;
            Monster monsterSpawned = CurrentChunk.SpawnMonster(monsterData, playerPosition, DespawnMonster);
            int nearbyChunkIndex = 0;
            int nearbyChunkCount = NearbyChunks.Count;

            while (!monsterSpawned && nearbyChunkIndex < nearbyChunkCount)
            {
                monsterSpawned = NearbyChunks[nearbyChunkIndex].SpawnMonster(monsterData, playerPosition, DespawnMonster);
                nearbyChunkIndex++;
            }

            if (monsterSpawned) _aliveMonsters.Add(monsterSpawned);
        }

        public void DespawnMonster(Monster monster)
        {
            _aliveMonsters.Remove(monster);
        }
    }
}
