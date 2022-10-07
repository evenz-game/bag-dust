using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class Player : MonoBehaviour
{
    private PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        // @테스트 먼지 획득
        if (Input.GetKeyDown(KeyCode.Equals))
            playerStatus.IncreaseDustCount(1);
        else if (Input.GetKeyDown(KeyCode.Minus))
            playerStatus.IncreaseDustCount(-1);
    }
}
