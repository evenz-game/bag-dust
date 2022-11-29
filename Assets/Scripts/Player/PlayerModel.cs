using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviour, PlayerStatus.OnChangedPlayerState
{
    [Header("Debug")]
    [SerializeField]
    private int debugModelIndex = -1;

    public UnityEvent<PlayerModelInfo> onInitializedPlayerModel = new UnityEvent<PlayerModelInfo>();

    [SerializeField]
    private PlayerModelInfo[] playerModelInfos;
    [SerializeField]
    private Vector3 spawnLocalEulerAngles = Vector3.zero;

    private int currentModelIndex = 0;
    private PlayerModelInfo currentModel;

    private void Awake()
    {
        // Debug
        if (debugModelIndex > -1)
            Init(debugModelIndex, PlayerModelType.Normal);
    }

    public void Init(int targetModelIndex, PlayerModelType targetModelType)
    {
        currentModelIndex = targetModelIndex;

        foreach (PlayerModelInfo info in playerModelInfos)
            if (info.ModelIndex == targetModelIndex && info.ModelType == targetModelType)
                CreateModel(info);
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
