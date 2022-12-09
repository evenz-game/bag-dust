using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : PlayerUI, PlayerStatus.OnChangedDustCount
{
    [Header("Target")]
    [SerializeField]
    private PlayerModel playerModel;

    [Space]
    [SerializeField]
    private Transform playerInfoTransform;
    [SerializeField]
    private List<Vector3> targetPositions;

    [Space]
    [SerializeField]
    private List<Sprite> characterSprites;

    [Header("UI Components")]
    [SerializeField]
    private Image imgaeCharacter;
    [SerializeField]
    private Image imageDustCount;

    [Header("Fill Image Dust Count")]
    [SerializeField]
    private float fillImageDustCountTime = 0.4f;
    [SerializeField]
    private AnimationCurve fillImageDustCountCurve;

    private void Awake()
    {
        playerStatus.onChangedDustCount.AddListener(OnChangedDustCount);
        playerModel.onInitializedPlayerModel.AddListener((PlayerModelInfo info) =>
        {
            imgaeCharacter.transform.position = targetPositions[playerStatus.Index];
            imgaeCharacter.gameObject.SetActive(true);
            imgaeCharacter.sprite = characterSprites[info.ModelIndex];
        });
    }

    public void OnChangedDustCount(int previousDustCount, int currentDustCount)
    {
        StopAllCoroutines();
        StartCoroutine(FillImageDustCountRoutine(currentDustCount));
    }

    private IEnumerator FillImageDustCountRoutine(int currentDustCount)
    {
        float timer = 0, percent = 0;
        float currentFill = imageDustCount.fillAmount;
        float targetFill = (float)currentDustCount / (float)playerStatus.MaxDustCount;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / fillImageDustCountTime;

            float fill = Mathf.Lerp(currentFill, targetFill, fillImageDustCountCurve.Evaluate(percent));

            imageDustCount.fillAmount = fill;

            yield return null;
        }

        imageDustCount.fillAmount = targetFill;
    }

    [ContextMenu("Add Target Position")]
    private void AddTargetPosition()
    {
        targetPositions.Add(playerInfoTransform.position);
    }
}

