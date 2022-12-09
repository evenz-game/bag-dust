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

    [Header("Axis")]
    public UnityEvent<Vector2> onUpdatedAxis = new UnityEvent<Vector2>();
    [SerializeField]
    private string xAxisName;
    [SerializeField]
    private string yAxisName;
    [SerializeField]
    private Vector2 axisScale = Vector2.one;
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
            currentAxis = new Vector2(Input.GetAxisRaw(xAxisName) * axisScale.x, Input.GetAxisRaw(yAxisName) * axisScale.y);
        else
            currentAxis = new Vector2(Input.GetAxis(xAxisName) * axisScale.x, Input.GetAxis(yAxisName) * axisScale.y);

        onUpdatedAxis.Invoke(currentAxis);
    }

    private void UdpateButtons()
    {
        foreach (ButtonEventSet set in buttonEventSets)
            set.CheckButtonDown();
    }

    private void UpdateAnyButtonDown()
    {
        if (!checkAnyButtonDown) return;
        if (!Input.anyKeyDown) return;

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
        private KeyCode buttonKeyCode;
        [SerializeField]
        private UnityEvent<ButtonType> onButtonDown = new UnityEvent<ButtonType>();

        public void CheckButtonDown()
        {
            if (Input.GetKeyDown(buttonKeyCode))
                onButtonDown.Invoke(buttonType);
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
