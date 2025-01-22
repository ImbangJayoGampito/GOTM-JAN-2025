using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public enum WaveState
{
    Free,
    Spawning,
    Clearing,
    Intermission,
    Finished
}
public class WaveImplementation : MonoBehaviour
{
    public WaveSystem waveSystem;
    public int spawnRate = 2;
    //int maxWave = 2;
    WaveState waveState = WaveState.Free;
    int currentWave = 0;
    Entity player;
    List<Enemy> enemies;
    // Cooldown spawnCooldown;
    Cooldown intermissionCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spawnCooldown.CooldownByRate(spawnRate);
        intermissionCooldown = gameObject.AddComponent<Cooldown>();
        intermissionCooldown.SetCooldown(waveSystem.intermissionTime);
        enemies = new List<Enemy>();
        intermissionCooldown.SetCooldown(false);
    }
    // Update is called once per frame
    void Update()
    {
        Intermission();
        WaitWaveClear();
        // Debug.Log("Intermission's cooldown = " + intermissionCooldown.GetCurrentCooldown());
    }
    public void StartGame()
    {
        if (waveState == WaveState.Finished)
        {
            Debug.Log("You've cleared this level, congratulations!");
            return;
        }
        waveState = WaveState.Intermission;
        Debug.Log("Game has started! Good luck!");
        intermissionCooldown.SetCooldown(false);
        intermissionCooldown.ResetCooldown();
    }
    public void EndGame()
    {
        if (waveState == WaveState.Finished)
        {
            Debug.Log("You've been rewarded meow");
            return;
        }
        waveState = WaveState.Free;
        currentWave = 0;
        Debug.Log("Game ended!, you lose meow");
    }
    void WaitWaveClear()
    {
        WaveState prevState = waveState;
        if (waveState != WaveState.Clearing || enemies.Count > 0)
        {
            return;
        }

        if (currentWave < waveSystem.waves.Count - 1)
        {
            Debug.Log("All enemies are cleared! Back to intermission");
            waveState = WaveState.Intermission;
            currentWave++;
        }
        else
        {
            waveState = WaveState.Finished;
            Debug.Log("Enemies cleared! Congratulations!!!!");
        }
    }
    IEnumerator SpawnEnemy()
    {
        foreach (EnemySpawn enemyToSpawn in waveSystem.waves[currentWave].enemiesIncoming)
        {
            for (int i = 0; i < enemyToSpawn.amount; i++)
            {
                Debug.Log("spawned meowww");
                yield return new WaitForSeconds(spawnRate <= 0 ? 1 : 1.0f / (float)spawnRate);
            }
        }
        waveState = WaveState.Clearing;
        Debug.Log("Clearing time!");
    }
    void Intermission()
    {
        // WaveState prevState = waveState;
        if (waveState != WaveState.Intermission)
        {
            return;
        }
        intermissionCooldown.SetCooldown(true);
        if (intermissionCooldown.IsCooldown())
        {
            return;
        }
        intermissionCooldown.SetCooldown(false);
        waveState = WaveState.Spawning;
        Debug.Log("It is now.... the " + currentWave + " Wave meow");
        StartCoroutine(SpawnEnemy());
    }
    bool IsValidPlayer(Collision collision)
    {
        Entity other = collision.gameObject.GetComponent<Entity>();
        PlayerEntity indicator = collision.gameObject.GetComponent<PlayerEntity>();
        if (other == null || indicator == null)
        {
            return false;
            // Attacked(other);
        }
        return true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (IsValidPlayer(collision))
        {
            StartGame();
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (IsValidPlayer(collision))
        {
            EndGame();
        }

    }
}
