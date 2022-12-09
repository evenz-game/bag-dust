using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField]
    private Camera manualCamera;
    private Camera targetCamera;

    [Header("Wall")]
    [SerializeField]
    private Transform wallTransform;

    [Header("Camera Zoom Infos")]
    [SerializeField]
    private List<CameraZoomInfo> cameraZoomInfos;

    private void Start()
    {
        targetCamera = manualCamera ? manualCamera : Camera.main;
    }

    public void ZoomByInfoName(string name)
    {
        foreach (CameraZoomInfo info in cameraZoomInfos)
        {
            if (info.name.Equals(name))
            {
                StopAllCoroutines();
                StartCoroutine(ZoomRoutineByInfo(info));
                break;
            }
        }
    }

    private IEnumerator ZoomRoutineByInfo(CameraZoomInfo info)
    {
        float timer = 0, percent = 0;

        float startCameraFOV = targetCamera.fieldOfView;
        Vector3 startWallScale = wallTransform.localScale;

        while (percent < 1 && info.zoomTime > 0)
        {
            timer += Time.deltaTime;
            percent = timer / info.zoomTime;

            targetCamera.fieldOfView
                = Mathf.Lerp(startCameraFOV, info.targetCameraFOV, info.zoomCurve.Evaluate(percent));
            wallTransform.localScale
                = Vector3.Lerp(
                    startWallScale,
                    new Vector3(info.targetWallScale, info.targetWallScale, info.targetWallScale),
                    info.zoomCurve.Evaluate(percent)
                );

            yield return null;
        }

        targetCamera.fieldOfView = info.targetCameraFOV;
        wallTransform.localScale = new Vector3(info.targetWallScale, info.targetWallScale, info.targetWallScale);

        info.onFinishedCameraZoom.Invoke();
    }

    [Serializable]
    private class CameraZoomInfo
    {
        public string name;
        public float zoomTime;
        public float targetCameraFOV;
        public float targetWallScale;
        public AnimationCurve zoomCurve;
        public UnityEvent onFinishedCameraZoom = new UnityEvent();
    }

}
