using UnityEngine;

[CreateAssetMenu(fileName = "EnemyComboData", menuName = "Enemy/EnemyComboData", order = 0)]
public class EnemyComboData : ScriptableObject {
    public EnemyAttackInfo[] attackTypeList;
}