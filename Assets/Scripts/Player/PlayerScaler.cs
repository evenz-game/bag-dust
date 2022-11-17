using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaler : PlayerComponent, PlayerStatus.OnChangedScale, PlayerModel.OnInitializedPlayerModel
{
    private PlayerModelInfo model;

    [Header("Time")]
    [SerializeField]
    private float changeTime;                   // 스케일 값에 의해 크기가 변하는 시간 

    public void OnChangedScale(float previousScale, float currentScale)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScaleRoutine(currentScale));
    }

    public void OnInitializedPlayerModel(PlayerModelInfo model)
    {
        this.model = model;
    }

    private IEnumerator ChangeScaleRoutine(float targetScale)
    {
        float timer = 0, percent = 0;
        float startScale = model.BodyTransform.localScale.x;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / changeTime;

            float scale = Mathf.Lerp(startScale, targetScale, percent);

            UpdateBodyScale(scale);
            UpdateHeadPosition(scale);

            yield return null;
        }
    }

    private void UpdateBodyScale(float targetScale)
    {
        model.BodyTransform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    private void UpdateHeadPosition(float targetScale)
    {
        float sin = (model.FaceHeight / 2f) / targetScale;       // 높이 / 반지름(빗변)
        float rad = Mathf.Asin(sin);
        float y = Mathf.Cos(rad) * targetScale * 0.01f;

        Transform faceTransform = model.PlayerFaceTransform;
        faceTransform.localPosition = new Vector3(faceTransform.localPosition.x, y, faceTransform.localPosition.z);
    }
}
