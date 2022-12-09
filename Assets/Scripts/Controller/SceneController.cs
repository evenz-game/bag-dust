using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private KeyCode[] restartKeyCode;

    public UnityEvent onStartedScene = new UnityEvent();

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        onStartedScene.Invoke();
    }

    private void Update()
    {
        foreach (KeyCode key in restartKeyCode)
        {
            if (Input.GetKeyDown(key))
                Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
