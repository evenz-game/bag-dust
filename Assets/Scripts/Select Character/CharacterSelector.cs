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

    [SerializeField]
    private float moveTime;
    [SerializeField]
    private AnimationCurve moveCurve;
    [SerializeField]
    private float minScale;
    [SerializeField]
    private AnimationCurve scaleCurve;

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
        imageIndex.sprite = indexSprites[playerIndex - 1];

        Vector3 targetUIPos = Camera.main.WorldToScreenPoint(currentCharacter.CharacterTransform.position);
        StopAllCoroutines();
        StartCoroutine(MoveUIIndexRoutine(targetUIPos));
    }

    private IEnumerator MoveUIIndexRoutine(Vector2 targetPosition)
    {
        float tiemr = 0, percent = 0;
        Vector2 startPos = uiIndexTransform.position;

        while (percent < 1)
        {
            tiemr += Time.deltaTime;
            percent = tiemr / moveTime;

            uiIndexTransform.position = Vector3.Lerp(startPos, targetPosition, moveCurve.Evaluate(percent));
            uiIndexTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * minScale, scaleCurve.Evaluate(percent));

            yield return null;
        }

        uiIndexTransform.position = targetPosition;
        uiIndexTransform.localScale = Vector3.one;
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
