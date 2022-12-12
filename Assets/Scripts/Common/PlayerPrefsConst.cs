using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerPrefs
{
    private readonly static string PLAYER_MODEL_INDEX = "playerModelIndex_";
    private readonly static string PLAYER_ACTIVE = "playerActive_";
    private readonly static string WINNER_INDEX = "winnerIndex";
    private readonly static string MAP_INDEX = "mapIndex";

    public static void SetPlayerModelIndex(int playerIndex, int modelIndex)
        => PlayerPrefs.SetInt(PLAYER_MODEL_INDEX + playerIndex.ToString(), modelIndex);
    public static int GetPlayerModelIndex(int playerIndex)
        => PlayerPrefs.GetInt(PLAYER_MODEL_INDEX + playerIndex.ToString(), -1);

    public static void SetPlayerActive(int playerIndex, bool value)
        => PlayerPrefs.SetInt(PLAYER_ACTIVE + playerIndex.ToString(), value ? 1 : 0);
    public static bool GetPlayerActive(int playerIndex)
        => PlayerPrefs.GetInt(PLAYER_ACTIVE + playerIndex.ToString(), 0) > 0;

    public static void SetWinnerIndex(int playerIndex)
        => PlayerPrefs.SetInt(WINNER_INDEX, playerIndex);
    public static int GetWinnerIndex()
        => PlayerPrefs.GetInt(WINNER_INDEX, -1);

    public static void SetMapIndex(int mapIndex)
        => PlayerPrefs.SetInt(MAP_INDEX, mapIndex);
    public static int GetMapIndex()
        => PlayerPrefs.GetInt(MAP_INDEX, 0);
}
