using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{

    [SerializeField]
    private float deltaTime;
    private float timer;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = deltaTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < deltaTime) return;

        timer = 0;

        GhostTrailItem trailItem = ObjectPooler.SpawnFromPool<GhostTrailItem>("Ghost Trail Item", transform.position, transform.rotation);
        trailItem.transform.localScale = transform.lossyScale;
        trailItem.Init(spriteRenderer.sprite);
    }
}
