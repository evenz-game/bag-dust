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
    private SelectableCharacter currentCharacter = null;
    public SelectableCharacter CurrentCharacter => currentCharacter;

    private bool freezeButtons = false;
    [SerializeField]
    private bool movable = false;

    [Header("UI")]
    [SerializeField]
    private Transform uiIndexTransform;
    [SerializeField]
    private Image imageIndex;
    [SerializeField]
    private Sprite[] indexSprites;
    [SerializeField]
    private Sprite selectedSprite;

    private void Start()
    {
        MyPlayerPrefs.SetPlayerActive(playerIndex, false);

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

        if (freezeButtons) return;

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

        MyPlayerPrefs.SetPlayerActive(playerIndex, true);
        MyPlayerPrefs.SetPlayerModelIndex(playerIndex, currentCharacter.ModelIndex);

        uiIndexTransform.gameObject.SetActive(true);
        uiIndexTransform.position = Camera.main.WorldToScreenPoint(currentCharacter.CharacterTransform.position);
        imageIndex.sprite = indexSprites[playerIndex - 1];
    }

    public void OnButtonDown(ButtonType buttonType)
    {
        if (freezeButtons) return;

        if (buttonType != ButtonType.A) return;

        if (!currentCharacter)
            UpdateCurrentCharacter(SelectCharacterController.GetUnhoveredSelectableCharacter());

        else
        {
            SelectableCharacterState state = currentCharacter.State;

            if (state == SelectableCharacterState.Hover)
            {
                currentCharacter.UpdateState(SelectableCharacterState.Select);
                imageIndex.sprite = selectedSprite;
            }
            else if (state == SelectableCharacterState.Select)
            {
                currentCharacter.UpdateState(SelectableCharacterState.Hover);
                imageIndex.sprite = indexSprites[playerIndex - 1];
            }
        }
    }

    public void FreezeButtons(bool value = true)
    {
        freezeButtons = value;
    }
}
