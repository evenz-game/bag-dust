using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour, PlayerStatus.OnChangedPlayerState
{
    [SerializeField]
    private Transform playerTransfrom;
    [Header("Icon")]
    [SerializeField]
    private Transform iconTransform;
    [SerializeField]
    private Vector3 addIconPosition;
    [SerializeField]
    private TextMeshProUGUI textLastChanceIcon;
    [SerializeField]
    private TextMeshProUGUI textDeadIcon;

    private void Update()
    {
        UpdateIconPosition();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransfrom.position + addIconPosition, 0.1f);
    }
}
