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

    [SerializeField]
    private Collider itemCollisionCollider;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
    }

    public void Scatter(Vector3 scatterForce)
    {
        print("ItemDust.Scatter" + scatterForce.ToString());
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
}
