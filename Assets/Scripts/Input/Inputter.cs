using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonType { A, B, Start, Pause }

public class Inputter : MonoBehaviour, GameController.InitializeInputterEvent
{
    [SerializeField]
    private bool init = false;
    [SerializeField]
    private bool useAxis = true;
    [SerializeField]
    private bool useAxisRaw = false;

    [Header("Inputter Setting")]
    [SerializeField]
    private InputterSetting inputterSetting;

    [Header("Axis")]
    public UnityEvent<Vector2> onUpdatedAxis = new UnityEvent<Vector2>();
    private Vector2 currentAxis;
    public Vector2 CurrentAxis => currentAxis;

    [Header("Buttons")]
    [SerializeField]
    private List<ButtonEventSet> buttonEventSets;

    [Header("Any Button")]
    [SerializeField]
    private bool checkAnyButtonDown = false;
    public UnityEvent onDownAnyButton = new UnityEvent();

    private void Update()
    {
        if (!init) return;

        UpdateAxis();
        UdpateButtons();
        UpdateAnyButtonDown();
    }

    private void UpdateAxis()
    {
        if (!useAxis) return;

        if (useAxisRaw)
            currentAxis = new Vector2(
                Input.GetAxisRaw(inputterSetting.xAxisName) * inputterSetting.axisScale.x,
                Input.GetAxisRaw(inputterSetting.yAxisName) * inputterSetting.axisScale.y
            );
        else
            currentAxis = new Vector2(
                Input.GetAxis(inputterSetting.xAxisName) * inputterSetting.axisScale.x,
                Input.GetAxis(inputterSetting.yAxisName) * inputterSetting.axisScale.y
            );

        onUpdatedAxis.Invoke(currentAxis);
    }

    private void UdpateButtons()
    {
        foreach (ButtonEventSet set in buttonEventSets)
            foreach (InputterSetting.ButtonMapping buttonMapping in inputterSetting.buttonMappings)
                if (set.CheckButtonDown(buttonMapping)) break;
    }

    private void UpdateAnyButtonDown()
    {
        if (!checkAnyButtonDown) return;
        if (!Input.anyKeyDown) return;
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3))) return;

        onDownAnyButton.Invoke();
    }

    public void Init()
    {
        init = true;
    }

    public void InitializeInputter()
    {
        Init();
    }

    [Serializable]
    private class ButtonEventSet
    {
        [SerializeField]
        private ButtonType buttonType;
        [SerializeField]
        private UnityEvent<ButtonType> onButtonDown = new UnityEvent<ButtonType>();

        public bool CheckButtonDown(InputterSetting.ButtonMapping buttonMapping)
        {
            if (buttonMapping.buttonType != this.buttonType) return false;

            if (Input.GetKeyDown(buttonMapping.buttonKeyCode))
            {
                onButtonDown.Invoke(buttonType);
                return true;
            }

            return false;
        }
    }

    public interface OnUpdatedAxis
    {
        public void OnUpdatedAxis(Vector2 axis);
    }

    public interface OnButtonDown
    {
        public void OnButtonDown(ButtonType buttonType);
    }
}
