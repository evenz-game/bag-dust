using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour, PlayerStatus.OnChangedPlayerState
{
    [Header("Components")]
    [SerializeField]
    private Collider bodyCollider;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        switch (currentPlayerState)
        {
            case PlayerState.Dead:
                bodyCollider.enabled = false;
                rigidbody.mass = 0;
                rigidbody.drag = 0;
                break;
        }
    }
}
