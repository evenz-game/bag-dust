using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public UnityEvent onFinishedChangeFov = new UnityEvent();

    [SerializeField]
    private float startFOV;
    [SerializeField]
    private float endFOV;
    [SerializeField]
    private float changeFOVTime;
    [SerializeField]
    private AnimationCurve changeFOVCurve;

    private void Start()
    {
        ChangeFOV();
    }

    private void ChangeFOV()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine());
    }

    private IEnumerator ChangeFOVRoutine()
    {
        float timer = 0, percent = 0;

        while (percent < 1 && changeFOVTime > 0)
        {
            timer += Time.deltaTime;
            percent = timer / changeFOVTime;

            Camera.main.fieldOfView = Mathf.Lerp(startFOV, endFOV, changeFOVCurve.Evaluate(percent));

            yield return null;
        }

        Camera.main.fieldOfView = endFOV;
        onFinishedChangeFov.Invoke();
    }

    public interface OnFinishedChangeFov
    {
        public void OnFinishedChangeFov();
    }
}
