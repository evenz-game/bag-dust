using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerStatus : MonoBehaviour
{
    [Header("Dust Count")]
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
    [Min(0)]
    [SerializeField]
    private int currentDustCount = 3;                  // 현재 먼지 개수
    public int CurrentDustCount => currentDustCount;

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
    private float currentWeight;                        // 현재 무게
    public float CurrentWeight => currentWeight;

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

    private void Start()
    {
        IncreaseDustCount(0, true);
    }

    /// <summary>
    /// 먼지 개수를 변경합니다.
    /// </summary>
    /// <param name="amount">변경할 먼지 개수(양)</param>
    /// <returns>실제로 변경된 먼지 개수</returns>
    public int IncreaseDustCount(int amount)
    {
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
        currentDustCount = Mathf.Clamp(currentDustCount, minDustCount, maxDustCount);

        // 먼지 개수 변경 없으면 0(변경된 개수) 리턴
        bool changed = currentDustCount != prevDustCount;
        if (!changed && !init) return 0;

        // 현재 먼지 개수가 몇 퍼센트인지
        float percent = (float)(currentDustCount - minDustCount) / (float)(maxDustCount - minDustCount);

        // 무게 및 스케일 업데이트 후, 이전 값 저장
        float prevWeight = UpdateWeight(percent);
        float prevScale = UpdateScale(percent);

        // 변경 이벤트 호출
        onChangedDustCount.Invoke(prevDustCount, currentDustCount);
        onChangedWeight.Invoke(prevWeight, currentWeight);
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

        currentWeight = Mathf.Lerp(minWeight, maxWeight, percent);

        return prev;
    }

    /// <summary>
    /// 스케일 값을 업데이트합니다.
    /// </summary>
    /// <param name="percent">현재 먼지 개수가 몇 퍼센트인지</param>
    /// <returns>이전 스케일 값</returns>
    private float UpdateScale(float percent)
    {
        float prev = currentScale;

        currentScale = Mathf.Lerp(minScale, maxScale, percent);

        return prev;
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
