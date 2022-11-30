using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected Player player;
    protected PlayerStatus playerStatus;
    protected PlayerAnimator playerAnimator;
    protected PlayerAudioPlayer playerAudioPlayer;
    protected PlayerEffector playerEffector;

    protected virtual void Awake()
    {
        player = GameObjectUtils.FindCompoenet<Player>(gameObject);
        playerStatus = GameObjectUtils.FindCompoenet<PlayerStatus>(gameObject);
        playerAnimator = GameObjectUtils.FindCompoenet<PlayerAnimator>(gameObject);
        playerAudioPlayer = GameObjectUtils.FindCompoenet<PlayerAudioPlayer>(gameObject);
        playerEffector = GameObjectUtils.FindCompoenet<PlayerEffector>(gameObject);
    }
}
