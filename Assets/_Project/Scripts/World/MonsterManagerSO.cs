using System.Collections.Generic;
using UnityEngine;
using Project.Combat;

namespace Project.World
{
    [CreateAssetMenu(fileName = "Monster Manager", menuName = "Project/World/Monster Manager")]
    public class MonsterManagerSO : ScriptableObject
    {
        public float SpawnCooldown = 1;
        public MonsterSO[] Monsters = { };
        public TimeManagerSO TimeManager = null;
        public DamageableReferenceSO PlayerReference = null;
        public LocationManagerSO LocationManager = null;

        [System.NonSerialized] private float _spawnTimer = 0;
        [System.NonSerialized] private List<Monster> _aliveMonsters = new List<Monster>();

        public void OnUpdate()
        {
            if (TimeManager.IsDay || _aliveMonsters.Count == LocationManager.Region.MaxMonstersSpawned) return;
            if (Mathf.Approximately(_spawnTimer, 0)) SpawnMonsters();
            else _spawnTimer = Mathf.Clamp(_spawnTimer - Time.deltaTime,
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

            int monsterSpawnCount = CalculateMonsterSpawnCount(LocationManager.Region.MaxMonstersPerSpawn);

            for (int i = 0; i < monsterSpawnCount; i++)
            {
                if (_aliveMonsters.Count == LocationManager.Region.MaxMonstersSpawned) break;
                SpawnMonster();
            }
        }

        public void SpawnMonster()
        {
            MonsterSO monsterData = Monsters[Random.Range(0, Monsters.Length)];
            Vector3 playerPosition = PlayerReference.Value.transform.position;
            Monster monsterSpawned = LocationManager.CurrentChunk.SpawnMonster(monsterData, playerPosition, DespawnMonster);
            int nearbyChunkIndex = 0;
            int nearbyChunkCount = LocationManager.NearbyChunks.Count;

            while (!monsterSpawned && nearbyChunkIndex < nearbyChunkCount)
            {
                monsterSpawned = LocationManager.NearbyChunks[nearbyChunkIndex].SpawnMonster(monsterData, playerPosition, DespawnMonster);
                nearbyChunkIndex++;
            }

            if (monsterSpawned) _aliveMonsters.Add(monsterSpawned);
        }

        public void DespawnMonster(Monster monster)
        {
            _aliveMonsters.Remove(monster);
        }

        public void KillAllMonsters()
        {
            for (int i = _aliveMonsters.Count - 1; i >= 0; i--)
            {
                Destroy(_aliveMonsters[i].gameObject);
            }

            _aliveMonsters.Clear();
        }
    }
}
