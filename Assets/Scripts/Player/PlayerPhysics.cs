using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : PlayerComponent, PlayerStatus.OnChangedPlayerState, PlayerModel.OnInitializedPlayerModel
{
    private PlayerModelInfo model;

    private new Rigidbody rigidbody;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        if (currentPlayerState == PlayerState.Dead)
            Die();
    }

    private void Die()
    {
        model.BodyCollider.enabled = false;
        rigidbody.mass = 0;
        rigidbody.drag = 0;

        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.Normalize();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.velocity = dir * 400;
    }

    public void OnInitializedPlayerModel(PlayerModelInfo model)
    {
        this.model = model;
    }
}
