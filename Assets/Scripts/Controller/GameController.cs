using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private List<PlayerStatus> activePlayers = new List<PlayerStatus>();

    [SerializeField]
    private GameObject canvasFinishGame;
    [SerializeField]
    private TextMeshProUGUI textWinnerPlayerIndex;

    private void Awake()
    {
        canvasFinishGame.SetActive(false);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 활성 상태 플레이어 검색
        // 활성화 상태를 기반으로 찾기 때문에 Awake에서 호출 금지
        PlayerStatus[] allPlayers = GameObject.FindObjectsOfType<PlayerStatus>();
        foreach (PlayerStatus player in allPlayers)
        {
            if (player.gameObject.activeSelf)
            {
                player.onChangedPlayerState.AddListener(CheckPlayerDeath);
                activePlayers.Add(player);
            }
        }
    }

    public void CheckPlayerDeath(PlayerState state)
    {
        if (state != PlayerState.Dead) return;

        for (int i = 0; i < activePlayers.Count; i++)
        {
            PlayerStatus player = activePlayers[i];

            if (player.CurrentPlayerState == PlayerState.Dead)
                activePlayers.Remove(player);
        }

        // 혼자 남았을 때
        if (activePlayers.Count == 1)
            FinishGame(activePlayers[0]);
    }

    private void FinishGame(PlayerStatus winner)
    {
        textWinnerPlayerIndex.text = winner.Index.ToString();
        canvasFinishGame.SetActive(true);
    }
}
