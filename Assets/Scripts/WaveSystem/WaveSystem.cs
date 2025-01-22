using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class EnemySpawn
{
    [SerializeField]
    public Enemy enemy;
    [SerializeField]
    public int amount;
}
[Serializable]
public class Wave
{
    [SerializeField]
    public int waveTime = 20;
    [SerializeField]
    public List<EnemySpawn> enemiesIncoming;
    [SerializeField]
    public DialogueItem playingDialogue;
    [SerializeField]
    public int goldReward = 10;
}
[CreateAssetMenu(fileName = "Waves", menuName = "Waves/Waves")]
public class WaveSystem : ScriptableObject
{
    public List<Wave> waves;
    public int intermissionTime;
    public int enemySpawnRate = 2;
}
