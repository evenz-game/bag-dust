using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private float moveTime;

    [ContextMenu("Set Start Position")]
    private void SetStartPosition()
    {
        startPosition = transform.position;
    }

    private void Awake()
    {
        endPosition = transform.position;
        transform.position = startPosition;
    }

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / moveTime;

            transform.position = Vector3.Lerp(startPosition, endPosition, percent);

            yield return null;
        }
    }
}
