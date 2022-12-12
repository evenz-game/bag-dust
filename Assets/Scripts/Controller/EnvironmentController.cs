using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private bool isSetScene = false;
    [SerializeField]
    private GameObject[] environmentPrefabs;

    private void Awake()
    {
        int index = MyPlayerPrefs.GetMapIndex();

        if (isSetScene)
        {
            index = Random.Range(0, environmentPrefabs.Length);
            MyPlayerPrefs.SetMapIndex(index);
        }

        GameObject env = environmentPrefabs[index];

        Instantiate(env, Vector3.zero, Quaternion.identity);
    }
}
