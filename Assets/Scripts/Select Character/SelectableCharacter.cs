using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableCharacterState { None, Hover, Select }

public class SelectableCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform characterTransform;
    public Transform CharacterTransform => characterTransform;

    [SerializeField]
    private SelectableCharacterState state = SelectableCharacterState.None;
    public SelectableCharacterState State => state;

    public SelectableCharacter FindSelectableCharacterByAxis(Vector2 axis)
        => SelectCharacterController.FindUnhoveredSelectableCharacterByAxis(this, axis);

    public void UpdateState(SelectableCharacterState newState)
    {
        state = newState;
    }
}
