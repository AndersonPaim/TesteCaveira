using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public ObjectsTag tag;
        public int size;
    }

    [SerializeField]  private List<Pool> _pools;

    private Dictionary<ObjectsTag, List<GameObject>> _poolDictionary;

    private List<GameObject> _objectPool;


    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _poolDictionary = new Dictionary<ObjectsTag, List<GameObject>>();

        foreach (Pool pool in _pools)
        {
            _objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = gameObject.transform;
                _objectPool.Add(obj);
            }

            _poolDictionary.Add(pool.tag, _objectPool);
        }
    }


    public GameObject SpawnFromPool(ObjectsTag id)
    {
        bool isPoolAvailable = false;
        GameObject objectToSpawn = null;

        for(int i = 0; i < _poolDictionary[id].Count; i++) //search for a disabled object in the pool
        {
            if(!_poolDictionary[id][i].activeInHierarchy)
            {
                isPoolAvailable = true;
                objectToSpawn = _poolDictionary[id][i];
            }
        }

        if (!isPoolAvailable)
        {
            return AddToPool(id); //if every object in the pool is active add new object
        }
        else
        {
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }
    }

    private GameObject AddToPool(ObjectsTag id) //add new object to the pool
    {
        GameObject newObject = _poolDictionary[id][0];
        _poolDictionary[id].Add(newObject);
        newObject.SetActive(true);

        GameObject obj = Instantiate(newObject);
        obj.transform.SetParent(gameObject.transform);
        return obj;
    }
}