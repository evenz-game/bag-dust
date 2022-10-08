using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected Player player;
    protected PlayerStatus playerStatus;

    protected virtual void Awake()
    {
        player = GameObjectUtils.FindCompoenet<Player>(gameObject);
        playerStatus = GameObjectUtils.FindCompoenet<PlayerStatus>(gameObject);
    }
}
