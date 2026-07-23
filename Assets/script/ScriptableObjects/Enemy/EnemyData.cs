using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Enemy Data", order = 0)]
public class EnemyData : ScriptableObject {
    public float HP;
    public GameObject enemyPrefab; 
    
}
