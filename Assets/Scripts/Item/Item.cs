using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected T FindCompoenet<T>(GameObject target) where T : MonoBehaviour
    {
        T result = null;

        result = target.GetComponent<T>();
        if (result) return result;

        result = target.GetComponentInChildren<T>();
        if (result) return result;

        result = target.GetComponentInParent<T>();
        return result;
    }
}
