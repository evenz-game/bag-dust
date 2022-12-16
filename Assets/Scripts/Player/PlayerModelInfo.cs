using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerModelType { Normal, Ghost }
public class PlayerModelInfo : MonoBehaviour
{
    [SerializeField]
    private int modelIndex = -1;
    public int ModelIndex => modelIndex;

    [SerializeField]
    private PlayerModelType modelType = PlayerModelType.Normal;
    public PlayerModelType ModelType => modelType;

    [Header("Face")]
    [SerializeField]
    private Transform playerFaceTransform;
    public Transform PlayerFaceTransform => playerFaceTransform;
    [SerializeField]
    private float faceHeight;
    public float FaceHeight => faceHeight;

    [Header("Body")]
    [SerializeField]
    private Transform bodyTransform;
    public Transform BodyTransform => bodyTransform;
    [SerializeField]
    private Collider bodyCollider;
    public Collider BodyCollider => bodyCollider;

    [Header("Animator")]
    [SerializeField]
    private Animator animator;
    public Animator Animator => animator;

    private void Awake()
    {
        if (modelIndex == -1 || bodyCollider == null)
            Debug.LogWarning("PlayerModelInfo: 모델 설정이 정상적이지 않습니다.");
    }
}
