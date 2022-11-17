using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : PlayerComponent, PlayerModel.OnInitializedPlayerModel
{
    [SerializeField]
    private Animator animator;

    public void Dash()
    {
        animator?.Play("Dash", -1, 0);
    }

    public void OnInitializedPlayerModel(PlayerModelInfo model)
    {
        animator = model.Animator;
    }
}
