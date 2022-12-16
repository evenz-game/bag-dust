using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private SettingButton[] settingButtons;

    [Space]
    [SerializeField]
    private GameObject canvasSetting;
    [SerializeField]
    private bool isActive = false;
    private bool isDone = false;
    public UnityEvent onActive = new UnityEvent();
    public UnityEvent onDeactive = new UnityEvent();

    [Space]
    [SerializeField]
    private bool verticalMove = true;
    [SerializeField]
    private float timeScaleAtActive = 0.0001f;

    [Header("Debug")]
    [SerializeField]
    private int currentIndex = 0;

    private void Awake()
    {
        FindSettingButtons();
        HoverButton(settingButtons[currentIndex]);
        Active(isActive);
    }

    private void FindSettingButtons()
    {
        settingButtons = GetComponentsInChildren<SettingButton>();
    }

    private bool movable = false;
    public void OnUpdatedAxis(Vector2 axis)
    {
        if (!isActive || isDone) return;

        /* 클릭 형식으로 인식할 수 있도록 계산 */
        if (!movable)
        {
            movable = (axis.sqrMagnitude == 0);
            return;
        }
        else
            if (axis == Vector2.zero) return;

        /* On Key Down */
        int dir = -(int)Mathf.Sign(axis.y);
        if (!verticalMove)
            dir = (int)Mathf.Sign(axis.x);

        int prevIndex = currentIndex;

        currentIndex += dir;
        currentIndex = Mathf.Clamp(currentIndex, 0, settingButtons.Length - 1);

        if (prevIndex == currentIndex)
        {
            movable = false;
            return;
        }

        SettingButton curButton = settingButtons[currentIndex];
        HoverButton(curButton);

        movable = false;
    }

    private void HoverButton(SettingButton targetButton)
    {
        foreach (var btn in settingButtons)
        {
            btn.Hover(btn == targetButton);
        }
    }

    public void Select()
    {
        if (!isActive || isDone) return;

        SettingButton curButton = settingButtons[currentIndex];
        curButton.Select();

        curButton.onSelected.AddListener(() =>
        {
            if (curButton.IsSelectOnce)
            {
                isDone = true;
                Active(false);
            }
        });
    }

    public void Active()
    {
        Active(!isActive);
    }

    public void Active(bool value)
    {
        if (value && isDone) return;

        isActive = value;
        Time.timeScale = value ? timeScaleAtActive : 1;

        if (isActive)
            onActive.Invoke();
        else
            onDeactive.Invoke();

        canvasSetting?.SetActive(value);
    }
}
