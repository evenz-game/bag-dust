using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField]
    private Transform startTransform;
    [SerializeField]
    private Transform endTransform;
    [SerializeField]
    private float moveTime;
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private bool isPingpong = true;

    [ContextMenu("Move To Start Position")]
    private void MoveToStartPosition()
    {
        transform.position = startTransform.position;
    }

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        Transform targetTransform = endTransform;

        do
        {
            float timer = 0, percent = 0;
            Transform firstTransform = targetTransform == endTransform ? startTransform : endTransform;

            while (percent < 1)
            {
                timer += Time.deltaTime;
                percent = timer / moveTime;

                transform.position = Vector3.Lerp(
                    firstTransform.position,
                    targetTransform.position,
                    curve.Evaluate(percent)
                );

                yield return null;
            }

            targetTransform = targetTransform == endTransform ? startTransform : endTransform;

        } while (isPingpong);
    }
}
