using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputterSetting", menuName = "ScriptableObjects/InputterSetting", order = 1)]
public class InputterSetting : ScriptableObject
{
    public string xAxisName;
    public string yAxisName;
    public Vector2 axisScale = Vector2.one;

    public ButtonMapping[] buttonMappings;

    [Serializable]
    public class ButtonMapping
    {
        public KeyCode buttonKeyCode;
        public ButtonType buttonType;
    }
}
