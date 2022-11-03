using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameController : MonoBehaviour, GameController.OnStartedGame
{
    private List<PlayerStatus> activePlayers = new List<PlayerStatus>();

    private enum GameStatus { None, Start, Finish }
    private GameStatus gameStatus = GameStatus.None;

    [Header("Game Start")]
    public UnityEvent onStartedGame = new UnityEvent();

    [Header("Game Finish")]
    public UnityEvent onFinishedGame = new UnityEvent();
    [SerializeField]
    private GameObject panelFinishGame;
    [SerializeField]
    private TextMeshProUGUI textWinnerPlayerIndex;

    [Header("Timer")]
    [SerializeField]
    private float gamePlayTime = 60;
    [SerializeField]
    private TextMeshProUGUI textTimer;
    [SerializeField]
    private UnityEvent onFinishedTimer = new UnityEvent();

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

        panelFinishGame.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        if (gameStatus == GameStatus.Start || gameStatus == GameStatus.Finish) return;

        gameStatus = GameStatus.Start;

        GameController.OnStartedGame[] onStartedGames = FindObjectsOfType<MonoBehaviour>().OfType<GameController.OnStartedGame>().ToArray();
        foreach (var e in onStartedGames)
            e.OnStartedGame();

        onStartedGame.Invoke();
    }

    public void FinishGame(PlayerStatus winner)
    {
        if (gameStatus == GameStatus.Finish) return;

        gameStatus = GameStatus.Finish;

        textWinnerPlayerIndex.text = winner.Index.ToString();
        panelFinishGame.SetActive(true);

        GameController.OnFinishedGame[] onStartedGames = FindObjectsOfType<MonoBehaviour>().OfType<GameController.OnFinishedGame>().ToArray();
        foreach (var e in onStartedGames)
            e.OnFinishedGame();

        onFinishedGame.Invoke();
    }

    private void FindActivePlayers()
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

    private PlayerStatus FindWinnerByDustCount()
    {
        PlayerStatus winner = null;

        for (int i = 0; i < activePlayers.Count; i++)
        {
            PlayerStatus player = activePlayers[i];

            if (player.CurrentPlayerState == PlayerState.Dead)
                continue;

            if (!winner)
                winner = player;
            else if (player.CurrentDustCount >= winner.CurrentDustCount)
                winner = player;
        }

        return winner;
    }

    private IEnumerator TimerRoutine()
    {
        float timer = gamePlayTime;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0) break;

            textTimer.text = ((int)timer).ToString();

            yield return null;
        }

        // 타이머 종료 이벤트 호출
        onFinishedTimer.Invoke();

        // 승리한 플레이어 있으면 게임 종료
        PlayerStatus winner = FindWinnerByDustCount();
        if (winner)
            FinishGame(winner);
    }

    void OnStartedGame.OnStartedGame()
    {
        FindActivePlayers();
        StartCoroutine(TimerRoutine());
    }

    public interface OnStartedGame
    {
        public void OnStartedGame();
    }

    public interface OnFinishedGame
    {
        public void OnFinishedGame();
    }
}
