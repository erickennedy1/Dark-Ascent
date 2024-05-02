using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbe : MonoBehaviour
{
    public bool prontoParaAtacar = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (AnimationHasFinished("contruindoOrbe"))
        {
            animator.Play("completaOrbe");
        }

        if (AnimationHasFinished("completaOrbe"))
        {
            animator.Play("carregadaOrbe");
            prontoParaAtacar = true;
        }
    }

    bool AnimationHasFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

}
