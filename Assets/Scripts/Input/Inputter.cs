using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonType { A, B, Start, Pause }

public class Inputter : MonoBehaviour, CameraController.OnFinishedChangeFov
{
    [SerializeField]
    private bool init = false;

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

    private void Update()
    {
        if (!init) return;

        UpdateAxis();
        UdpateButtons();
    }

    private void UpdateAxis()
    {
        currentAxis = new Vector2(Input.GetAxis(xAxisName) * axisScale.x, Input.GetAxis(yAxisName) * axisScale.y);
        onUpdatedAxis.Invoke(currentAxis);
    }

    private void UdpateButtons()
    {
        foreach (ButtonEventSet set in buttonEventSets)
            set.CheckButtonDown();
    }

    public void OnFinishedChangeFov()
    {
        Init();
    }

    public void Init()
    {
        init = true;
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
