using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDustParticleCreator : PlayerComponent, PlayerStatus.OnChangedDustCount, PlayerStatus.OnChangedPlayerState
{
    [SerializeField]
    private GameObject dustParticlePrefab;          // 먼지 조각 프리팹

    [Space]
    [SerializeField]
    private int dustParticleCountAtDeath = 50;

    private new Rigidbody rigidbody;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    public void OnChangedDustCount(int previousDustCount, int currentDustCount)
    {
        int diff = currentDustCount - previousDustCount;

        if (diff >= 0) return;

        CreateDustParticle(Mathf.Abs(diff));
    }

    private void CreateDustParticle(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject dustClone = Instantiate(dustParticlePrefab, transform.position, Quaternion.identity);

            ItemDust itemDust = GameObjectUtils.FindCompoenet<ItemDust>(dustClone);
            itemDust.Scatter(rigidbody.velocity);
        }
    }

    public void OnChangedPlayerState(PlayerState currentPlayerState)
    {
        if (currentPlayerState == PlayerState.Dead)
            CreateDustParticle(dustParticleCountAtDeath);
    }
}
