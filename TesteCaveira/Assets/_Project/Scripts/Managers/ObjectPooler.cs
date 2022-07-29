using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    [SerializeField]  private List<Pool> _pools;

    private Dictionary<int, List<GameObject>> _poolDictionary;

    private List<GameObject> _objectPool;


    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _poolDictionary = new Dictionary<int, List<GameObject>>();

        foreach (Pool pool in _pools)
        {
            _objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, gameObject.transform, true);
                obj.SetActive(false);
                _objectPool.Add(obj);
            }

            _poolDictionary.Add(pool.prefab.GetInstanceID(), _objectPool);
        }
    }


    public GameObject SpawnFromPool(int id)
    {
        bool isPoolAvailable = false;
        GameObject objectToSpawn = null;

        for(int i = 0; i < _poolDictionary[id].Count; i++)
        {
            if(!_poolDictionary[id][i].activeInHierarchy)
            {
                isPoolAvailable = true;
                objectToSpawn = _poolDictionary[id][i];
            }
        }

        if (!isPoolAvailable)
        {
            return AddToPool(id);
        }
        else
        {
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
    }

    private GameObject AddToPool(int id)
    {
        GameObject newObject = _poolDictionary[id][0];
        _poolDictionary[id].Add(newObject);
        newObject.SetActive(true);

        GameObject obj = Instantiate(newObject, gameObject.transform, true);
        return obj;
    }
}