using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputterSetting", menuName = "ScriptableObjects/InputterSetting", order = 1)]
public class InputterSetting : ScriptableObject
{
    [Header("Reference")]
    public InputterSetting reference;

    [ContextMenu("Sync")]
    private void Sync()
    {
        if (!reference) return;
        this.xAxisName = reference.xAxisName;
        this.yAxisName = reference.yAxisName;
        this.axisScale = reference.axisScale;
        this.buttonMappings = reference.buttonMappings;
    }

    [Space]
    [Header("Settings")]
    [SerializeField]
    private string xAxisName;
    public string XAxisName => reference != null ? reference.XAxisName : xAxisName;
    [SerializeField]
    private string yAxisName;
    public string YAxisName => reference != null ? reference.YAxisName : yAxisName;
    [SerializeField]
    private Vector2 axisScale = Vector2.one;
    public Vector2 AxisScale => reference != null ? reference.AxisScale : axisScale;

    [SerializeField]
    private ButtonMapping[] buttonMappings;
    public ButtonMapping[] ButtonMappings => reference != null ? reference.ButtonMappings : buttonMappings;

    [Serializable]
    public class ButtonMapping
    {
        public KeyCode buttonKeyCode;
        public ButtonType buttonType;
    }
}
