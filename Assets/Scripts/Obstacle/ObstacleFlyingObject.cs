using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleFlyingObject : MonoBehaviour
{
    [SerializeField]
    private bool isSubObstacle = false;
    public bool IsSubObstacle => isSubObstacle;

    [Space]
    [SerializeField]
    private bool useManualFlyingDirection = false;
    [SerializeField]
    private Vector2 manualFlyingDirection;
    [SerializeField]
    private bool spawnBottom = false;
    public bool SpawnBottom => spawnBottom;

    private Vector2 flyingDirection;
    [Space]
    [SerializeField]
    private float flyingSpeed;
    protected Vector3 flyingForce => flyingDirection * flyingSpeed;
    [SerializeField]
    private float knockbackForceScale = 1;

    [Header("Shake")]
    [SerializeField]
    private bool useShakeAtDestroy = false;
    [SerializeField]
    private float shakeAmount = 0.3f;
    [SerializeField]
    private float shakeDuration = 0.4f;

    [Header("UI")]
    [SerializeField]
    private ObstacleDangerUI dangerUIPrefab;
    [SerializeField]
    private Transform dangerUISpawnTransform;

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

        if (useManualFlyingDirection)
            flyingDirection = manualFlyingDirection.normalized;
        else
            flyingDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 0f)
                ).normalized;

        Vector3 dangerUISpawnPos = transform.position;
        if (dangerUISpawnTransform)
            dangerUISpawnPos = dangerUISpawnTransform.position;

        ObstacleDangerUI dangerUI = Instantiate<ObstacleDangerUI>(dangerUIPrefab, dangerUISpawnPos, Quaternion.Euler(0, 0, Angle(flyingDirection)));
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

    private void OnDestroy()
    {
        if (useShakeAtDestroy)
            CameraWalkingController.Shake(shakeAmount, shakeDuration);
    }
}
