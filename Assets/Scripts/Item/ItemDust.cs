using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDust : Item
{
    [SerializeField]
    private int increaseDustAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus status = FindCompoenet<PlayerStatus>(other.gameObject);
        if (!status) return;

        status.IncreaseDustCount(increaseDustAmount);

        Destroy(this.gameObject);
    }
}
