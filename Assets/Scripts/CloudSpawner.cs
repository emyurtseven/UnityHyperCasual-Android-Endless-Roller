using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] float cloudMoveSpeed = 25f;
    [SerializeField] float meanSpawnInterval = 2f;
    List<GameObject> activeCloudObjects = new List<GameObject>();

    private void Start() 
    {
        StartCoroutine(SpawnClouds());
        StartCoroutine(DespawnClouds());    
    }

    private IEnumerator SpawnClouds()
    {
        while (true)
        {
            GameObject newCloud = ObjectPool.GetPooledObject("Cloud");
            activeCloudObjects.Add(newCloud);

            float posX = Random.Range(8f, 35f);
            int sign = Random.Range(0, 2) * 2 - 1;
            posX *= sign;

            Vector3 position = new Vector3(posX,
                                            Random.Range(5, 15),
                                            170f);

            newCloud.transform.position = position;
            float scaleFactor = Random.Range(1f, 3f);
            newCloud.transform.localScale = Vector3.one * scaleFactor;
            newCloud.SetActive(true);

            float interval = Random.Range(meanSpawnInterval - 0.5f, meanSpawnInterval + 0.5f);
            yield return new WaitForSeconds(interval);
        }
    }

    private void Update() 
    {
        for (int i = activeCloudObjects.Count - 1; i >= 0 ; i--)
        {
            activeCloudObjects[i].transform.Translate(Vector3.back * cloudMoveSpeed * Time.deltaTime);
        }
    }


    private IEnumerator DespawnClouds()
    {
        while (true)
        {
            for (int i = activeCloudObjects.Count - 1; i >= 0; i--)
            {
                if (activeCloudObjects[i].transform.position.z < -10)
                {
                    ObjectPool.ReturnPooledObject(activeCloudObjects[i].name, activeCloudObjects[i]);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
