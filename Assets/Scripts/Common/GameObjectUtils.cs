using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    public static T FindCompoenet<T>(GameObject targetGameObject) where T : Component
    {
        T result = null;

        result = targetGameObject.GetComponent<T>();
        if (result) return result;

        result = targetGameObject.GetComponentInChildren<T>();
        if (result) return result;

        result = targetGameObject.GetComponentInParent<T>();
        return result;
    }

    public static bool FindCompoenet<T>(GameObject targetGameObject, out T targetBehavior) where T : MonoBehaviour
    {
        targetBehavior = FindCompoenet<T>(targetGameObject);
        return targetBehavior != null;
    }
}
