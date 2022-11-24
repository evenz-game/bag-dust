using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        for (int i = 0; i < ghostChildren.Count; i++)
        {
            PlayerGhost child = ghostChildren[i];

            if (child.Kick())
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
            child.Kick(999);        // 999: 무조건 떨어뜨린다는 의미
            RemoveChildren(child);
            playerStatus.AddWeight(-ghostWeight);
        }
    }

    /* ------ 자식 ------ */
    public UnityEvent<int> onChangedLeftKickCount = new UnityEvent<int>();              // 남은 킥 횟수 변화 이벤트
    public UnityEvent<PlayerGhost> onChangedParent = new UnityEvent<PlayerGhost>();     // 부모 변경 이벤트

    private Transform originParentTransform;

    [SerializeField]
    private float ghostWeight;

    [SerializeField]
    private int kickCount = 15;
    public int leftKickCount = 0;

    private PlayerGhost lastParent = null;  // 마지막으로 붙어있던 부모
    private float lastKickOffTime = 0;      // 마지막으로 차여서 "떨어져나간" 시간

    private bool hasParent = false;

    private void OnTriggerStay(Collider other)
    {

        // 내가 유령일 때, 다른 플레이어와 부딪히면
        if (isGhost && !hasParent && GameObjectUtils.FindCompoenet<PlayerGhost>(other.gameObject, out PlayerGhost parent))
        {
            // 같은 부모인데, 떨어진지 3초 전이라면 다시 못 붙음
            if (parent == lastParent && Time.time - lastKickOffTime < 3) return;

            // 부모가 유령이 아닐 때
            // 나를 자식으로 넣고
            if (!parent.isGhost)
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;

                lastParent = parent;
                parent.AddChildren(this);
                parent.playerStatus.AddWeight(ghostWeight);
                onChangedParent.Invoke(parent);

                leftKickCount = kickCount;
                onChangedLeftKickCount.Invoke(leftKickCount);

                hasParent = true;
            }
        }
    }

    private bool Kick(int kickCount = 1)
    {
        leftKickCount -= kickCount;
        leftKickCount = Mathf.Max(0, leftKickCount);
        onChangedLeftKickCount.Invoke(leftKickCount);

        if (leftKickCount == 0)
        {
            onChangedParent.Invoke(null);
            transform.parent = originParentTransform;
            rigidbody.isKinematic = false;
            hasParent = false;
            lastKickOffTime = Time.time;
            return true;
        }

        return false;
    }
}
