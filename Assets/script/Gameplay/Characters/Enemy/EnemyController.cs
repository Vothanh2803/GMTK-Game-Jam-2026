using UnityEngine;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private float currentHP;
    [SerializeField] private Animator animator;

    public event Action OnEnemyDeath;
    public event Action OnTurnCompleted;

    private bool isHitFinished = false;
    private float currentDamge = 0f;
    public bool isParryWindowOpen = false;
    public float delayBeforeCombo = 1f;

    public float CurrentHP => currentHP;
    public float MaxHP => data != null ? data.HP : 100f;


    public void Init(EnemyData enemyData)
    {
        data = enemyData;
        currentHP = data.HP;
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    public void DoCombo()
    {
        int randomIndex = UnityEngine.Random.Range(0, data.comboList.Length);
        EnemyComboData selectedCombo = data.comboList[randomIndex];

        StartCoroutine(ExecuteCombo(selectedCombo));
    }

    private IEnumerator ExecuteCombo(EnemyComboData combo)
    {
        transform.position = EnemyManager.Instance.attackPoint.position;
        yield return new WaitForSeconds(delayBeforeCombo);

        foreach (EnemyAttackInfo attackInfo in combo.attackTypeList)
        {
            isHitFinished = false;
            currentDamge = attackInfo.damage;

            if (attackInfo.attackType == EnemyAttackType.heavyAttack)
            {
                animator.SetTrigger("HeavyAttack");
            }
            else
            {
                animator.SetTrigger("LightAttack");
            }
            yield return new WaitUntil(() => isHitFinished);

            yield return new WaitForSeconds(attackInfo.delayNextAttack);
        }

        transform.position = EnemyManager.Instance.spawnPoint.position;
        OnTurnCompleted?.Invoke();
        
    }

    public void OpenParryWindow()
    {
        isParryWindowOpen = true;
        Debug.Log("Open parry window");
    }

    //Gan event nay o frame danh trung player
    public void OnHitImpact()
    {
        GameManager.Instance.SendAttack("Player", currentDamge);
        isParryWindowOpen = false;
        Debug.Log("Close parry window");
    }

    //Gan event nay o frame cuoi cung cua animation
    public void OnHitComplete()
    {
        isHitFinished = true;
        isParryWindowOpen = false;
    }
}
