using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    private static SoundController instance;

    [SerializeField]
    private int soundLevel = 3;
    public static int SoundLevel => instance.soundLevel;

    private void Awake()
    {
        instance = this;
        soundLevel = MyPlayerPrefs.GetSoundLevel();
    }

    private void Start()
    {
        UpdateAudioMixer();
    }

    public static int NextLevel()
    {
        return instance._NextLevel();
    }

    private int _NextLevel()
    {
        soundLevel++;

        if (soundLevel > 3)
            soundLevel = 0;

        MyPlayerPrefs.SetSoundLevel(soundLevel);

        UpdateAudioMixer();

        return soundLevel;
    }

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private List<int> volumeSet = new List<int>() { -80, -20, -10, 0 };
    private void UpdateAudioMixer()
    {
        audioMixer.SetFloat("Master", volumeSet[soundLevel]);
    }
}
