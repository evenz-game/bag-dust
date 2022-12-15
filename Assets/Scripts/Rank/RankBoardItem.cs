using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankBoardItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textRank;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private TextMeshProUGUI textScore;

    private void Awake()
    {
        textRank.text = "";
        textName.text = "";
        textScore.text = "";
    }

    public void Init(int rank, string name, int score)
    {
        textRank.text = rank.ToString();
        textName.text = name;
        textScore.text = score.ToString();
    }
}
