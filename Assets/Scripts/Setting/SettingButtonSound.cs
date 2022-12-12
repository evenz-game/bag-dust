using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingButtonSound : SettingButton
{
    [SerializeField]
    private Image[] imageLevels;

    [SerializeField]
    private Sprite onLevelSprite;
    [SerializeField]
    private Sprite offLevelSprite;

    private void Start()
    {
        UpdateLevelImage(SoundController.SoundLevel);
    }

    public override void Select()
    {
        base.Select();

        int level = SoundController.NextLevel();
        UpdateLevelImage(level);
    }

    private void UpdateLevelImage(int level)
    {
        int imageImg = level - 1;

        for (int i = 0; i < imageLevels.Length; i++)
        {
            if (i >= level)
                imageLevels[i].sprite = offLevelSprite;
            else
                imageLevels[i].sprite = onLevelSprite;

        }
    }
}
