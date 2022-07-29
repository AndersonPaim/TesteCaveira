using System;
using System.Collections.Generic;
using Coimbra.Services.Events;
using Cysharp.Threading.Tasks;
using Enemy;
using Event;
using UnityEngine;

namespace Managers.Spawner
{
    public class EnemySpawnerController : MonoBehaviour
    {
        public Action OnFinishWaves;
        public Action<int> OnStartWave;

        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private WaypointController _waypointsController;
        [SerializeField] private GameObject _player;
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

        private void Start()
        {
            SetupEvents();
            InitializeWaves();
            SetupWave();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void SetupEvents()
        {
            OnGameStarted.AddListener(StartSpawning);
            OnClearEnemies.AddListener(ClearEnemiesAlive);
        }

        private void DestroyEvents()
        {
            OnGameStarted.RemoveAllListeners();
            OnClearEnemies.RemoveAllListeners();
        }

        private void InitializeWaves()
        {
            if(_spawnWaves.Count == 0)
            {
                RandomizeWaves();
            }
        }

        private void ClearEnemiesAlive(ref EventContext context, in OnClearEnemies e)
        {
            foreach(GameObject enemy in _currentEnemiesObj)
            {
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
                enemyBase.KillEnemy();
            }
        }

        private void StartSpawning(ref EventContext context, in OnGameStarted e)
        {
            OnStartWave?.Invoke(_currentWave + 1);
            SpawnEnemiesASync();
        }

        private void SetupWave()
        {
            _kills = 0;
            _currentEnemies = 0;
            _totalWaveEnemies = 0;

            foreach(EnemySpawner enemy in _spawnWaves[_currentWave].Enemies)
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

                List<EnemySpawner> enemyList = new List<EnemySpawner>();

                for(int j = 0; j < _enemiesPrefab.Count; j++)
                {
                    EnemySpawner enemy = new EnemySpawner();
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
            GameObject enemyObj = ObjectPooler.sInstance.SpawnFromPool(obj.GetInstanceID());
            enemyObj.transform.position = GetSpawnPos().position;
            EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
            enemy.SetupEnemy(_waypointsController, _player);
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
            _scoreManager.KillEnemyScore(score);
            CanFinishWave();
        }

        private Transform GetSpawnPos()
        {
            int randomPos = UnityEngine.Random.Range(0, _spawnPositions.Count);

            return _spawnPositions[randomPos];
        }
    }
}