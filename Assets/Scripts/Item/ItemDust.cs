using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDust : Item
{
    [SerializeField]
    private int increaseDustAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
        {
            status.IncreaseDustCount(increaseDustAmount);
            Destroy(this.gameObject);
        }
    }
}
