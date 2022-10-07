using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected Player player;
    protected PlayerStatus playerStatus;

    protected virtual void Awake()
    {
        player = FindCompoenet<Player>();
        playerStatus = FindCompoenet<PlayerStatus>();
    }

    protected T FindCompoenet<T>() where T : MonoBehaviour
    {
        T result = null;

        result = GetComponent<T>();
        if (result) return result;

        result = GetComponentInChildren<T>();
        if (result) return result;

        result = GetComponentInParent<T>();
        return result;
    }
}
