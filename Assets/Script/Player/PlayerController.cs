using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerCombat combat;
    public PlayerParry parry;
    private PlayerRage rage;
    public PlayerStats stats;
    private Animator animator;
    
    [Header("Position Settings")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Transform idlePosition;

    [Header("Action Timing Settings")]
    [SerializeField] private float delayBeforeMove = 0.2f;
    [SerializeField] private float delayBeforeAttack = 0.5f;

    private bool isHitFinished = true; 
    public bool isAttackedThisTurn = false;

    private void Awake()
    {
        combat = GetComponent<PlayerCombat>();
        parry = GetComponent<PlayerParry>();
        rage = GetComponent<PlayerRage>();
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    public void OnLightAttack()
    {
        if (isAttackedThisTurn) return;
        if (!combat.CanLightAttack()) return; 

        isAttackedThisTurn = true;
        StartCoroutine(ExecuteAttackRoutine("LightAttack"));
    }

    public void OnHeavyAttack()
    {
        if (isAttackedThisTurn) return;
        if (!combat.CanHeavyAttack()) return; 

        isAttackedThisTurn = true;
        StartCoroutine(ExecuteAttackRoutine("HeavyAttack"));
    }

    public void OnRageAttack()
    {
        if (isAttackedThisTurn) return;
        if (!combat.CanPerformRageAttack()) return;

        isAttackedThisTurn = true;
        StartCoroutine(ExecuteAttackRoutine("RageAttack"));
    }

    public void OnBlock()
    {
        if (isAttackedThisTurn) return;

        isAttackedThisTurn = true;
        combat.Block();
    }

    public void OnHurt()
    {
        if (animator != null) animator.SetTrigger("Hurt");
    }

    private IEnumerator ExecuteAttackRoutine(string attackTriggerName)
    {
        isHitFinished = false;

        yield return new WaitForSeconds(delayBeforeMove);
        transform.position = attackPosition.position;

        yield return new WaitForSeconds(delayBeforeAttack);

        if (animator != null)
        {
            animator.SetTrigger(attackTriggerName);
        }

        yield return new WaitUntil(() => isHitFinished);

        yield return new WaitForSeconds(delayBeforeMove);
        transform.position = idlePosition.position;

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void OnLightAttackImpact()
    {
        combat.LightAttack();
    }

    public void OnHeavyAttackImpact()
    {
        combat.HeavyAttack();
    }

    public void OnRageHitImpact()
    {
        combat.ExecuteRageSingleHitDamage();
    }
    
    public void OnHitComplete()
    {
        isHitFinished = true;
    }
}