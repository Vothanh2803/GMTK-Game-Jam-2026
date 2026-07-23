using System;

[Serializable]
public struct EnemyAttackInfo
{
    public EnemyAttackType attackType;
    public float damage;
    public float delayNextAttack;
}