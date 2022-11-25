using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviour, PlayerStatus.OnChangedPlayerState
{
    public UnityEvent<PlayerModelInfo> onInitializedPlayerModel = new UnityEvent<PlayerModelInfo>();

    [SerializeField]
    private PlayerModelInfo[] playerModelInfos;

    private int currentModelIndex = 0;
    private PlayerModelInfo currentModel;

    private void Awake()
    {
        // Debug
        Init(currentModelIndex, PlayerModelType.Normal);
    }

    public void Init(int targetModelIndex, PlayerModelType targetModelType)
    {
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
        currentModel.transform.localEulerAngles = Vector3.zero;

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
