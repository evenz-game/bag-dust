using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFlyingObjectSpawnController : MonoBehaviour
{
    [Header("Position")]
    [SerializeField]
    private Transform minSpawnTransform;
    [SerializeField]
    private Transform maxSpawnTransform;

    [Header("Time")]
    [SerializeField]
    private float minSpawnDeltaTime;
    [SerializeField]
    private float maxSpawnDeltaTime;

    [Header("Obstacle")]
    [SerializeField]
    private ObstacleFlyingObject[] obstaclePrefabs;

    private bool isSpawnable = true;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDeltaTime, maxSpawnDeltaTime));

            Spawn();

            // 스폰 가능할 때까지 기다림
            while (!isSpawnable)
                yield return null;
        }
    }

    private void Spawn()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);

        Vector3 minPos = minSpawnTransform.position;
        Vector3 maxPos = maxSpawnTransform.position;

        ObstacleFlyingObject obstacleClone
            = Instantiate<ObstacleFlyingObject>(
                obstaclePrefabs[index],
                new Vector3(
                    Random.Range(minPos.x, maxPos.x),
                    Random.Range(minPos.y, maxPos.y),
                    Random.Range(minPos.z, maxPos.z)
                ),
                Quaternion.identity
            );

        isSpawnable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<ObstacleFlyingObject>(other.gameObject, out _))
        {
            isSpawnable = true;
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(minSpawnTransform.position, maxSpawnTransform.position);
    }
}
