using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
public enum EnemyMovimentType
{
    IDLE,
    WALK,
    RUN,
    ATACK,
    DIE
}
public class EnemyAnimation : NetworkBehaviour
{
    private Animator animator;
    [SyncVar]
    public EnemyMovimentType type;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Animation();
    }
    private void Animation()
    {
        if (type == EnemyMovimentType.DIE)
        {
            animator.SetLayerWeight(1, 0);
            animator.Play("Death");
            return;
        }
        if (type == EnemyMovimentType.ATACK)
        {
            animator.SetLayerWeight(1, 1);
            animator.Play("Attack");
        }
        animator.SetBool("Walk", type == EnemyMovimentType.WALK);
        animator.SetBool("Run", type == EnemyMovimentType.RUN);
    }
    public void SetType(EnemyMovimentType _type)
    {
        type = _type;
    }

}
