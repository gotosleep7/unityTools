using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace qin_makeface
{


    public class GameObjectPoolUtils
    {
        private static GameObjectPoolUtils _instance;
        public static GameObjectPoolUtils Instance
        {
            get
            {
                if (_instance == null) _instance = new GameObjectPoolUtils();
                return _instance;
            }
        }
        Dictionary<string, GameObjectPool> dict = new Dictionary<string, GameObjectPool>();
        public T Get<T>(string key, GameObject prefab)
        {
            return Get(key, prefab).GetComponent<T>();
        }

        public GameObject Get(string key, GameObject prefab)
        {
            GameObjectPool pool;
            if (!dict.TryGetValue(key, out pool))
            {
                pool = new GameObjectPool();
                pool.Prefab = prefab;
                dict[key] = pool;
            }
            return pool.Pool.Get();
        }

        public void Release(string key, GameObject obj)
        {
            if (dict.TryGetValue(key, out var pool))
            {
                pool.Pool.Release(obj);
            }
        }


    }


    public class GameObjectPool
    {
        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        IObjectPool<GameObject> m_Pool;
        public GameObject Prefab;
        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new ObjectPool<GameObject>(CreatePooledItem,
                                                                OnTakeFromPool,
                                                                OnReturnedToPool,
                                                                OnDestroyPoolObject,
                                                                collectionChecks,
                                                                10,
                                                                maxPoolSize);
                    else
                        m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }

        // public IObjectPool<GameObject> GeneratePool(GameObject prefab, PoolType poolType = PoolType.Stack)
        // {
        //     IObjectPool<GameObject> pool = null;
        //     if (poolType == PoolType.Stack)
        //         pool = new ObjectPool<GameObject>(CreatePooledItem,
        //                                                 OnTakeFromPool,
        //                                                 OnReturnedToPool,
        //                                                 OnDestroyPoolObject,
        //                                                 collectionChecks,
        //                                                 10,
        //                                                 maxPoolSize);
        //     else
        //         pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
        //     return pool;
        // }

        GameObject CreatePooledItem()
        {
            var go = GameObject.Instantiate(Prefab);
            return go;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GameObject system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GameObject system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GameObject system)
        {
            GameObject.Destroy(system.gameObject);
        }

    }
}