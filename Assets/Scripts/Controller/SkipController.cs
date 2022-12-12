using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipController : MonoBehaviour
{
    public UnityEvent onSkip = new UnityEvent();
    private bool skip = false;

    [SerializeField]
    private Image imageSkip;
    [SerializeField]
    private float fillTime;

    [Space]
    [SerializeField]
    private float minScale;
    [SerializeField]
    private float maxScale;

    [SerializeField]
    private float animationTime;
    [SerializeField]
    private AnimationCurve animationCurve;

    [Space]
    [SerializeField]
    private InputterSetting inputterSetting;
    private KeyCode targetKeyCode = KeyCode.Joystick1Button0;

    private Coroutine fillRoutine = null;

    private void Update()
    {
        if (skip) return;

        UpdateTargetKeyCode();
        if (Input.GetKeyDown(targetKeyCode))
        {
            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            StartCoroutine(ClickAnimationRoutine());

            fillRoutine = StartCoroutine(FillRoutine(1, fillTime));
        }
        else if (Input.GetKeyUp(targetKeyCode))
        {
            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            fillRoutine = StartCoroutine(FillRoutine(0, fillTime / 2));
        }
    }

    private void UpdateTargetKeyCode()
    {
        foreach (var mapping in inputterSetting.buttonMappings)
        {
            if (mapping.buttonType == ButtonType.A)
            {
                targetKeyCode = mapping.buttonKeyCode;
                return;
            }
        }
    }

    private IEnumerator ClickAnimationRoutine()
    {
        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / animationTime;

            imageSkip.transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, animationCurve.Evaluate(percent));

            yield return null;
        }
    }

    private IEnumerator FillRoutine(float targetFill, float fillTime)
    {
        float startFill = imageSkip.fillAmount;

        float timer = 0, percent = 0;
        float realFillTime = fillTime * (1 - startFill);
        if (targetFill < startFill)
            realFillTime = fillTime * startFill;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / realFillTime;

            imageSkip.fillAmount = Mathf.Lerp(startFill, targetFill, percent);

            yield return null;
        }

        imageSkip.fillAmount = targetFill;

        if (targetFill == 1 && !skip)
        {
            skip = true;
            onSkip.Invoke();
        }
    }
}
