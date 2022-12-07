using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterController : MonoBehaviour
{
    private static SelectCharacterController instance;

    [Header("Selectable Characters")]
    [SerializeField]
    private SelectableCharacterRow[] selectableCharacterRows;

    private void Awake() => instance = this;

    public static SelectableCharacter GetUnhoveredSelectableCharacter()
    {
        foreach (SelectableCharacterRow row in instance.selectableCharacterRows)
            foreach (SelectableCharacter character in row.selectableCharacters)
                if (character.State == SelectableCharacterState.None)
                    return character;

        return null;
    }

    public static SelectableCharacter FindUnhoveredSelectableCharacterByAxis(SelectableCharacter currentSelectableCharacter, Vector2 axis)
    {
        // 현재 인덱스 찾기
        int[] indexs = instance.FindIndexsBySelectableCharacter(currentSelectableCharacter);
        return instance.FindUnhoveredSelectableCharacterByAxis(indexs, axis);
    }

    private int[] FindIndexsBySelectableCharacter(SelectableCharacter targetSelectableCharacter)
    {
        SelectableCharacterRow[] rows = selectableCharacterRows;

        for (int r = 0; r < rows.Length; r++)
        {
            SelectableCharacterRow row = rows[r];
            SelectableCharacter[] characters = row.selectableCharacters;

            for (int c = 0; c < characters.Length; c++)
            {
                SelectableCharacter character = characters[c];
                if (targetSelectableCharacter == character)
                {
                    return new int[] { r, c };
                }
            }
        }

        return new int[] { -1, -1 };
    }

    private SelectableCharacter FindUnhoveredSelectableCharacterByAxis(int[] currentIndexs, Vector2 axis)
    {
        int rowIdx = currentIndexs[0];
        int colIdx = currentIndexs[1];

        // 좌우 이동
        if (axis.x != 0)
        {
            SelectableCharacterRow currentRow = selectableCharacterRows[rowIdx];

            int newColIdx = NextColumnIndex(currentRow, colIdx, axis.x);
            while (newColIdx != colIdx)
            {
                SelectableCharacter character = currentRow.selectableCharacters[newColIdx];
                if (character.State != SelectableCharacterState.None)
                {
                    newColIdx = NextColumnIndex(currentRow, newColIdx, axis.x);
                    continue;
                }

                return character;
            }
        }

        // 상하 이동
        else if (axis.y != 0)
        {
            int newRowIdx = NextRowIndex(rowIdx, -axis.y);
            while (newRowIdx != rowIdx)
            {
                // 해당 열을 한 칸씩 오른쪽으로 이동하면서 선택되지 않은 캐릭터를 탐색한다.
                SelectableCharacterRow row = selectableCharacterRows[newRowIdx];
                int startColIdx = NextColumnIndex(row, colIdx, 0);
                int newColIdx = startColIdx;
                startColIdx = NextColumnIndex(row, startColIdx, -1);

                while (newColIdx != startColIdx)
                {
                    SelectableCharacter character = row.selectableCharacters[newColIdx];
                    if (character.State != SelectableCharacterState.None)
                    {
                        newColIdx = NextColumnIndex(row, newColIdx, 1);
                        continue;
                    }

                    return character;
                }

                newRowIdx = NextRowIndex(newRowIdx, -axis.y);
            }

        }

        return null;
    }

    private int NextRowIndex(int currentRowIndex, float direction)
    {
        int arrLength = selectableCharacterRows.Length;

        int targetIdx = currentRowIndex;

        if (direction != 0)
            targetIdx += (int)Mathf.Sign(direction);

        if (targetIdx < 0) targetIdx = arrLength - 1;
        if (targetIdx >= arrLength) targetIdx = 0;

        return targetIdx;
    }

    private int NextColumnIndex(SelectableCharacterRow row, int currentColumnIndex, float direction)
    {
        int rowLength = row.selectableCharacters.Length;

        int targetIdx = currentColumnIndex;

        if (direction != 0)
            targetIdx += (int)Mathf.Sign(direction);

        if (targetIdx < 0) targetIdx = rowLength - 1;
        if (targetIdx >= rowLength) targetIdx = 0;

        return targetIdx;
    }

    [Serializable]
    private class SelectableCharacterRow
    {
        public SelectableCharacter[] selectableCharacters;
    }
}
