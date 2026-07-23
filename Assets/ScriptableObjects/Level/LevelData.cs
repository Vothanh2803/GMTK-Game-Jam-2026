using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 0)]
public class LevelData : ScriptableObject {
    public string levelName;
    public EnemyData[] enemyList;
}