using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabPaths
{
    public const string obstaclesRootPath = "Obstacles/Prefabs";
    public const string debugRootPath = "Obstacles/Debug";
}

/// <summary>
/// Provides object pooling for bullets and enemies
/// </summary>
public static class ObjectPool
{
    static int prefabPoolSize;
    
    static Dictionary<string, List<GameObject>> objectPools;
    static Dictionary<string, GameObject> pooledPrefabs;

    /// <summary>
    /// Initializes the pools, call in game manager
    /// </summary>
    public static void Initialize(int poolSize)
    {
        prefabPoolSize = poolSize;
        objectPools = new Dictionary<string, List<GameObject>>();
        pooledPrefabs = new Dictionary<string, GameObject>();

        GameObject objectPoolsHolder = new GameObject("ObjectPools");
        objectPoolsHolder.tag = "ObjectPools";

        GameObject[] prefabs;
        if (GameManager.Instance.DebugMode)
        {
            prefabs = Resources.LoadAll<GameObject>(PrefabPaths.debugRootPath);
        }
        else
        {
            prefabs = Resources.LoadAll<GameObject>(PrefabPaths.obstaclesRootPath);
        }

        foreach (var prefab in prefabs)
        {
            string name = prefab.name;
            GameObject child = new GameObject(name);
            child.transform.parent = objectPoolsHolder.transform;

            pooledPrefabs.Add(name, prefab);
            objectPools.Add(name, new List<GameObject>(prefabPoolSize));

            for (int i = 0; i < objectPools[name].Capacity; i++)
            {
                objectPools[name].Add(GetNewObject(name));
            }
        }
    }

    /// <summary>
    /// Gets a pooled object from the pool
    /// </summary>
    /// <returns>pooled object</returns>
    /// <param name="name">name of the pooled object to get</param>
    public static GameObject GetPooledObject(string name)
    {
        if (!objectPools.ContainsKey(name))
        {
            Debug.LogError($"Key {name} is missing from the object pool");
            GameObject placeholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            return placeholder;
        }

        List<GameObject> pool = objectPools[name];

        // check for available object in pool
        if (pool.Count > 0)
        {
            // remove object from pool and return (replace code below)
            int i = UnityEngine.Random.Range(0, pool.Count);
            GameObject obj = pool[i];
            pool.RemoveAt(i);
            return obj;
        }
        else
        {
            // pool empty, so expand pool and return new object (replace code below)
            pool.Capacity++;
            return GetNewObject(name);
        }
    }

    /// <summary>
    /// Returns a pooled object to the pool
    /// </summary>
    /// <param name="name">name of pooled object</param>
    /// <param name="obj">object to return to pool</param>
    public static bool ReturnPooledObject(string name,
        GameObject obj)
    {
        if (objectPools == null)
        {
            return false;
        }
        else if (!objectPools.ContainsKey(name))
        {
            return false;
        }
        
        obj.SetActive(false);
        objectPools[name].Add(obj);

        return true;
    }

    /// <summary>
    /// Gets a new object
    /// </summary>
    /// <returns>new object</returns>
    static GameObject GetNewObject(string type)
    {
        if (pooledPrefabs[type] == null)
        {
            Debug.LogWarning($"{type} pooled obj dictionary has no items. Check path strings or make sure asset exists at correct path.");
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            return sphere;
        }
        
        GameObject obj = GameObject.Instantiate(pooledPrefabs[type], GameObject.Find(type).transform);
        obj.name = pooledPrefabs[type].name;

        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Removes all the pooled objects from the object pools
    /// </summary>
    public static void EmptyPools()
    {
        // add your code here
        foreach (KeyValuePair<string, List<GameObject>> keyValuePair in objectPools)
        {
            keyValuePair.Value.Clear();
        }
    }


    /// <summary>
    /// Gets the current pool count for the given pooled object
    /// </summary>
    /// <param name="type">pooled object name</param>
    /// <returns>current pool count</returns>
    public static int GetPoolCount(string type)
    {
        if (objectPools.ContainsKey(type))
        {
            return objectPools[type].Count;
        }
        else
        {
            // should never get here
            return -1;
        }
    }

    /// <summary>
    /// Gets the current pool capacity for the given pooled object
    /// </summary>
    /// <param name="name">pooled object name</param>
    /// <returns>current pool capacity</returns>
    public static int GetPoolCapacity(string name)
    {
        if (objectPools.ContainsKey(name))
        {
            return objectPools[name].Capacity;
        }
        else
        {
            // should never get here
            return -1;
        }
    }
}

