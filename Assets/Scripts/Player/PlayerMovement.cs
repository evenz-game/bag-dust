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

        Quaternion target = Quaternion.LookRotation(axis, transform.up);
        // target.eulerAngles = new Vector3(target.eulerAngles.x, 90, target.eulerAngles.z);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target,
            Time.deltaTime * playerStatus.RotateSpeed
        );
    }

    private Quaternion GetTargetRotationByAxis(Vector2 axis)
    {
        Quaternion result = new Quaternion();

        if (axis.x == 1 && axis.y == 0)
            result.eulerAngles = new Vector3(0, 0, 270);
        else if (axis.x == 1 && axis.y == 1)
            result.eulerAngles = new Vector3(0, 0, 315);
        else if (axis.x == 0 && axis.y == 1)
            result.eulerAngles = new Vector3(0, 0, 360);
        else if (axis.x == -1 && axis.y == 1)
            result.eulerAngles = new Vector3(0, 0, 45);
        else if (axis.x == -1 && axis.y == 0)
            result.eulerAngles = new Vector3(0, 0, 90);
        else if (axis.x == -1 && axis.y == -1)
            result.eulerAngles = new Vector3(0, 0, 135);
        else if (axis.x == 0 && axis.y == -1)
            result.eulerAngles = new Vector3(0, 0, 180);
        else if (axis.x == 1 && axis.y == -1)
            result.eulerAngles = new Vector3(0, 0, 225);

        return result;
    }

    public void Dash()
    {
        rigidbody.AddForce(transform.forward * playerStatus.DashPower, ForceMode.Impulse);
    }

    public void OnChangedWeight(float previousWeight, float currentWeight)
    {
        rigidbody.mass = currentWeight;
    }

    public void Knockback(Vector3 knockbackForce)
    {
        rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
    }
}
