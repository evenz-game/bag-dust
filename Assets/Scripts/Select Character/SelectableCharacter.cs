using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableCharacterState { None, Hover, Select }

public class SelectableCharacter : MonoBehaviour
{
    [SerializeField]
    private SelectableCharacter[] topCharacters;
    [SerializeField]
    private SelectableCharacter[] bottomCharacters;
    [SerializeField]
    private SelectableCharacter[] leftCharacters;
    [SerializeField]
    private SelectableCharacter[] rightCharacters;

    [SerializeField]
    private SelectableCharacterState state = SelectableCharacterState.None;
    public SelectableCharacterState State => state;

    public SelectableCharacter FindSelectableCharacterByAxis(Vector2 axis)
    {
        if (axis.x > 0)
        {
            foreach (SelectableCharacter character in rightCharacters)
                if (character.State == SelectableCharacterState.None)
                    return character;

        }
        else if (axis.x < 0)
        {
            foreach (SelectableCharacter character in leftCharacters)
                if (character.State == SelectableCharacterState.None)
                    return character;

        }
        else if (axis.y > 0)
        {
            foreach (SelectableCharacter character in topCharacters)
                if (character.State == SelectableCharacterState.None)
                    return character;

        }
        else if (axis.y < 0)
        {
            foreach (SelectableCharacter character in bottomCharacters)
                if (character.State == SelectableCharacterState.None)
                    return character;

        }

        return null;
    }

    public void UpdateState(SelectableCharacterState newState)
    {
        state = newState;
    }
}
