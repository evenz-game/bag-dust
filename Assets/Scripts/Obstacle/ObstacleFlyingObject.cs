using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleFlyingObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 flyingForce;
    [SerializeField]
    private float knockbackForceScale = 1;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = true;
        Invoke(nameof(AddForce), 1f);
    }

    protected virtual void AddForce()
    {
        rigidbody.isKinematic = false;
        // rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(flyingForce, ForceMode.Impulse);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement movement))
        {
            movement.Knockback(rigidbody.velocity * knockbackForceScale);

            if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
                status.IncreaseDustCount(-2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + flyingForce);
    }
}
