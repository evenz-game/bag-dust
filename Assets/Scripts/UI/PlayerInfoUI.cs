using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : MonoBehaviour, PlayerStatus.OnChangedDustCount
{
    [Header("Player Status")]
    [SerializeField]
    private PlayerStatus playerStatus;

    [Header("Target Position")]
    [SerializeField]
    private List<Vector3> targetPositions;

    [Header("UI Components")]
    [SerializeField]
    private TextMeshProUGUI textPlayerIndex;
    [SerializeField]
    private Image imageDustCount;

    [Header("Fill Image Dust Count")]
    [SerializeField]
    private float fillImageDustCountTime = 0.4f;
    [SerializeField]
    private AnimationCurve fillImageDustCountCurve;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        transform.position = targetPositions[playerStatus.Index];
        textPlayerIndex.text = playerStatus.Index.ToString();
        imageDustCount.fillAmount = (float)playerStatus.CurrentDustCount / (float)playerStatus.MaxDustCount;
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
        targetPositions.Add(transform.position);
    }
}
