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

    private void Start()
    {
        Initialize();
        SpawnEnemiesASync();
    }

    private void Initialize()
    {
        _objectPooler = _manager.ObjectPooler;
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
                //TODO WIN
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
            int randomEnemy = Random.Range(1, 3);

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
        int randomPos = Random.Range(0, _spawnPositions.Count);

        return _spawnPositions[randomPos];
    }
}