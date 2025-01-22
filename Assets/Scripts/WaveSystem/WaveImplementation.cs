using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Renderer renderer;
    Cooldown enemyCheckCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spawnCooldown.CooldownByRate(spawnRate);
        intermissionCooldown = gameObject.AddComponent<Cooldown>();
        enemyCheckCooldown = gameObject.AddComponent<Cooldown>();
        enemyCheckCooldown.SetCooldown(0.1f);
        intermissionCooldown.SetCooldown(waveSystem.intermissionTime);
        enemies = new List<Enemy>();
        intermissionCooldown.SetCooldown(false);
        renderer = gameObject.GetComponent<Renderer>();

    }
    // Update is called once per frame
    void Update()
    {
        Intermission();
        CheckEnemy();
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
    // Because fuck performance I guess
    public void CheckEnemy()
    {
        if (enemyCheckCooldown.IsCooldown())
        {
            return;
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].IsDead())
            {
                continue;
            }
            enemies.RemoveAt(i);
            Debug.Log("Great job! " + enemies.Count + " left! ");
            i--;
        }
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
    public void EnemyCheck()
    {

    }
    void WaitWaveClear()
    {
        WaveState prevState = waveState;
        // Debug.Log(enemies.Count);
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
                // Debug.Log("spawned meowww");
                Enemy enemy = enemyToSpawn.enemy;
                if (enemy == null)
                {
                    continue;
                }
                float spawnX = UnityEngine.Random.Range(transform.position.x - renderer.bounds.size.x / 2, transform.position.x + renderer.bounds.size.x / 2);
                float spawnY = transform.position.y + 20;
                float spawnZ = UnityEngine.Random.Range(transform.position.z - renderer.bounds.size.z / 2, transform.position.z + renderer.bounds.size.z / 2);
                Vector3 position = new Vector3(spawnX, spawnY, spawnZ);
                GameObject gameObject = Instantiate(enemy.gameObject, position, Quaternion.identity);
                enemies.Add(gameObject.GetComponent<Enemy>());
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
