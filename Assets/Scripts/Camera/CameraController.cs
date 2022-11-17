using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;

    public UnityEvent onFinishedCameraWalking = new UnityEvent();

    [SerializeField]
    private List<CameraWalkingInfo> cameraWalkingInfos;
    private bool isWalking = false;

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Add Camera Walking Info")]
    private void AddCameraWalkingInfo()
    {
        CameraWalkingInfo info = new CameraWalkingInfo();
        info.targetFOV = Camera.main.fieldOfView;
        info.targetPosition = Camera.main.transform.position; ;

        cameraWalkingInfos.Add(info);
    }

    [ContextMenu("Set To Start Camera Position And FOV")]
    private void SetToStartCameraPositionAndFOV()
    {
        if (cameraWalkingInfos.Count == 0) return;

        CameraWalkingInfo info = cameraWalkingInfos[0];
        Camera.main.fieldOfView = info.targetFOV;
        Camera.main.transform.position = info.targetPosition;
    }

    public void StartCameraWalking()
    {
        if (isWalking) return;

        StopAllCoroutines();
        StartCoroutine(CameraWalkingRoutine());
    }

    private IEnumerator CameraWalkingRoutine()
    {
        isWalking = true;

        foreach (CameraWalkingInfo info in cameraWalkingInfos)
        {
            yield return StartCoroutine(CameraWalkingRoutineByInfo(info));
            yield return new WaitForSeconds(info.stayTime);
        }

        isWalking = false;

        onFinishedCameraWalking.Invoke();
    }

    private IEnumerator CameraWalkingRoutineByInfo(CameraWalkingInfo info)
    {
        float timer = 0, percent = 0;

        float startFOV = Camera.main.fieldOfView;
        Vector3 startPosition = Camera.main.transform.position;

        while (percent < 1 && info.walkingTime > 0)
        {
            timer += Time.deltaTime;
            percent = timer / info.walkingTime;

            Camera.main.fieldOfView
                = Mathf.Lerp(startFOV, info.targetFOV, info.walkingCurve.Evaluate(percent));
            Camera.main.transform.position
                = Vector3.Lerp(startPosition, info.targetPosition, info.walkingCurve.Evaluate(percent));

            yield return null;
        }

        Camera.main.fieldOfView = info.targetFOV;
        Camera.main.transform.position = info.targetPosition;

        info.onFinishedCameraWalking.Invoke();
    }

    public interface OnFinishedCameraWalking
    {
        public void OnFinishedCameraWalking();
    }

    [Serializable]
    private class CameraWalkingInfo
    {
        public float walkingTime;
        public AnimationCurve walkingCurve;
        public float targetFOV;
        public Vector3 targetPosition;
        public float stayTime;
        public UnityEvent onFinishedCameraWalking = new UnityEvent();
    }

    public static void Shake(float amount, float duration)
    {
        if (instance == null || instance.isWalking) return;

        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShakeRoutine(amount, duration));
    }

    private IEnumerator ShakeRoutine(float amount, float duration)
    {
        float timer = 0;
        Vector3 originPos = Camera.main.transform.position;

        while (timer <= duration)
        {
            Camera.main.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originPos;
    }
}
