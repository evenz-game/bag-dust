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
        if (currentPlayerState == PlayerState.Dead)
            Die();
    }

    private void Die()
    {
        bodyCollider.enabled = false;
        rigidbody.mass = 0;
        rigidbody.drag = 0;

        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.Normalize();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.velocity = dir * 400;
    }
}
