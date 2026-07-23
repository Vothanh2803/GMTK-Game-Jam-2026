using UnityEngine;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private float currentHP;

    public event Action OnEnemyDeath;
    public event Action OnTurnCompleted;

    private bool isHitFinished = false;
    private float currentDamge = 0f;


    public void Init(EnemyData enemyData)
    {
        data = enemyData;
        currentHP = data.HP;
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
    }

    private IEnumerator ExecuteCombo(EnemyComboData combo)
    {
        foreach (EnemyAttackInfo attackInfo in combo.attackTypeList)
        {
            isHitFinished = false;
            currentDamge = attackInfo.damage;

            if (attackInfo.attackType == EnemyAttackType.heavyAttack)
            {
                //Animation heavyAttack
                Debug.Log("heavyAttack");
            }
            else
            {
                //Animation lightAttack
                Debug.Log("lightAttack");
            }
            yield return new WaitUntil(() => isHitFinished);

            yield return new WaitForSeconds(attackInfo.delayNextAttack);
        }

        OnTurnCompleted?.Invoke();
    }

    //Gan event nay o frame danh trung player
    public void OnHitImpact()
    {
        GameManager.Instance.SendAttack("Player", currentDamge);
    }

    //Gan event nay o frame cuoi cung cua animation
    public void OnHitComplete()
    {
        isHitFinished = true;
    }
}
