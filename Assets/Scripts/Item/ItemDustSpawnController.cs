using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDustSpawnController : MonoBehaviour
{
    [Header("Item")]
    [SerializeField]
    private ItemDust itemDustPrefab;

    [Header("Position")]
    [SerializeField]
    private Transform minSpawnTransform;
    [SerializeField]
    private Transform maxSpawnTransform;

    [Header("Count")]
    [SerializeField]
    private int minSpawnCount;
    [SerializeField]
    private int maxSpawnCount;

    [Header("Time")]
    [SerializeField]
    private float minSpawnDeltaTime;
    [SerializeField]
    private float maxSpawnDeltaTime;

    [Header("Force")]
    [SerializeField]
    private float minSpawnForce;
    [SerializeField]
    private float maxSpawnForce;

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
        }
    }

    private void Spawn()
    {
        int count = Random.Range(minSpawnCount, maxSpawnCount);

        Vector3 minPos = minSpawnTransform.position;
        Vector3 maxPos = maxSpawnTransform.position;

        for (int i = 0; i < count; i++)
        {
            ItemDust itemClone
                = Instantiate<ItemDust>(
                    itemDustPrefab,
                    new Vector3(
                        Random.Range(minPos.x, maxPos.x),
                        Random.Range(minPos.y, maxPos.y),
                        Random.Range(minPos.z, maxPos.z)
                    ),
                    Quaternion.identity
                );

            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(0, -1f)).normalized;
            Vector2 force = dir * Random.Range(minSpawnForce, maxSpawnForce);
            itemClone.Scatter(force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(minSpawnTransform.position, maxSpawnTransform.position);
    }
}
