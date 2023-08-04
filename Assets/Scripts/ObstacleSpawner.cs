using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;

    float minSpawnInterval;
    float maxSpawnInterval;
    float obstacleMoveSpeed;

    List<string> obstacles = new List<string>();

    void Start()
    {
        Transform objPoolRoot = GameObject.FindGameObjectWithTag("ObjectPools").transform;

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

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            int index = Random.Range(0, obstacles.Count);

            string name = obstacles[index];

            GameObject newObstacleObj = ObjectPool.GetPooledObject(name);

            Obstacle newObstacle = newObstacleObj.GetComponent<Obstacle>();
            newObstacleObj.GetComponent<Obstacle>().MoveSpeed = obstacleMoveSpeed;

            int allowedLaneIndex = Random.Range(0, newObstacle.AllowedLanes.Length);

            Transform spawnPoint = spawnPoints[newObstacle.AllowedLanes[allowedLaneIndex]];

            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, 
                                                    newObstacleObj.transform.position.y, 
                                                    spawnPoint.position.z);

            newObstacleObj.transform.position = spawnPosition;
            newObstacleObj.SetActive(true);
            GameManager.Instance.difficultyUpEvent.AddListener(newObstacle.OnDifficultyUpListener);

            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);

            yield return new WaitForSeconds(interval);
        }
    }
}
