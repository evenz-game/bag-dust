using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour, Inputter.OnUpdatedAxis, Inputter.OnButtonDown
{
    [SerializeField]
    private int playerIndex = 1;

    [SerializeField]
    public SelectableCharacter currentCharacter = null;

    [SerializeField]
    private bool movable = false;

    private void Start()
    {
        if (currentCharacter)
            currentCharacter.UpdateState(SelectableCharacterState.Hover);
    }

    public void OnUpdatedAxis(Vector2 axis)
    {
        if (!movable)
        {
            movable = (axis.sqrMagnitude == 0);
            return;
        }
        else
            if (axis == Vector2.zero) return;

        if (!currentCharacter) return;
        if (currentCharacter.State == SelectableCharacterState.Select) return;

        SelectableCharacter newCharacter = currentCharacter.FindSelectableCharacterByAxis(axis);
        if (!newCharacter) return;

        currentCharacter.UpdateState(SelectableCharacterState.None);
        currentCharacter = newCharacter;
        currentCharacter.UpdateState(SelectableCharacterState.Hover);

        movable = false;
    }

    public void OnButtonDown(ButtonType buttonType)
    {
        if (buttonType != ButtonType.A) return;

        if (!currentCharacter)
            currentCharacter = SelectCharacterController.GetUnhoveredSelectableCharacter();

        else
        {
            SelectableCharacterState state = currentCharacter.State;

            if (state == SelectableCharacterState.Hover)
                currentCharacter.UpdateState(SelectableCharacterState.Select);
            else if (state == SelectableCharacterState.Select)
                currentCharacter.UpdateState(SelectableCharacterState.Hover);
        }
    }
}
