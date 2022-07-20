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

    [SerializeField] private GameManager _manager;
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private List<Wave> _spawnWaves = new List<Wave>();

    private List<GameObject> _currentArchers = new List<GameObject>();
    private List<GameObject> _currentMelees = new List<GameObject>();
    private int _currentArcherEnemies;
    private int _currentMeleeEnemies;
    private int _currentWave = 0;
    private ObjectPooler _objectPooler;

    public void ClearEnemiesAlive()
    {
        foreach(GameObject enemy in _currentMelees)
        {
            enemy.GetComponent<MeleeAI>().CurrentState.Death();
        }

        foreach(GameObject enemy in _currentArchers)
        {
            enemy.GetComponent<ArcherAI>().CurrentState.Death();
        }
    }

    private async UniTask Start()
    {
        Initialize();
        InitializeWaves();
        await SpawnEnemiesASync();
    }

    private void Initialize()
    {
        _objectPooler = _manager.ObjectPooler;
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
        for(int i = 0; i < 5; i++)
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

    private void ArcherDeath(GameObject enemy)
    {
        _currentArcherEnemies--;
        _currentArchers.Remove(enemy);
        enemy.GetComponent<ArcherAI>().OnEnemyDie -= MeleeDeath;
        CanFinishWave();
    }

    private void MeleeDeath(GameObject enemy)
    {
        _currentMeleeEnemies--;
        _currentMelees.Remove(enemy);
        enemy.GetComponent<MeleeAI>().OnEnemyDie -= MeleeDeath;
    }

    private void CanFinishWave()
    {
        if(_currentArcherEnemies == 0 && _currentMeleeEnemies == 0)
        {
            _currentWave++;

            if(_currentWave >= _spawnWaves.Count)
            {
                OnFinishWaves?.Invoke();
            }
            else
            {
                SpawnEnemiesASync();
            }
        }
    }

    private async UniTask SpawnEnemiesASync()
    {
        await UniTask.Delay(_spawnWaves[_currentWave].SpawnDelay * 1000);

        if(_currentArcherEnemies >= _spawnWaves[_currentWave].MaxArcherEnemies && _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnMelee(GetSpawnPos());
        }
        else if(_currentMeleeEnemies >= _spawnWaves[_currentWave].MaxMeleeEnemies && _currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies)
        {
            await SpawnArcher(GetSpawnPos());
        }
        else
        {
            int randomEnemy = UnityEngine.Random.Range(1, 3);

            if(randomEnemy == 1)
            {
                await SpawnArcher(GetSpawnPos());
            }
            else
            {
                await SpawnMelee(GetSpawnPos());
            }
        }
    }

    private async UniTask SpawnArcher(Transform pos)
    {
        _currentArcherEnemies++;
        GameObject enemy = _objectPooler.SpawnFromPool(ObjectsTag.ArcherEnemy);
        enemy.transform.position = pos.position;
        ArcherAI archer = enemy.GetComponent<ArcherAI>();
        archer.SetupEnemy(_manager);
        archer.OnEnemyDie += ArcherDeath;
        _currentArchers.Add(enemy);

        if(_currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies || _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnEnemiesASync();
        }
    }

    private async UniTask SpawnMelee(Transform pos)
    {
        _currentMeleeEnemies++;
        GameObject enemy = _objectPooler.SpawnFromPool(ObjectsTag.MeleeEnemy);
        enemy.transform.position = pos.position;
        MeleeAI melee = enemy.GetComponent<MeleeAI>();
        melee.SetupEnemy(_manager);
        melee.OnEnemyDie += MeleeDeath;
        _currentMelees.Add(enemy);

        if(_currentArcherEnemies < _spawnWaves[_currentWave].MaxArcherEnemies || _currentMeleeEnemies < _spawnWaves[_currentWave].MaxMeleeEnemies)
        {
            await SpawnEnemiesASync();
        }
    }

    private Transform GetSpawnPos()
    {
        int randomPos = UnityEngine.Random.Range(0, _spawnPositions.Count);

        return _spawnPositions[randomPos];
    }
}