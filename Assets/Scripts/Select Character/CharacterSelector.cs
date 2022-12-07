using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour, Inputter.OnUpdatedAxis, Inputter.OnButtonDown
{
    [SerializeField]
    private int playerIndex = 1;

    [SerializeField]
    public SelectableCharacter currentCharacter = null;

    [SerializeField]
    private bool movable = false;

    [Header("UI")]
    [SerializeField]
    private Transform uiIndexTransform;
    [SerializeField]
    private TextMeshProUGUI textIndex;

    private void Start()
    {
        textIndex.text = $"{playerIndex}";

        if (currentCharacter)
            UpdateCurrentCharacter(currentCharacter);
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
        print(newCharacter);
        if (!newCharacter) return;

        UpdateCurrentCharacter(newCharacter);

        movable = false;
    }

    private void UpdateCurrentCharacter(SelectableCharacter targetCharacter)
    {
        if (currentCharacter)
            currentCharacter.UpdateState(SelectableCharacterState.None);

        currentCharacter = targetCharacter;
        currentCharacter.UpdateState(SelectableCharacterState.Hover);

        uiIndexTransform.position = Camera.main.WorldToScreenPoint(currentCharacter.CharacterTransform.position);
    }

    public void OnButtonDown(ButtonType buttonType)
    {
        if (buttonType != ButtonType.A) return;

        if (!currentCharacter)
            UpdateCurrentCharacter(SelectCharacterController.GetUnhoveredSelectableCharacter());

        else
        {
            SelectableCharacterState state = currentCharacter.State;

            if (state == SelectableCharacterState.Hover)
            {
                currentCharacter.UpdateState(SelectableCharacterState.Select);
                textIndex.text = $"{playerIndex} (selected)";
            }
            else if (state == SelectableCharacterState.Select)
            {
                currentCharacter.UpdateState(SelectableCharacterState.Hover);
                textIndex.text = $"{playerIndex}";
            }
        }
    }
}
