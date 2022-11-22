using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : PlayerComponent, PlayerStatus.OnChangedPlayerState, Inputter.OnButtonDown
{
    private bool isGhost => playerStatus.CurrentPlayerState == PlayerState.Ghost;

    private new Rigidbody rigidbody;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(this.gameObject);

        originParentTransform = transform.parent;
    }

    /* ------ 부모 ------ */
    private List<PlayerGhost> ghostChildren = new List<PlayerGhost>();

    private void AddChildren(PlayerGhost children)
    {
        if (ghostChildren.Contains(children)) return;

        ghostChildren.Add(children);

        children.transform.parent = this.transform;
    }

    private void RemoveChildren(PlayerGhost children)
    {
        if (!ghostChildren.Contains(children)) return;

        ghostChildren.Remove(children);

        children.Kick();

        if (GameObjectUtils.FindCompoenet<PlayerMovement>(children.gameObject, out PlayerMovement childMovement))
        {
            Vector3 dir = children.transform.position - transform.position;
            childMovement.Knockback(dir * 15);
        }
    }

    public void OnButtonDown(ButtonType buttonType)
    {
        // 부모가 A버튼 누르면
        if (!isGhost && buttonType == ButtonType.A)
            KickChildren();
    }

    /// <summary>
    /// 자식 유령을 차는(kick) 함수
    /// </summary>
    private void KickChildren()
    {
        foreach (PlayerGhost child in ghostChildren)
        {
            child.leftKickCount--;
            if (child.leftKickCount == 0)
            {
                RemoveChildren(child);
                playerStatus.AddWeight(-ghostWeight);
            }
        }
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        if (currentPlayerState == PlayerState.Dead)
            KickAllChildren();
    }

    private void KickAllChildren()
    {
        foreach (PlayerGhost child in ghostChildren)
        {
            RemoveChildren(child);
            playerStatus.AddWeight(-ghostWeight);
        }
    }

    /* ------ 자식 ------ */
    private Transform originParentTransform;

    [SerializeField]
    private float ghostWeight;

    [SerializeField]
    private int kickCount = 15;
    public int leftKickCount = 0;

    private float lastKickTime = 0;

    private bool hasParent = false;

    private void OnTriggerStay(Collider other)
    {

        // 내가 유령일 때, 다른 플레이어와 부딪히면
        if (isGhost && !hasParent && GameObjectUtils.FindCompoenet<PlayerGhost>(other.gameObject, out PlayerGhost parent))
        {
            if (Time.time - lastKickTime < 3) return;

            // 부모가 유령이 아닐 때
            // 나를 자식으로 넣고
            if (!parent.isGhost)
            {

                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
                parent.AddChildren(this);
                parent.playerStatus.AddWeight(ghostWeight);
                leftKickCount = kickCount;
                hasParent = true;
            }
        }
    }

    private void Kick()
    {
        transform.parent = originParentTransform;
        rigidbody.isKinematic = false;
        hasParent = false;
        lastKickTime = Time.time;
    }
}
