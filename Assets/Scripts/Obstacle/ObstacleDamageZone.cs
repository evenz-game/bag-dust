using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageZone : Obstacle
{
    [Header("Knockback")]
    [SerializeField]
    private float knockbackPower;
    [SerializeField]
    private int decreaseDustCount = 4;

    private Animator animator;

    private void Awake()
    {
        animator = GameObjectUtils.FindCompoenet<Animator>(this.gameObject);
    }

    public void Spawn()
    {
        animator?.Play("Spawn", -1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement movement))
        {
            movement.Knockback(transform.up * knockbackPower);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement movement))
        {
            movement.Knockback(transform.up * knockbackPower);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
        {
            if (decreaseDustCount <= 0) return;

            status.ClashObstacle(decreaseDustCount);
            CameraWalkingController.Shake(0.3f, 0.2f);
        }
    }
}
