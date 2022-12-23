using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PlayerStatus;

public enum PlayerState
{
    Dead = -1, Ghost = -2,
    Live = 1, LastChance = 2
}

[Serializable]
public class PlayerStatus : PlayerComponent, GameController.OnFinishedGame
{
    [Header("Init")]
    [SerializeField]
    private PlayerModel playerModel;
    private bool init = false;

    [Header("Index")]
    [SerializeField]
    private int index = 0;
    public int Index => index;

    [Header("State")]
    public UnityEvent<PlayerState> onChangedPlayerState
        = new UnityEvent<PlayerState>();
    private PlayerState currentPlayerState              // 현재 플레이어 상태
        = PlayerState.Live;
    public PlayerState CurrentPlayerState
        => currentPlayerState;

    [Header("Dust")]
    public UnityEvent<int, int> onChangedDustCount      // 먼지 변경 이벤트 (prevDustCount, curDustCount);
        = new UnityEvent<int, int>();
    [Min(0)]
    [SerializeField]
    private int minDustCount = 0;                       // 최소 먼지 개수
    public int MinDustCount => minDustCount;
    [Min(0)]
    [SerializeField]
    private int maxDustCount = 10;                      // 최대 먼지 개수
    public int MaxDustCount => maxDustCount;
    [SerializeField]
    private bool useMaxCount = true;                    // 최대 존재 여부 (싱글 모드)
    [Min(0)]
    [SerializeField]
    private int currentDustCount = 3;                   // 현재 먼지 개수
    public int CurrentDustCount => currentDustCount;
    private float dustCountPercent;                     // 현재 먼지 개수가 몇 퍼센트인지

    [Header("Weight")]
    public UnityEvent<float, float> onChangedWeight     // 무게 변경 이벤트 (prevWeight, curWeight);
        = new UnityEvent<float, float>();
    [SerializeField]
    private float minWeight = 1;                        // 최소 무게
    public float MinWeight => minWeight;
    [SerializeField]
    private float maxWeight = 5;                        // 최대 무게
    public float MaxWeight => maxWeight;
    [SerializeField]
    private float currentWeight;                        // 현재 캐릭터 무게
    private float addedWeight;                          // 추가되는 무게
    public float TotalWeight => currentWeight + addedWeight;
    [SerializeField]
    private bool useWeightCurve;                        // 커브 사용 여부
    [SerializeField]
    private AnimationCurve weightCurve;                 // 무게 커브

    [Header("Scale")]
    public UnityEvent<float, float> onChangedScale      // 스케일 변경 이벤트 (prevScale, curScale);
        = new UnityEvent<float, float>();
    [SerializeField]
    private float minScale = 1;                         // 최소 스케일
    public float MinSclae => minScale;
    [SerializeField]
    private float maxScale = 5;                         // 최대 스케일
    public float MaxSclae => maxScale;
    [SerializeField]
    private float currentScale;                         // 현재 스케일
    public float CurrentSclae => currentScale;
    [SerializeField]
    private bool useScaleCurve;                         // 커브 사용 여부
    [SerializeField]
    private AnimationCurve scaleCurve;                  // 스케일 커브

    [Header("Movement")]
    [SerializeField]
    private float dashPower;                            // 대쉬 파워
    public float DashPower => dashPower;
    [SerializeField]
    private float minRotateSpeed;                       // 최소 회전 속도
    public float MinRotateSpeed => minRotateSpeed;
    [SerializeField]
    private float maxRotateSpeed;                       // 최대 회전 속도
    public float MaxRotateSpeed => maxRotateSpeed;
    public float RotateSpeed => Mathf.Lerp(minRotateSpeed, maxRotateSpeed, dustCountPercent);

    [Header("Ghost")]
    [SerializeField]
    private int ghostDustCount = 3;

    [Header("For Single Mode Result Scene")]
    [SerializeField]
    private bool isSingleModeResultScene = false;

    protected override void Awake()
    {
        base.Awake();

        playerModel.onInitializedPlayerModel.AddListener((_) => Init());

        // 죽음 체크
        // 죽을 경우, 유령 상태로 변경
        onChangedPlayerState.AddListener(CheckDeath);
    }

    private void Init()
    {
        if (init) return;

        if (isSingleModeResultScene)
            IncreaseDustCount(MyPlayerPrefs.GetDustCount(1) - currentDustCount, true);
        else
            IncreaseDustCount(0, true);

        init = true;
    }

    /// <summary>
    /// 먼지 개수를 증가시킵니다.
    /// </summary>
    /// <param name="amount">증가시킬 먼지 개수(양)</param>
    /// <returns>실제로 변경된 먼지 개수</returns>
    public int IncreaseDustCount(int amount)
    {
        // 이미 죽어있으면, 리턴
        if ((int)currentPlayerState <= (int)PlayerState.Dead)
            return 0;

        // 외부 호출은 무조건 초기화를 위한 호출이 아님
        return IncreaseDustCount(amount, false);
    }

