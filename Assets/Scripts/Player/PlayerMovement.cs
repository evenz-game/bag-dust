using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerComponent, PlayerStatus.OnChangedWeight, Inputter.OnUpdatedAxis
{
    private new Rigidbody rigidbody;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnUpdatedAxis(Vector2 axis)
    {
        if (axis == Vector2.zero) return;

        rigidbody.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(axis),
            Time.deltaTime * playerStatus.RotateSpeed
        );
    }

    public void Dash()
    {
        rigidbody.AddForce(transform.forward * playerStatus.DashPower, ForceMode.Impulse);
    }

    public void OnChangedWeight(float previousWeight, float currentWeight)
    {
        rigidbody.mass = currentWeight;
    }
}
