using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeaderUI : PlayerUI, PlayerStatus.OnChangedPlayerState
{
    [Header("Transform")]
    [SerializeField]
    private Transform playerTransfrom;

    [Header("Index")]
    [SerializeField]
    private Image imageIndex;
    [SerializeField]
    private Sprite[] indexSprites;
    [SerializeField]
    private Vector3 addIndexPosition;

    [Header("Icon")]
    [SerializeField]
    private GameObject LastChanceIcon;
    [SerializeField]
    private GameObject DeadIcon;
    [SerializeField]
    private Vector3 addIconPosition;

    [Header("Dash Timer")]
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private Image imageDashTimer;
    [SerializeField]
    private Vector3 addDashTimerPosition;

    [Header("Ghost Kick Counter")]
    [SerializeField]
    private PlayerGhost playerGhost;
    [SerializeField]
    private TextMeshProUGUI textGhostKickCounter;
    [SerializeField]
    private Vector3 addGhostKickCounterPosition;

    private Camera mainCamera;

    private void Awake()
    {
        playerStatus.onChangedPlayerState.AddListener(OnChangedPlayerState);
        playerMovement.onDash.AddListener(OnDash);
        playerGhost.onChangedLeftKickCount.AddListener(UpdateGhostKickCounter);

        playerGhost.onChangedParent.AddListener((PlayerGhost parent) =>
            imageDashTimer.gameObject.SetActive(parent == null));
    }

    private void Start()
    {
        imageIndex.sprite = indexSprites[playerStatus.Index];
    }

    private void Update()
    {
        UpdateIndexPosition();
        UpdateIconPosition();
        UpdateDashTimerPosition();
        UpdateGhostKickCounterPosition();
    }

    private void UpdateIndexPosition()
    {
        imageIndex.transform.position = WorldToScreenPoint(playerTransfrom.position + addIndexPosition);
    }

    private void UpdateIconPosition()
    {
        if (LastChanceIcon.activeSelf)
            LastChanceIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);

        if (DeadIcon.activeSelf)
            DeadIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);
    }

    private void UpdateDashTimerPosition()
    {
        if (imageDashTimer.gameObject.activeSelf)
            imageDashTimer.transform.position = WorldToScreenPoint(playerTransfrom.position + addDashTimerPosition);
    }

    private void OnDash(float dashDelayTime)
    {
        StartCoroutine(DashTimerRoutine(dashDelayTime));
    }

    private IEnumerator DashTimerRoutine(float time)
    {
        float timer = 0, percent = 0;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / time;

            imageDashTimer.fillAmount = percent;

            yield return null;
        }

        imageDashTimer.fillAmount = 1;
    }

    private void UpdateGhostKickCounterPosition()
    {
        if (textGhostKickCounter.gameObject.activeSelf)
            textGhostKickCounter.transform.position = WorldToScreenPoint(playerTransfrom.position + addGhostKickCounterPosition);
    }

    private void UpdateGhostKickCounter(int leftKickCount)
    {
        textGhostKickCounter.text = leftKickCount > 0 ? leftKickCount.ToString() : "";
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        switch (currentPlayerState)
        {
            case PlayerState.Live:
                LastChanceIcon.gameObject.SetActive(false);
                DeadIcon.gameObject.SetActive(false);
                break;
            case PlayerState.LastChance:
                LastChanceIcon.gameObject.SetActive(true);
                DeadIcon.gameObject.SetActive(false);
                break;
            case PlayerState.Dead:
                LastChanceIcon.gameObject.SetActive(false);
                DeadIcon.gameObject.SetActive(true);
                break;
            case PlayerState.Ghost:
                LastChanceIcon.gameObject.SetActive(false);
                DeadIcon.gameObject.SetActive(false);
                imageDashTimer.gameObject.SetActive(true);
                textGhostKickCounter.gameObject.SetActive(true);
                break;
        }
    }

    private Vector3 WorldToScreenPoint(Vector3 worldPosition)
    {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransfrom.position + addIconPosition, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerTransfrom.position + addIndexPosition, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerTransfrom.position + addDashTimerPosition, 0.1f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(playerTransfrom.position + addGhostKickCounterPosition, 0.1f);
    }
}
