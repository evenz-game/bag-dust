using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : PlayerComponent, PlayerStatus.OnChangedPlayerState
{
    [SerializeField]
    private Transform playerTransfrom;

    [Header("Index")]
    [SerializeField]
    private Transform indexTransform;
    [SerializeField]
    private Vector3 addIndexPosition;
    [SerializeField]
    private TextMeshProUGUI textIndex;

    [Header("Icon")]
    [SerializeField]
    private Transform iconTransform;
    [SerializeField]
    private Vector3 addIconPosition;
    [SerializeField]
    private TextMeshProUGUI textLastChanceIcon;
    [SerializeField]
    private TextMeshProUGUI textDeadIcon;

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
        indexTransform.position = playerTransfrom.position + addIndexPosition;
    }

    private void UpdateIconPosition()
    {
        iconTransform.position = playerTransfrom.position + addIconPosition;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransfrom.position + addIconPosition, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerTransfrom.position + addIndexPosition, 0.1f);
    }
}
