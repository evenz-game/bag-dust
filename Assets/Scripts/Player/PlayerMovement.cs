using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerComponent, Inputter.OnUpdatedAxis
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
}