    /// <summary>
    /// 먼지 개수를 변경합니다.
    /// </summary>
    /// <param name="amount">변경할 먼지 개수(양)</param>
    /// <param name="init">초기화를 위한 호출인지 여부</param>
    /// <returns>실제로 변경된 먼지 개수</returns>
    private int IncreaseDustCount(int amount, bool init)
    {
        // 이번 먼지 개수 저장
        int prevDustCount = currentDustCount;

        // 현재 먼지 개수 업데이트
        currentDustCount += amount;
        if (useMaxCount)
            currentDustCount = Mathf.Clamp(currentDustCount, minDustCount, maxDustCount);
        else
            currentDustCount = Mathf.Clamp(currentDustCount, minDustCount, Int16.MaxValue);

        // 플레이어 상태 업데이트 후, 변경 이벤트 호출
        if (UpdatePlayerState(prevDustCount, currentDustCount, amount))
            onChangedPlayerState.Invoke(currentPlayerState);

        // 먼지 개수 변경 없으면 0(변경된 개수) 리턴
        bool changed = currentDustCount != prevDustCount;
        if (!changed && !init)
            return 0;

        // 현재 먼지 개수가 몇 퍼센트인지
        dustCountPercent = (float)(currentDustCount - minDustCount) / (float)(maxDustCount - minDustCount);

        // 무게 및 스케일 업데이트 후, 이전 값 저장
        float prevWeight = UpdateWeight(dustCountPercent);
        float prevScale = UpdateScale(dustCountPercent);

        // 변경 이벤트 호출
        onChangedDustCount.Invoke(prevDustCount, currentDustCount);
        onChangedWeight.Invoke(prevWeight, TotalWeight);
        onChangedScale.Invoke(prevScale, currentScale);

        // 먼지 개수 차이 반환
        int diff = currentDustCount - prevDustCount;
        return diff;
    }

    /// <summary>
    /// 무게 값을 업데이트합니다.
    /// </summary>
    /// <param name="percent">현재 먼지 개수가 몇 퍼센트인지</param>
    /// <returns>이전 무게 값</returns>
    private float UpdateWeight(float percent)
    {
        float prev = currentWeight;

        if (useWeightCurve)
            currentWeight = Mathf.LerpUnclamped(minWeight, maxWeight, weightCurve.Evaluate(percent));
        else
            currentWeight = Mathf.LerpUnclamped(minWeight, maxWeight, percent);

        return prev;
    }

    /// <summary>
    /// 스케일 값을 업데이트합니다.
    /// </summary>
    /// <param name="percent">현재 먼지 개수가 몇 퍼센트인지</param>
    /// <returns>이전 스케일 값</returns>
    private float UpdateScale(float percent)
    {
        float prev = TotalWeight;

        if (useScaleCurve)
            currentScale = Mathf.LerpUnclamped(minScale, maxScale, scaleCurve.Evaluate(percent));
        else
            currentScale = Mathf.LerpUnclamped(minScale, maxScale, percent);

        return prev;
    }

    /// <summary>
    /// 플레이어 상태(state)를 업데이트합니다.
    /// </summary>
    /// <param name="previousDustCount">이전 먼지 개수</param>
    /// <param name="currentDustCount">현재 먼지 개수</param>
    /// <param name="increaseDustAmount">증가시킬 먼지 개수(양)</param>
    /// <returns>상태 변경 여부</returns>
    private bool UpdatePlayerState(int previousDustCount, int currentDustCount, int increaseDustAmount)
    {
        if (currentPlayerState == PlayerState.Ghost) return false;

        PlayerState prevPlayerState = currentPlayerState;

        if (previousDustCount == 0)
        {
            if (currentDustCount == 0 && increaseDustAmount < 0)
                currentPlayerState = PlayerState.Dead;

            else if (currentDustCount > 0 && increaseDustAmount > 0)
                currentPlayerState = PlayerState.Live;
        }
        else
        {
            if (currentDustCount == 0 && increaseDustAmount < 0)
                currentPlayerState = PlayerState.LastChance;

            else if (currentDustCount > 0)
                currentPlayerState = PlayerState.Live;
        }

        return currentPlayerState != prevPlayerState;
    }

    public void CheckDeath(PlayerState currentPlayerState)
    {
        // 죽으면 3초뒤 유령으로 상태 변경
        if (currentPlayerState == PlayerState.Dead)
            Invoke(nameof(ChangeToGhost), 3);
    }

    private void ChangeToGhost()
    {
        currentPlayerState = PlayerState.Ghost;
        onChangedPlayerState.Invoke(currentPlayerState);
        IncreaseDustCount(ghostDustCount, true);
    }

    public void AddWeight(float weight)
    {
        if (weight == 0) return;
        float prev = TotalWeight;

        addedWeight += weight;

        if (addedWeight < 0)
            addedWeight = 0;

        onChangedWeight.Invoke(prev, TotalWeight);
    }

    public void ClashObstacle(int decreaseDustAmount)
    {
        IncreaseDustCount(-decreaseDustAmount);

        playerEffector?.ClashObstacle();
    }

    public void OnFinishedGame()
    {
        MyPlayerPrefs.SetDustCount(index, currentDustCount);
    }

    public interface OnChangedPlayerState
    {
        public void OnChangedPlayerState(PlayerState currentPlayerState);
    }
    public interface OnChangedDustCount
    {
        public void OnChangedDustCount(int previousDustCount, int currentDustCount);
    }
    public interface OnChangedWeight
    {
        public void OnChangedWeight(float previousWeight, float currentWeight);
    }
    public interface OnChangedScale
    {
        public void OnChangedScale(float previousScale, float currentScale);
    }
}
