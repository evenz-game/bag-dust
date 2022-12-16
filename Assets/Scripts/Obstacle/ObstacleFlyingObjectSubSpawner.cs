using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFlyingObjectSubSpawner : MonoBehaviour
{
    [SerializeField]
    private ObstacleFlyingObject subObstaclePrefab;
    [SerializeField]
    private int subSpawnCount;
    [SerializeField]
    private Transform originSpawnTransform;
    [SerializeField]
    private Vector2 subSpawnDeltaDistance;
    [SerializeField]
    private float subSpawnDeltaTime;

    private void Awake()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        Vector2 startPos = transform.position;

        for (int i = 0; i < subSpawnCount; i++)
        {
            yield return new WaitForSeconds(subSpawnDeltaTime);
            Vector2 spawnPos = startPos + (subSpawnDeltaDistance * (i + 1));

            Instantiate(subObstaclePrefab, spawnPos, Quaternion.identity);
        }
    }
}
