using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : PlayerComponent, PlayerStatus.OnChangedPlayerState
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
    private TextMeshProUGUI textLastChanceIcon;
    [SerializeField]
    private TextMeshProUGUI textDeadIcon;
    [SerializeField]
    private Vector3 addIconPosition;

    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
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
        if (textLastChanceIcon.IsActive())
            textLastChanceIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);

        if (textDeadIcon.IsActive())
            textDeadIcon.transform.position = WorldToScreenPoint(playerTransfrom.position + addIconPosition);
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        switch (currentPlayerState)
        {
            case PlayerState.Live:
                textLastChanceIcon.gameObject.SetActive(false);
                textDeadIcon.gameObject.SetActive(false);
                break;
            case PlayerState.LastChance:
                textLastChanceIcon.gameObject.SetActive(true);
                textDeadIcon.gameObject.SetActive(false);
                break;
            case PlayerState.Dead:
                textLastChanceIcon.gameObject.SetActive(false);
                textDeadIcon.gameObject.SetActive(true);
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
