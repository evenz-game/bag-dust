using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeaderUI : PlayerUI, PlayerStatus.OnChangedPlayerState
{
    [SerializeField]
    private Transform playerTransfrom;

    [Header("Index")]
    [SerializeField]
    private RectTransform indexTransform;
    [SerializeField]
    private TextMeshProUGUI textIndex;
    [SerializeField]
    private Vector3 addIndexPosition;

    [Header("Icon")]
    [SerializeField]
    private GameObject LastChanceIcon;
    [SerializeField]
    private GameObject DeadIcon;
    [SerializeField]
    private Vector3 addIconPosition;

    private Camera mainCamera;

    private void Awake()
    {
        playerStatus.onChangedPlayerState.AddListener(OnChangedPlayerState);
    }

    private void Start()
    {
        textIndex.text = playerStatus.Index.ToString();
    }

    private void Update()
    {
        UpdateIndexPosition();
        UpdateIconPosition();
    }

    private void UpdateIndexPosition()
    {
        indexTransform.position = WorldToScreenPoint(playerTransfrom.position + addIndexPosition);
    }

    private void UpdateIconPosition()
    {
        if (LastChanceIcon.activeSelf)
            LastChanceIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);

        if (DeadIcon.activeSelf)
            DeadIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);
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
    }
}
