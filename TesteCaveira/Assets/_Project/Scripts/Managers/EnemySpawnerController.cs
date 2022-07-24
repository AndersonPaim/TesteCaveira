using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enemy.Archer;
using Enemy.Melee;
using Managers;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int MaxArcherEnemies;
        public int MaxMeleeEnemies;
        public int SpawnDelay;
    }

    public Action OnFinishWaves;
    public Action<int> OnStartWave;

    [SerializeField] private GameManager _manager;
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private List<Wave> _spawnWaves = new List<Wave>();
    [SerializeField] private int _randomSpawnWaves;

    private List<GameObject> _currentArchers = new List<GameObject>();
    private List<GameObject> _currentMelees = new List<GameObject>();
    private int _currentArcherEnemies;
    private int _currentMeleeEnemies;
    private int _archersKills = 0;
    private int _meleeKills = 0;
    private int _currentWave = 0;
    private bool _isSpawning = false;
    private ObjectPooler _objectPooler;

    public void ClearEnemiesAlive()
    {
        foreach(GameObject enemy in _currentMelees)
        {
            enemy.GetComponent<MeleeAI>().TakeDamage(999);
        }

        foreach(GameObject enemy in _currentArchers)
        {
            enemy.GetComponent<ArcherAI>().TakeDamage(999);
        }
    }

    private void Start()
    {
        Initialize();
        SetupEvents();
        InitializeWaves();
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

    private void RandomizeWaves()
    {
        for(int i = 0; i < _randomSpawnWaves; i++)
        {
            float enemiesNumber = i + 1 * 2;

            int meleeEnemies = 0;
            int archerEnemies = 0;
            int randomSpawnDelay = 0;

            for(int j = 0; j < enemiesNumber; j++)
            {
                int randomEnemy = UnityEngine.Random.Range(0, 2);
                randomSpawnDelay = UnityEngine.Random.Range(1, 10);

                if(randomEnemy == 0)
                {
                    meleeEnemies++;
                }
                else
                {
                    archerEnemies++;
                }
            }

            Wave wave = new Wave();
            wave.MaxArcherEnemies = meleeEnemies;
            wave.MaxMeleeEnemies = archerEnemies;
            wave.SpawnDelay = randomSpawnDelay;

            _spawnWaves.Add(wave);
        }
    }

    private void CanFinishWave()
    {
        if(_spawnWaves[_currentWave].MaxArcherEnemies == _archersKills && _spawnWaves[_currentWave].MaxMeleeEnemies == _meleeKills)
        {
            _currentWave++;
            _archersKills = 0;
            _meleeKills = 0;
            _currentArcherEnemies = 0;
            _currentMeleeEnemies = 0;

            if(_currentWave >= _spawnWaves.Count)
            {
                OnFinishWaves?.Invoke();
            }
            else
            {
                OnStartWave?.Invoke(_currentWave + 1);
                SpawnEnemiesASync();
            }
        }
    }

    private async UniTask SpawnEnemiesASync()
    {
        await UniTask.Delay(_spawnWaves[_currentWave].SpawnDelay * 1000);

        _isSpawning = true;

        if(_currentArcherEnemies >= _spawnWaves[_currentWave].MaxArcherEnemies && _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnMelee();
        }
        else if(_currentMeleeEnemies >= _spawnWaves[_currentWave].MaxMeleeEnemies && _currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies)
        {
            await SpawnArcher();
        }
        else
        {
            int randomEnemy = UnityEngine.Random.Range(1, 3);

            if(randomEnemy == 1)
            {
                await SpawnArcher();
            }
            else
            {
                await SpawnMelee();
            }
        }
    }

    private async UniTask SpawnArcher()
    {
        _currentArcherEnemies++;
        GameObject enemy = _objectPooler.SpawnFromPool(ObjectsTag.ArcherEnemy);
        enemy.transform.position = GetSpawnPos().position;
        ArcherAI archer = enemy.GetComponent<ArcherAI>();
        archer.SetupEnemy(_manager);
        archer.OnEnemyDie += ArcherDeath;
        _currentArchers.Add(enemy);

        if(_currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies || _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnEnemiesASync();
        }
        else
        {
            _isSpawning = false;
        }
    }

    private async UniTask SpawnMelee()
    {
        _currentMeleeEnemies++;
        GameObject enemy = _objectPooler.SpawnFromPool(ObjectsTag.MeleeEnemy);
        enemy.transform.position = GetSpawnPos().position;
        MeleeAI melee = enemy.GetComponent<MeleeAI>();
        melee.SetupEnemy(_manager);
        melee.OnEnemyDie += MeleeDeath;
        _currentMelees.Add(enemy);

        if(_currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies || _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnEnemiesASync();
        }
        else
        {
            _isSpawning = false;
        }
    }

    private void ArcherDeath(GameObject enemy, int score)
    {
        _archersKills++;
        _currentArchers.Remove(enemy);
        enemy.GetComponent<ArcherAI>().OnEnemyDie -= ArcherDeath;
        _manager.ScoreManager.KillEnemyScore(score);
        CanFinishWave();
    }

    private void MeleeDeath(GameObject enemy, int score)
    {
        _meleeKills++;
        _currentMelees.Remove(enemy);
        enemy.GetComponent<MeleeAI>().OnEnemyDie -= MeleeDeath;
        _manager.ScoreManager.KillEnemyScore(score);
        CanFinishWave();
    }

    private Transform GetSpawnPos()
    {
        int randomPos = UnityEngine.Random.Range(0, _spawnPositions.Count);

        return _spawnPositions[randomPos];
    }
}