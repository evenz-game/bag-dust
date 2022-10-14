using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageZone : Obstacle
{
    [Header("Knockback")]
    [SerializeField]
    private float knockbackPower;

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
            status.IncreaseDustCount(-4);
        }
    }
}