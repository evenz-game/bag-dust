using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private TextMeshProUGUI textDustPercentLeft;
    [SerializeField]
    private TextMeshProUGUI textDustPercentRight;
    [SerializeField]
    private bool displayCount = false;

    [Header("Fill Image Dust Count")]
    [SerializeField]
    private float fillImageDustCountTime = 0.4f;
    [SerializeField]
    private AnimationCurve fillImageDustCountCurve;
    [SerializeField]
    private AnimationCurve textDustPercentScaleCurve;

    private void Awake()
    {
        playerStatus.onChangedDustCount.AddListener(OnChangedDustCount);
        playerModel.onInitializedPlayerModel.AddListener((PlayerModelInfo info) =>
        {
            imgaeCharacter.transform.position = targetPositions[playerStatus.Index];
            imgaeCharacter.gameObject.SetActive(true);
            imgaeCharacter.sprite = characterSprites[info.ModelIndex];

            if (playerStatus.Index % 2 == 0)
                textDustPercentLeft.gameObject.SetActive(true);
            else
                textDustPercentRight.gameObject.SetActive(true);
        });
    }

    public void OnChangedDustCount(int previousDustCount, int currentDustCount)
    {
        StopAllCoroutines();
        StartCoroutine(FillImageDustCountRoutine(currentDustCount));
    }

    private int prevPercent = 0;
    private IEnumerator FillImageDustCountRoutine(int currentDustCount)
    {
        float timer = 0, percent = 0;

        int startPercent = prevPercent;
        int targetPercent = (int)((float)currentDustCount / (float)playerStatus.MaxDustCount * 100);

        int currentPercent = startPercent;
        string percentString = "";
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / fillImageDustCountTime;

            currentPercent = (int)Mathf.Lerp(startPercent, targetPercent, fillImageDustCountCurve.Evaluate(percent));
            prevPercent = currentPercent;

            percentString = $"{ currentPercent.ToString()}<size=30>%</size>";
            if (displayCount)
                percentString = currentDustCount.ToString();

            textDustPercentLeft.text = percentString;
            textDustPercentRight.text = percentString;

            float scale = Mathf.Lerp(1, 1.2f, textDustPercentScaleCurve.Evaluate(percent));
            textDustPercentLeft.transform.localScale = Vector3.one * scale;
            textDustPercentRight.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        percentString = $"{ currentPercent.ToString()}<size=30>%</size>";
        if (displayCount)
            percentString = currentDustCount.ToString();

        textDustPercentLeft.text = percentString;
        textDustPercentRight.text = percentString;
    }

    [ContextMenu("Add Target Position")]
    private void AddTargetPosition()
    {
        targetPositions.Add(playerInfoTransform.position);
    }
}

