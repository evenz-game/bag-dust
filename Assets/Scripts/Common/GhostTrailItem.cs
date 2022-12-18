using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrailItem : MonoBehaviour
{
    [SerializeField]
    private float trailTime;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void OnEnable()
    {
        StartCoroutine(TrailRoutine());
    }

    private IEnumerator TrailRoutine()
    {
        float timer = 0, percent = 0;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / trailTime;

            spriteRenderer.color = Color.Lerp(startColor, endColor, percent);

            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(this.gameObject);
    }
}
