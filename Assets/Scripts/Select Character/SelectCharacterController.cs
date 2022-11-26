using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterController : MonoBehaviour
{
    private static SelectCharacterController instance;

    [Header("Selectable Characters")]
    [SerializeField]
    private SelectableCharacter[] selectableCharacters;

    private void Awake()
    {
        instance = this;
    }

    public static SelectableCharacter GetUnhoveredSelectableCharacter()
    {
        foreach (SelectableCharacter character in instance.selectableCharacters)
            if (character.State == SelectableCharacterState.None)
                return character;

        return null;
    }
}
