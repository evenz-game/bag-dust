using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDust : Item
{
    [SerializeField]
    private int increaseDustAmount = 1;

    [Header("Scatter")]
    [SerializeField]
    private float disableColliderTime;
    [SerializeField]
    private float scatterAngle;
    [SerializeField]
    private float minScatterForceScale = 1;
    [SerializeField]
    private float maxScatterForceScale = 1;

    [Header("Minimize")]
    [SerializeField]
    private bool minimize = true;
    [SerializeField]
    private float minimizeDelayTimeAtStart = 2;
    [SerializeField]
    private float minimizeTime = 3;
    [SerializeField]
    private AnimationCurve minimizeCurve;

    [Space]
    [SerializeField]
    private Collider itemCollisionCollider;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    private void Start()
    {
        if (minimize)
            StartCoroutine(MinimizeRoutine());
    }

    public void Scatter(Vector3 scatterForce)
    {
        StartCoroutine(ScatterRoutine(scatterForce));
    }

    private IEnumerator ScatterRoutine(Vector3 scatterForce)
    {
        itemCollisionCollider.enabled = false;
        rigidbody.velocity = scatterForce * Random.Range(minScatterForceScale, maxScatterForceScale);

        yield return new WaitForSeconds(disableColliderTime);

        itemCollisionCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
        {
            status.IncreaseDustCount(increaseDustAmount);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator MinimizeRoutine()
    {
        yield return new WaitForSeconds(minimizeDelayTimeAtStart);

        Vector3 startScale = transform.localScale;

        float timer = 0, percent = 0;

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / minimizeTime;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, minimizeCurve.Evaluate(percent));

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
