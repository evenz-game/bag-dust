using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectTracker : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position);
    }

    private void OnEnable()
    {
        UpdatePosition();
    }
}
