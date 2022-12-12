using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField]
    private Image imageButton;

    [Header("Hover")]
    [SerializeField]
    protected float hoverAnimationTime;
    [SerializeField]
    protected AnimationCurve hoverAnimationCurve;
    [SerializeField]
    protected float minHoverScale;
    [SerializeField]
    protected float maxHoverScale;
    protected Coroutine hoverCoroutine;

    [Header("Select")]
    [SerializeField]
    protected bool isSelectOnce = false;
    protected bool isSelectable = true;
    public bool IsSelectOnce => isSelectOnce;
    [SerializeField]
    public UnityEvent onSelected = new UnityEvent();
    [SerializeField]
    protected float selectAnimationTime;
    [SerializeField]
    protected AnimationCurve selectAnimationCurve;
    [SerializeField]
    protected float minSelectScale;
    [SerializeField]
    protected float maxSelectScale;
    protected Coroutine selectCoroutine;

    protected bool isHover = false;
    public void Hover(bool value)
    {
        if (isHover == value) return;

        isHover = value;

        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        float targetScale = isHover ? maxHoverScale : minHoverScale;
        float hoverAnimationTime = isHover ? this.hoverAnimationTime : this.hoverAnimationTime / 2;
        hoverCoroutine
            = StartCoroutine(HoverAnimationRoutine(imageButton.transform.localScale.x, targetScale, hoverAnimationTime));
    }

    private IEnumerator HoverAnimationRoutine(float startScale, float targetScale, float hoverAnimationTime)
    {
        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / hoverAnimationTime;

            imageButton.transform.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, targetScale, hoverAnimationCurve.Evaluate(percent));

            yield return null;
        }
    }

    public virtual void Select()
    {
        if (!isSelectable) return;

        if (selectCoroutine != null)
            StopCoroutine(selectCoroutine);

        selectCoroutine = StartCoroutine(SelectAnimationRoutine());

        if (isSelectOnce)
            isSelectable = false;
    }

    private IEnumerator SelectAnimationRoutine()
    {
        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / hoverAnimationTime;

            imageButton.transform.localScale = Vector3.one * Mathf.LerpUnclamped(minSelectScale, maxSelectScale, selectAnimationCurve.Evaluate(percent));

            yield return null;
        }

        OnSelect();
    }

    protected virtual void OnSelect()
    {
        onSelected.Invoke();
    }
}
