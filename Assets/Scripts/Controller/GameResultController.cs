using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultController : MonoBehaviour
{
    [SerializeField]
    private PlayerStatus[] players;

    [SerializeField]
    private Transform winnerPositionTransform;

    [SerializeField]
    private List<Transform> loserPositionTransforms;

    private int winnerIndex = -1;

    private void Awake()
    {
        winnerIndex = GetWinnerIndex();
        if (winnerIndex == -1)
        {
            Debug.LogError("GameResultController: winnerIndex가 없습니다.");
            return;
        }

        SetPlayerActive();
        SetPlayerInputterActive();
        SetPlayerPosition();
        Invoke(nameof(PlayWinnerAnimation), 1);
    }

    private int GetWinnerIndex()
    {
        int winnerIndex = MyPlayerPrefs.GetWinnerIndex();
        return winnerIndex;
    }

    private void SetPlayerActive()
    {
        foreach (PlayerStatus player in players)
        {
            bool isActive = MyPlayerPrefs.GetPlayerActive(player.Index);
            player.transform.root.gameObject.SetActive(isActive);
        }
    }

    private void SetPlayerInputterActive()
    {
        foreach (PlayerStatus player in players)
            if (player.Index != winnerIndex)
                foreach (Inputter inputter in player.GetComponents<Inputter>())
                    inputter.Init();
    }

    private void SetPlayerPosition()
    {
        foreach (PlayerStatus player in players)
        {
            if (player.Index == winnerIndex)
                player.transform.position = winnerPositionTransform.position;
            else
            {
                if (loserPositionTransforms.Count == 0) continue;

                player.transform.position = loserPositionTransforms[0].position;
                loserPositionTransforms.RemoveAt(0);
            }
        }
    }

    private void PlayWinnerAnimation()
    {
        foreach (PlayerStatus player in players)
            if (player.Index == winnerIndex)
                player.GetComponent<PlayerAnimator>()?.Win();
    }

    private void OnDrawGizmos()
    {
        if (winnerPositionTransform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(winnerPositionTransform.position, Vector3.one);
        }

        Gizmos.color = Color.red;
        foreach (Transform loserPos in loserPositionTransforms)
            Gizmos.DrawCube(loserPos.position, Vector3.one);
    }
}
