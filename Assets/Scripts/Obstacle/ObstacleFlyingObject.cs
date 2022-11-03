using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleFlyingObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 flyingForce;
    [SerializeField]
    private float knockbackForceScale = 1;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip spawnAudioClip;

    private new Rigidbody rigidbody;
    private MeshRenderer[] meshRenderers;
    protected AudioSource audioSource;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        EnableMeshRenderers(false);
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = true;
        Invoke(nameof(Init), 1f);
    }

    protected virtual void Init()
    {
        EnableMeshRenderers(true);
        AddForce();

        audioSource?.PlayOneShot(spawnAudioClip);
    }

    protected void EnableMeshRenderers(bool value)
    {
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = value;
    }

    protected virtual void AddForce()
    {
        rigidbody.isKinematic = false;
        // rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(flyingForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement movement))
            movement.Knockback(rigidbody.velocity * knockbackForceScale);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
            status.IncreaseDustCount(-2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + flyingForce);
    }
}
