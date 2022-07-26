using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;

namespace Managers.Spawner
{
    public class EnemySpawnerController : MonoBehaviour
    {
        public Action OnFinishWaves;
        public Action<int> OnStartWave;

        [SerializeField] private GameManager _manager;
        [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
        [Header("CUSTOM SPAWN DATA")]
        [SerializeField] private List<Wave> _spawnWaves = new List<Wave>();
        [Header("RANDOM SPAWN DATA")]
        [SerializeField] private List<GameObject> _enemiesPrefab;
        [SerializeField] private int _randomWaveNumber;
        [SerializeField] private int _maxRandomSpawnDelay;
        [SerializeField] private int _minRandomSpawnDelay;

        private List<GameObject> _currentEnemiesObj = new List<GameObject>();
        private int _currentEnemies;
        private int _totalWaveEnemies;
        private int _kills = 0;
        private int _currentWave = 0;
        private bool _isSpawning = false;
        private ObjectPooler _objectPooler;

        public void ClearEnemiesAlive()
        {
            foreach(GameObject enemy in _currentEnemiesObj)
            {
                enemy.GetComponent<EnemyBase>().TakeDamage(999);
            }
        }

        private void Start()
        {
            Initialize();
            SetupEvents();
            InitializeWaves();
            SetupWave();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void Initialize()
        {
            _objectPooler = _manager.ObjectPooler;
        }

        private void SetupEvents()
        {
            _manager.OnGameStarted += StartSpawning;
        }

        private void DestroyEvents()
        {
            _manager.OnGameStarted -= StartSpawning;
        }

        private void StartSpawning()
        {
            OnStartWave?.Invoke(_currentWave + 1);
            SpawnEnemiesASync();
        }

        private void InitializeWaves()
        {
            if(_spawnWaves.Count == 0)
            {
                RandomizeWaves();
            }
        }

        private void SetupWave()
        {
            _kills = 0;
            _currentEnemies = 0;
            _totalWaveEnemies = 0;

            foreach(EnemySpawn enemy in _spawnWaves[_currentWave].Enemies)
            {
                _totalWaveEnemies += enemy.EnemyNumber;
            }
        }

        private void RandomizeWaves()
        {
            InitializeWaveLists();

            int enemiesSpawned = 0;

            for(int i = 0; i < _randomWaveNumber; i++)
            {
                float enemiesNumber = i + 1 * 2;

                for(int j = 0; j < enemiesNumber; j++)
                {
                    int randomEnemy = UnityEngine.Random.Range(0, _enemiesPrefab.Count);
                    enemiesSpawned++;
                    _spawnWaves[i].Enemies[randomEnemy].EnemyNumber++;
                }

                _spawnWaves[i].SpawnDelay = UnityEngine.Random.Range(_minRandomSpawnDelay, _maxRandomSpawnDelay + 1);
            }
        }

        private void InitializeWaveLists()
        {
            for(int i = 0; i < _randomWaveNumber; i++)
            {
                Wave wave = new Wave();

                List<EnemySpawn> enemyList = new List<EnemySpawn>();

                for(int j = 0; j < _enemiesPrefab.Count; j++)
                {
                    EnemySpawn enemy = new EnemySpawn();
                    enemy.EnemyNumber = 0;
                    enemy.EnemyPrefab = _enemiesPrefab[j];
                    enemyList.Add(enemy);
                }

                wave.Enemies = enemyList;
                _spawnWaves.Add(wave);
            }
        }

        private void CanFinishWave()
        {
            if(_totalWaveEnemies == _kills)
            {
                _currentWave++;

                if(_currentWave >= _spawnWaves.Count)
                {
                    OnFinishWaves?.Invoke();
                }
                else
                {
                    SetupWave();
                    OnStartWave?.Invoke(_currentWave + 1);
                    SpawnEnemiesASync();
                }
            }
        }

        private async UniTask SpawnEnemiesASync()
        {
            await UniTask.Delay(_spawnWaves[_currentWave].SpawnDelay * 1000);

            _isSpawning = true;

            int randomEnemy = UnityEngine.Random.Range(0, _spawnWaves[_currentWave].Enemies.Count);

            if(_spawnWaves[_currentWave].Enemies[randomEnemy].EnemyNumber != 0)
            {
                _spawnWaves[_currentWave].Enemies[randomEnemy].EnemyNumber--;
                await SpawnEnemy(_spawnWaves[_currentWave].Enemies[randomEnemy].EnemyPrefab);
            }
            else
            {
                await SpawnEnemiesASync();
            }
        }

        private async UniTask SpawnEnemy(GameObject obj)
        {
            _currentEnemies++;
            GameObject enemyObj = _objectPooler.SpawnFromPool(obj.GetInstanceID());
            enemyObj.transform.position = GetSpawnPos().position;
            EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
            enemy.SetupEnemy(_manager);
            enemy.OnEnemyDie += EnemyDeath;
            _currentEnemiesObj.Add(enemyObj);

            if(_currentEnemies == _totalWaveEnemies)
            {
                _isSpawning = false;
            }
            else
            {
                await SpawnEnemiesASync();
            }
        }

        private void EnemyDeath(GameObject enemy, int score)
        {
            _kills++;
            _currentEnemiesObj.Remove(enemy);
            enemy.GetComponent<EnemyBase>().OnEnemyDie -= EnemyDeath;
            _manager.ScoreManager.KillEnemyScore(score);
            CanFinishWave();
        }

        private Transform GetSpawnPos()
        {
            int randomPos = UnityEngine.Random.Range(0, _spawnPositions.Count);

            return _spawnPositions[randomPos];
        }
    }
}