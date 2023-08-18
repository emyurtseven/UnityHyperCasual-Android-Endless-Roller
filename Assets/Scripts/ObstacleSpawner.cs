using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages periodic spawning of obstacles.
/// Attached to an object in scene. Spawns from where the object is.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    // spawn points available to spawn obstacles from. picks a random one each time.
    [SerializeField] Transform[] spawnPoints;

    float minSpawnInterval;
    float maxSpawnInterval;
    float obstacleMoveSpeed;

    List<string> obstacles = new List<string>();

    void Start()
    {
        // add pooled objects as name references, to pick a random one later
        Transform objPoolRoot = GameObject.FindGameObjectWithTag("ObstaclePools").transform;

        foreach (Transform item in objPoolRoot)
        {
            obstacles.Add(item.name);
        }

        obstacleMoveSpeed = GameManager.Instance.GameSpeed;
        minSpawnInterval = GameManager.Instance.MinSpawnInterval;
        maxSpawnInterval = GameManager.Instance.MaxSpawnInterval;
    }

    public void OnDifficultyUpListener()
    {
        obstacleMoveSpeed = GameManager.Instance.GameSpeed;
        minSpawnInterval = GameManager.Instance.MinSpawnInterval;
        maxSpawnInterval = GameManager.Instance.MaxSpawnInterval;
    }

    public void StartSpawnCoroutine()
    {
        StartCoroutine(SpawnObstacles());
    }

    /// <summary>
    /// Spawns a random obstacle from the pool and sets its parameters.
    /// </summary>
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // get a random index to pick an obstacle name
            int randomIndex = Random.Range(0, obstacles.Count);
            string name = obstacles[randomIndex];

            // get the obstacle from pool
            GameObject newObstacleObj = ObjectPool.GetPooledObject(name);

            if (!newObstacleObj.TryGetComponent<Obstacle>(out Obstacle newObstacle))
            {
                Debug.LogWarning($"Object {newObstacleObj.name} is missing Obstacle script. Spawning another one");
                continue;
            }

            // set speed
            newObstacleObj.GetComponent<Obstacle>().MoveSpeed = obstacleMoveSpeed;

            // pick a random lane from obstacles alowed lanes
            int allowedLaneIndex = Random.Range(0, newObstacle.AllowedLanes.Length);

            // set spawn point to that lane
            Transform spawnPoint = spawnPoints[newObstacle.AllowedLanes[allowedLaneIndex]];
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, 
                                                    newObstacleObj.transform.position.y, 
                                                    spawnPoint.position.z);

            // relocate and activate obstacle object, add listener for difficultyUpEvent 
            newObstacleObj.transform.position = spawnPosition;
            newObstacleObj.SetActive(true);
            GameManager.Instance.difficultyUpEvent.AddListener(newObstacle.OnDifficultyUpListener);

            // wait random interval before spawning cycle starts again
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);

            yield return new WaitForSeconds(interval);
        }
    }
}
