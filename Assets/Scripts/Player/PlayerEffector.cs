using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffector : PlayerComponent
{
    [Header("Dash")]
    [SerializeField]
    private GameObject dashEffectPrefab;

    [Header("Clash Other Player")]
    [SerializeField]
    private GameObject clashOtherPlayerEffectPrefab;

    [Header("Clash Obstacle")]
    [SerializeField]
    private GameObject clashObstacleEffectPrefab;

    public void Dash()
    {
        Instantiate(dashEffectPrefab, transform.position, Quaternion.Euler(transform.localEulerAngles));
    }

    public void ClashOtherPlayer(Vector3 myPosition, Vector3 otherPlayerPosition)
    {
        Vector3 centerPosition = (myPosition + otherPlayerPosition) / 2;
        Instantiate(clashOtherPlayerEffectPrefab, centerPosition, Quaternion.identity);
    }

    public void ClashObstacle()
    {
        Instantiate(clashObstacleEffectPrefab, transform.position, Quaternion.identity);
    }
}
