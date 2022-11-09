using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inactivator : MonoBehaviour
{
    [SerializeField]
    private float delayTime;

    private void Inactive()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke(nameof(Inactive), delayTime);
    }
}
