using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerComponent, PlayerStatus.OnChangedPlayerState, PlayerStatus.OnChangedWeight, PlayerModel.OnInitializedPlayerModel, Inputter.OnUpdatedAxis
{
    private PlayerModelInfo model;

    private new Rigidbody rigidbody;

    [SerializeField]
    private float knockbackScale;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnChangedWeight(float previousWeight, float currentWeight)
    {
        rigidbody.mass = currentWeight;
    }

    /* 회전 및 이동 */
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
        playerAudioPlayer?.Dash();
    }

    /* 넉백 */
    public void Knockback(Vector3 knockbackForce)
    {
        rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement otherPlayer))
            ClashOtherPlayer(otherPlayer);
    }

    private void ClashOtherPlayer(PlayerMovement otherPlayer)
    {
        if (rigidbody.velocity.sqrMagnitude > otherPlayer.rigidbody.velocity.sqrMagnitude)
        {
            otherPlayer.Knockback(rigidbody.velocity.normalized * playerStatus.CurrentWeight * knockbackScale);
            playerAudioPlayer?.ClashOtherPlayer();
        }
    }

    /* 플레이어 상태 변화 */
    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        if (currentPlayerState == PlayerState.Dead)
            Die();
    }

    /* 사망 시, 카메라 쪽으로 */
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

    /* 모델 초기화 */
    public void OnInitializedPlayerModel(PlayerModelInfo model)
    {
        this.model = model;
    }
}
