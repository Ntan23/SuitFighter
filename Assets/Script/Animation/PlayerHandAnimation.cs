using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandAnimation : MonoBehaviour
{
    private Animator animator;
    
    void Start() => animator = GetComponent<Animator>();

    public void BackToIdle() => animator.Play("Idle");
    public void PlayRockAnimation() => animator.Play("Rock");
    public void PlayPaperAnimation() => animator.Play("Paper");
    public void PlayScissorsAnimation() => animator.Play("Scissors");
    public void PlayGuardAnimation() => animator.Play("Guard");
}
