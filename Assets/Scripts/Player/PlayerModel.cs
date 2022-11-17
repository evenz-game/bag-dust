using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviour
{
    public UnityEvent<PlayerModelInfo> onInitializedPlayerModel = new UnityEvent<PlayerModelInfo>();

    [SerializeField]
    private PlayerModelInfo[] playerModelInfos;

    private void Awake()
    {
        // Debug
        Init(0);
    }

    public void Init(int targetModelIndex)
    {
        foreach (PlayerModelInfo info in playerModelInfos)
            if (info.ModelIndex == targetModelIndex)
                CreateModel(info);
    }

    private void CreateModel(PlayerModelInfo model)
    {
        PlayerModelInfo modelClone = Instantiate(model);
        modelClone.transform.parent = this.transform;
        modelClone.transform.localPosition = Vector3.zero;
        modelClone.transform.localEulerAngles = Vector3.zero;

        onInitializedPlayerModel.Invoke(modelClone);
    }

    public interface OnInitializedPlayerModel
    {
        public void OnInitializedPlayerModel(PlayerModelInfo model);
    }
}
