using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraWalkingController : MonoBehaviour
{
    private static List<CameraWalkingController> instances = new List<CameraWalkingController>();

    public UnityEvent onFinishedCameraWalking = new UnityEvent();

    [SerializeField]
    private Camera manualCamera;
    private Camera targetCamera;
    [SerializeField]
    private List<CameraWalkingInfo> cameraWalkingInfos;
    private bool isWalking = false;

    private Vector3 startPosition;

    private void Awake()
    {
        instances.Add(this);
    }

    private void Start()
    {
        targetCamera = manualCamera ? manualCamera : Camera.main;
        startPosition = targetCamera.transform.position;
    }

    [ContextMenu("Add Camera Walking Info")]
    private void AddCameraWalkingInfo()
    {
        CameraWalkingInfo info = new CameraWalkingInfo();
        info.targetFOV = targetCamera.fieldOfView;
        info.targetPosition = targetCamera.transform.position; ;

        cameraWalkingInfos.Add(info);
    }

    [ContextMenu("Set To Start Camera Position And FOV")]
    private void SetToStartCameraPositionAndFOV()
    {
        if (cameraWalkingInfos.Count == 0) return;

        CameraWalkingInfo info = cameraWalkingInfos[0];
        targetCamera.fieldOfView = info.targetFOV;
        targetCamera.transform.position = info.targetPosition;
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
            if (info.disable) continue;

            yield return StartCoroutine(CameraWalkingRoutineByInfo(info));
            yield return new WaitForSeconds(info.stayTime);
        }

        isWalking = false;

        onFinishedCameraWalking.Invoke();
    }

    private IEnumerator CameraWalkingRoutineByInfo(CameraWalkingInfo info)
    {
        float timer = 0, percent = 0;

        float startFOV = targetCamera.fieldOfView;
        Vector3 startPosition = targetCamera.transform.position;

        while (percent < 1 && info.walkingTime > 0)
        {
            timer += Time.deltaTime;
            percent = timer / info.walkingTime;

            targetCamera.fieldOfView
                = Mathf.Lerp(startFOV, info.targetFOV, info.walkingCurve.Evaluate(percent));
            targetCamera.transform.position
                = Vector3.Lerp(startPosition, info.targetPosition, info.walkingCurve.Evaluate(percent));

            yield return null;
        }

        targetCamera.fieldOfView = info.targetFOV;
        targetCamera.transform.position = info.targetPosition;

        info.onFinishedCameraWalking.Invoke();
    }

    public static void RemoveWalkingInfoByCode(int code)
    {
        foreach (CameraWalkingController inst in instances)
            inst?._RemoveWalkingInfoByCode(code);
    }

    private void _RemoveWalkingInfoByCode(int code)
    {

        for (int i = 0; i < cameraWalkingInfos.Count; i++)
        {
            CameraWalkingInfo info = cameraWalkingInfos[i];
            if (info.code == code)
                info.disable = true;
        }

    }

    public interface OnFinishedCameraWalking
    {
        public void OnFinishedCameraWalking();
    }

    [Serializable]
    private class CameraWalkingInfo
    {
        public int code;
        public bool disable = false;
        public float walkingTime;
        public AnimationCurve walkingCurve;
        public float targetFOV;
        public Vector3 targetPosition;
        public float stayTime;
        public UnityEvent onFinishedCameraWalking = new UnityEvent();
    }

    public static void Shake(float amount, float duration)
    {
        foreach (CameraWalkingController instance in instances)
        {
            if (!instance) continue;
            if (instance.isWalking) continue;
            instance?.StopAllCoroutines();
            instance?.StartCoroutine(instance?.ShakeRoutine(amount, duration));
        }
    }

    private IEnumerator ShakeRoutine(float amount, float duration)
    {
        float timer = 0;
        Vector3 originPos = targetCamera.transform.position;

        while (timer <= duration)
        {
            targetCamera.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }

        targetCamera.transform.position = startPosition;
    }
}
