using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    
    void Start() => animator = GetComponent<Animator>();

    public void BackToIdle() => animator.Play("Idle");
    public void PlayAttackAnimation() => animator.Play("Attack");
    public void PlayAttackedAnimation() => animator.Play("Attacked");
}
