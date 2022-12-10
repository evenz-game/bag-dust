using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableCharacterState { None, Hover, Select }

public class SelectableCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform characterTransform;
    public Transform CharacterTransform => characterTransform;

    [SerializeField]
    private SelectableCharacterState state = SelectableCharacterState.None;
    public SelectableCharacterState State => state;

    public int ModelIndex
        => GetComponentInChildren<PlayerModelInfo>()
            ? GetComponentInChildren<PlayerModelInfo>().ModelIndex
            : -1;

    public SelectableCharacter FindSelectableCharacterByAxis(Vector2 axis)
        => SelectCharacterController.FindUnhoveredSelectableCharacterByAxis(this, axis);

    public void UpdateState(SelectableCharacterState newState)
    {
        state = newState;

        StopAllCoroutines();

        if (newState != SelectableCharacterState.None)
            StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        Rigidbody rigidbody = characterTransform.GetComponent<Rigidbody>();
        PlayerAnimator animator = characterTransform.GetComponent<PlayerAnimator>();

        float animTime = Random.Range(0.5f, 3f);
        float deltaTime = Random.Range(0.5f, 2f);
        float timer = 0;

        while (true)
        {
            rigidbody.AddTorque(Random.insideUnitSphere, ForceMode.Impulse);
            rigidbody.velocity = Vector3.zero;
            animator.Dash();

            yield return new WaitForSeconds(deltaTime);
            timer += deltaTime;

            if (timer >= animTime)
                break;

            deltaTime = Random.Range(0.5f, 2f);
        }
    }
}
