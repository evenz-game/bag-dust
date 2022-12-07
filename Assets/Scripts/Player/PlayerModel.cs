using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : PlayerComponent, PlayerStatus.OnChangedPlayerState
{
    [SerializeField]
    private bool useManualModelIndex = false;
    [SerializeField]
    private int modelIndex = -1;

    public UnityEvent<PlayerModelInfo> onInitializedPlayerModel = new UnityEvent<PlayerModelInfo>();

    [SerializeField]
    private PlayerModelInfo[] playerModelInfos;
    [SerializeField]
    private Vector3 spawnLocalEulerAngles = Vector3.zero;

    private int currentModelIndex = 0;
    private PlayerModelInfo currentModel;

    protected override void Awake()
    {
        base.Awake();

        // 수동 선택이 아닐 경우, 자동으로 불러옴
        if (!useManualModelIndex)
        {
            int storedModelIndex = PlayerPrefs.GetInt($"playerModelIndex_{playerStatus.Index}", -1);
            if (storedModelIndex > -1)
                modelIndex = storedModelIndex;
        }

        else if (modelIndex == -1)
        {
            Debug.LogError("PlayerModel: 모델 인덱스가 -1입니다.");
            return;
        }

        Init(modelIndex, PlayerModelType.Normal);
    }

    public void Init(int targetModelIndex, PlayerModelType targetModelType)
    {
        currentModelIndex = targetModelIndex;

        foreach (PlayerModelInfo info in playerModelInfos)
        {
            if (info.ModelIndex != targetModelIndex) continue;
            if (info.ModelType != targetModelType) continue;

            CreateModel(info);
            return;
        }

        Debug.LogError($"PlayerModel: 존재하지 않는 모델 인덱스({targetModelIndex})입니다.");
    }

    private void CreateModel(PlayerModelInfo model)
    {
        if (currentModel)
            Destroy(currentModel.gameObject);

        currentModel = Instantiate(model);
        currentModel.transform.parent = this.transform;
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localEulerAngles = spawnLocalEulerAngles;

        onInitializedPlayerModel.Invoke(currentModel);

        PlayerPrefs.SetInt($"playerModelIndex_{playerStatus.Index}", modelIndex);
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        if (currentPlayerState == PlayerState.Ghost)
            Init(currentModelIndex, PlayerModelType.Ghost);
    }

    public interface OnInitializedPlayerModel
    {
        public void OnInitializedPlayerModel(PlayerModelInfo model);
    }
}
