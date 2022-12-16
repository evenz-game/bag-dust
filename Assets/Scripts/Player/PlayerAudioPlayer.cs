using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioPlayer : PlayerComponent
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip dashAudioClip;
    [SerializeField]
    private AudioClip lastChanceAudioClip;
    [SerializeField]
    private AudioClip dieAudioClip;
    [SerializeField]
    private AudioClip clashOtherPlayerAudioClip;
    [SerializeField]
    private AudioClip clashObstacleAudioClip;

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerStatus.onChangedPlayerState.AddListener((PlayerState state) =>
        {
            if (state == PlayerState.LastChance) LastChance();
            else if (state == PlayerState.Dead) Die();
        });

        playerStatus.onChangedDustCount.AddListener((int prevCnt, int newCnt) =>
        {
            if (prevCnt > newCnt) ClashObstacle();
        });
    }

    public void Dash()
    {
        audioSource.PlayOneShot(dashAudioClip);
    }

    public void LastChance()
    {
        audioSource.PlayOneShot(lastChanceAudioClip);
    }

    public void Die()
    {
        audioSource.PlayOneShot(dieAudioClip);
    }

    public void ClashOtherPlayer()
    {
        audioSource.PlayOneShot(clashOtherPlayerAudioClip);
    }

    public void ClashObstacle()
    {
        audioSource.PlayOneShot(clashObstacleAudioClip);
    }
}
