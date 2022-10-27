using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour, Inputter.OnButtonDown
{
    [SerializeField]
    private Animator playerAnimator;

    public void OnButtonDown(ButtonType buttonType)
    {
        if (buttonType == ButtonType.A)
            playerAnimator.Play("Dash", -1, 0);
    }
}
