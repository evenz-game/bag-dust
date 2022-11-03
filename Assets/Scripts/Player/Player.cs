using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class Player : PlayerComponent
{
    protected override void Awake()
    {
        base.Awake();
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
