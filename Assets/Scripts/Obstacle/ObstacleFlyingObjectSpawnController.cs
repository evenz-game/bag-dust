using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFlyingObjectSpawnController : MonoBehaviour, GameController.OnStartedGame
{
    [Header("Position - Top")]
    [SerializeField]
    private Transform minSpawnTransform;
    [SerializeField]
    private Transform maxSpawnTransform;

    [Header("Position - Bottom")]
    [SerializeField]
    private Transform minBottomSpawnTransform;
    [SerializeField]
    private Transform maxBottomSpawnTransform;

    [Header("Level")]
    [SerializeField]
    private Level[] levels;
    [SerializeField]
    private int startLevelIndex = 0;
    private int currentLevelIndex;
    private Level currentLevel;

    [Header("Obstacle")]
    [SerializeField]
    private ObstacleFlyingObject[] obstaclePrefabs;
    private Dictionary<ObstacleFlyingObject, int> spawnCount = new Dictionary<ObstacleFlyingObject, int>();

    private bool isSpawnable = true;

    private void Awake()
    {
        foreach (var obstaclePrefab in obstaclePrefabs)
            spawnCount.Add(obstaclePrefab, 0);

        currentLevelIndex = startLevelIndex - 1;
        NextLevel();
    }

    public void NextLevel()
    {
        if (currentLevelIndex >= levels.Length - 1) return;

        currentLevelIndex++;
        currentLevel = levels[currentLevelIndex];
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(currentLevel.minSpawnDeltaTime, currentLevel.maxSpawnDeltaTime));

            Spawn();

            // 스폰 가능할 때까지 기다림
            while (!isSpawnable)
                yield return null;
        }
    }

    private void Spawn()
    {
        // 제일 작은 카운드 탐색
        int minCnt = int.MaxValue;
        foreach (var key in spawnCount.Keys)
        {
            int cnt = spawnCount[key];
            if (cnt < minCnt)
                minCnt = cnt;
        }

        // 작은 카운트들만 모으기
        List<ObstacleFlyingObject> prefabs = new List<ObstacleFlyingObject>();
        foreach (var key in spawnCount.Keys)
        {
            int cnt = spawnCount[key];
            if (cnt == minCnt)
                prefabs.Add(key);
        }

        // 프리팹 선택
        int index = Random.Range(0, prefabs.Count);

        ObstacleFlyingObject obstacle = prefabs[index];
        spawnCount[obstacle]++;

        Vector3 minPos = minSpawnTransform.position;
        Vector3 maxPos = maxSpawnTransform.position;

        if (obstacle.SpawnBottom)
        {
            minPos = minBottomSpawnTransform.position;
            maxPos = maxBottomSpawnTransform.position;
        }

        ObstacleFlyingObject obstacleClone
            = Instantiate<ObstacleFlyingObject>(
                obstacle,
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
        if (GameObjectUtils.FindCompoenet<ObstacleFlyingObject>(other.gameObject, out ObstacleFlyingObject obstacle))
        {
            if (obstacle.IsSubObstacle) return;

            isSpawnable = true;
            Destroy(other.transform.root.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(minSpawnTransform.position, maxSpawnTransform.position);
        Gizmos.DrawLine(minBottomSpawnTransform.position, maxBottomSpawnTransform.position);
    }

    public void OnStartedGame()
    {
        StartCoroutine(SpawnRoutine());
    }

    [System.Serializable]
    private class Level
    {
        public float minSpawnDeltaTime;
        public float maxSpawnDeltaTime;
    }
}
