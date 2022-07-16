using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enemy.Archer;
using Enemy.Melee;
using Managers;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private int _maxArcherEnemies;
    [SerializeField] private int _maxMeleeEnemies;
    [SerializeField] private int _spawnDelay;

    //private Dictionary<int, GameObject> _currentEnemies = new Dictionary<int, GameObject>();
     private List<GameObject> _currentEnemies = new List<GameObject>();
    private int _currentArcherEnemies;
    private int _currentMeleeEnemies;
    private ObjectPooler _objectPooler;

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
        _currentEnemies.Remove(enemy);
        enemy.GetComponent<ArcherAI>().OnEnemyDie -= MeleeDeath;
    }

    private void MeleeDeath(GameObject enemy)
    {
        _currentMeleeEnemies--;
        _currentEnemies.Remove(enemy);
        enemy.GetComponent<MeleeAI>().OnEnemyDie -= MeleeDeath;
    }

    private async UniTask SpawnEnemiesASync()
    {
        await UniTask.Delay(_spawnDelay * 1000);

        if(_currentArcherEnemies >= _maxArcherEnemies && _currentMeleeEnemies < _maxMeleeEnemies)
        {
            await SpawnMelee(GetSpawnPos());
        }
        else if(_currentMeleeEnemies >= _maxMeleeEnemies && _currentArcherEnemies < _maxArcherEnemies)
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
        _currentEnemies.Add(enemy);

        if(_currentArcherEnemies < _maxArcherEnemies || _currentMeleeEnemies < _maxMeleeEnemies)
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
        _currentEnemies.Add(enemy);

        if(_currentArcherEnemies < _maxArcherEnemies || _currentMeleeEnemies < _maxMeleeEnemies)
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