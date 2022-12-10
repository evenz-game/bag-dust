using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleFlyingObject : MonoBehaviour
{
    private Vector2 flyingDirection;
    [SerializeField]
    private float flyingSpeed;
    private Vector3 flyingForce => flyingDirection * flyingSpeed;
    [SerializeField]
    private float knockbackForceScale = 1;

    [Header("UI")]
    [SerializeField]
    private ObstacleDangerUI dangerUIPrefab;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip spawnAudioClip;

    protected new Rigidbody rigidbody;
    private MeshRenderer[] meshRenderers;
    protected AudioSource audioSource;

    private void Awake()
    {
        rigidbody = GameObjectUtils.FindCompoenet<Rigidbody>(gameObject);
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        EnableMeshRenderers(false);

        flyingDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 0f)
            ).normalized;

        ObstacleDangerUI dangerUI = Instantiate<ObstacleDangerUI>(dangerUIPrefab, transform.position, Quaternion.Euler(0, 0, Angle(flyingDirection)));
        dangerUI.onDisabledUI.AddListener(Init);
    }

    public static float Angle(Vector2 direction)
    {
        float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        degree = (degree + 360) % 360;  // -180~180 -> 0~360

        return degree;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = true;
    }

    public virtual void Init()
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
        rigidbody.velocity = flyingForce;
        // rigidbody.AddForce(flyingSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerMovement>(other.gameObject, out PlayerMovement movement))
            movement.Knockback(rigidbody.velocity * knockbackForceScale);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameObjectUtils.FindCompoenet<PlayerStatus>(other.gameObject, out PlayerStatus status))
        {
            status.ClashObstacle(2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + flyingForce);
    }
}
