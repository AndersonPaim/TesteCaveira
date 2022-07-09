using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private GameManager _manager;
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private int _maxArcherEnemies;
    [SerializeField] private int _maxMeleeEnemies;
    [SerializeField] private int _spawnDelay;

    private int _currentArcherEnemies;
    private int _currentMeleeEnemies;
    private ObjectPooler _objectPooler;

    private async UniTask Start()
    {
        _objectPooler = _manager.ObjectPooler;
        await SpawnEnemies();
    }

    private async UniTask SpawnEnemies()
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

        if(_currentArcherEnemies < _maxArcherEnemies || _currentMeleeEnemies < _maxMeleeEnemies)
        {
            await SpawnEnemies();
        }
    }

    private async UniTask SpawnMelee(Transform pos)
    {
        _currentMeleeEnemies++;
        GameObject enemy = _objectPooler.SpawnFromPool(ObjectsTag.ArcherEnemy);
        enemy.transform.position = pos.position;

        if(_currentArcherEnemies < _maxArcherEnemies || _currentMeleeEnemies < _maxMeleeEnemies)
        {
            await SpawnEnemies();
        }
    }

    private Transform GetSpawnPos()
    {
        int randomPos = Random.Range(0, _spawnPositions.Count);

        return _spawnPositions[randomPos];
    }

}