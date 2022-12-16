using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : PlayerComponent, PlayerModel.OnInitializedPlayerModel
{
    [SerializeField]
    private Animator animator;

    private int modelIndex = 0;

    public void Dash()
    {
        animator?.Play("Dash", -1, 0);
    }

    public void Win()
    {
        animator?.Play("Win", -1, 0);
    }

    public void Ready()
    {
        animator?.SetFloat("Model Index", modelIndex);
        animator?.Play("Ready", -1, 0);
    }

    public void OnInitializedPlayerModel(PlayerModelInfo model)
    {
        animator = model.Animator;
        modelIndex = model.ModelIndex;
    }
}
