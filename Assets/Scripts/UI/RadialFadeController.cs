using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialFadeController : MonoBehaviour
{
    public Animator animator;

    public void SetFadeOn()
    {
        animator.SetBool("isFadeOn", true);
    }

    public void SetFadeOff()
    {
        animator.SetBool("isFadeOn", false);
    }

}
