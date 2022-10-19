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
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private bool isPingpong = true;

    [ContextMenu("Set Start Position")]
    private void SetStartPosition()
    {
        startPosition = transform.position;
    }

    [ContextMenu("Set End Position")]
    private void SetEndPosition()
    {
        endPosition = transform.position;
    }

    [ContextMenu("Move To Start Position")]
    private void MoveToStartPosition()
    {
        transform.position = startPosition;
    }

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        do
        {
            float timer = 0, percent = 0;
            Vector3 firstPosition = transform.position;

            while (percent < 1)
            {
                timer += Time.deltaTime;
                percent = timer / moveTime;

                transform.position = Vector3.Lerp(firstPosition, endPosition, curve.Evaluate(percent));

                yield return null;
            }

            Vector3 temp = endPosition;
            endPosition = startPosition;
            startPosition = temp;

        } while (isPingpong);
    }
}
