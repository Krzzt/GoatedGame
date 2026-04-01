using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private List<List<GameObject>> enemiesToSpawn = new List<List<GameObject>>();
    private int currentWave = 0;
    private int waveAmount;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool doneSpawning = false;


    private List<GameObject> spawnPoints = new List<GameObject>();

    private List<GameObject> currentEnemyList = new List<GameObject>();

    public static event Action<int> NewEnemiesRemaining;
    public static event Action<int, int> NewWaveText;
    public static event Action LastWave;
    void Start()
    {
        InvokeRepeating("CheckForNextWave",0,0.2f); //checks for next wave / end of room every .2 seconds
    }
    private void OnEnable()
    {
        RoomScript.StartWaves += GenerateWaves;
        Enemy.enemyDies += RemoveEnemy;
        GameManager.currRoomChanged += NewSpawnPoints;
        LayerManager.newLayer += NewEnemyList;
        RoomScript.SpawnBoss += SpawnBoss;
    }
    private void OnDisable()
    {
        RoomScript.StartWaves -= GenerateWaves;
        Enemy.enemyDies -= RemoveEnemy;
        GameManager.currRoomChanged -= NewSpawnPoints;
        LayerManager.newLayer -= NewEnemyList;
        RoomScript.SpawnBoss -= SpawnBoss;
    }

    public void NewEnemyList()
    {
        currentEnemyList = LayerManager.GetEnemyListFromLayer(); //call by reference!
    }

    public void CheckForNextWave()
    {
        if (GameManager.currentRoom.State == Enums.RoomState.Uncleared && enemiesToSpawn[currentWave].Count <= 0 && aliveEnemies.Count <= 0) //if there are no enemies to spawn and none are alive
        {
            NextWave(); //start the next Wave
        }
        if (aliveEnemies.Count == 0 && GameManager.currentRoom.State == Enums.RoomState.Uncleared && doneSpawning) //and the room ur currently in is not cleared and the spawning is finished
        {
           GameManager.currentRoom.ClearRoom();
        }
    }
    public void RemoveEnemy(GameObject enemyToRemove) //gets called as an event every time an enemy dies
    {
        aliveEnemies.Remove(enemyToRemove);
        NewEnemiesRemaining?.Invoke(aliveEnemies.Count);
    }
    public void GenerateWaves(int budget)
    {
            currentWave = 0; //reset waves as this gets called every new room via an event (0 because nextWave does currentwave++)
            enemiesToSpawn.Clear(); //clear previous possible EnemySpawns
            List<Enemy> enemyScriptList = new List<Enemy>();
            foreach (GameObject enemy in currentEnemyList)
            {
                enemyScriptList.Add(enemy.GetComponent<Enemy>()); //get every enemy component from the GameObjects
            }
            waveAmount = Random.Range(1, 4); //between 1 and 3 waves (currently placeholder)
            budget = (int)(budget * (0.8f + (waveAmount / 5f)));
            //1 Wave = budget * 1,
            //2 Waves = budget * 1.2
            //3 Waves = budget * 1.4
            //...
            enemiesToSpawn.Add(new List<GameObject>()); //for good measure
            for (int i = 1; i <= waveAmount; i++) //for every wave
            {
                enemiesToSpawn.Add(new List<GameObject>());
                List<GameObject> possibleEnemies = new List<GameObject>(currentEnemyList); //fuck you call by reference (we need to set this as a new list with the old one as a parameter to prevent call by reference)
                int currentWaveBudget = budget / waveAmount; //if there is a remainder, it just gets killed
                while (possibleEnemies.Count > 0)
                {
                    int randomIndex = Random.Range(0, possibleEnemies.Count); //get a random index from every possible enemy
                    if (currentWaveBudget < possibleEnemies[randomIndex].GetComponent<Enemy>().Cost) //if we cant afford it
                    {
                        possibleEnemies.RemoveAt(randomIndex); //remove this enemy from possibleEnemies
                    }
                    else //if we can afford it
                    {
                        enemiesToSpawn[i].Add(possibleEnemies[randomIndex]);
                        currentWaveBudget -= possibleEnemies[randomIndex].GetComponent<Enemy>().Cost;
                        //subtract the cost and add the Enemy to EnemiesToSpawn
                    }
                }
            }
            NextWave(); //Start the Waves by calling "NextWave"
    }

    public void NextWave()
    {
        if (currentWave < waveAmount) //if the wave should spawn (because it exists)
        {
            currentWave++;
            doneSpawning = false;
            NewWaveText?.Invoke(currentWave, waveAmount);
            StartWaveSpawn();
        }
        else //if we are done spawning
        {
            doneSpawning = true;
            LastWave?.Invoke();
        }

    }

    private void StartWaveSpawn()
    {
        StartCoroutine(waitForEnemySpawn());
    }

    public IEnumerator waitForEnemySpawn()
    {
            while (enemiesToSpawn[currentWave].Count > 0) //as long as there are enemies remaining
            {
                SpawnEnemy();
                yield return new WaitForSeconds(Random.Range(100,201)/ 100f); //wait between 1 to 2 seconds then spawn another enemy
            }
    }

    public void SkipWave()
    {
        while (enemiesToSpawn[currentWave].Count > 0)
        {
            SpawnEnemy();
        }
        NextWave();
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemiesToSpawn[currentWave][0]);
        enemiesToSpawn[currentWave].RemoveAt(0);
        aliveEnemies.Add(newEnemy);
        NewEnemiesRemaining?.Invoke(aliveEnemies.Count);
        SetEnemyPos(newEnemy);
    }
    public void SpawnEnemy(GameObject enemy) //if we want to Spawn a specific enemy
    {        
        GameObject newEnemy = Instantiate(enemy);
        aliveEnemies.Add(newEnemy);
        NewEnemiesRemaining?.Invoke(aliveEnemies.Count);
        SetEnemyPos(newEnemy);
    }

    public void SetEnemyPos(GameObject newEnemy)
    {
        Camera mainCam = Camera.main;
        float maxX = mainCam.pixelWidth;
        float maxY = mainCam.pixelHeight;
        do
        {
            newEnemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        } while ((mainCam.WorldToScreenPoint(newEnemy.transform.position).x < maxX && mainCam.WorldToScreenPoint(newEnemy.transform.position).x > 0) && (mainCam.WorldToScreenPoint(newEnemy.transform.position).y < maxY && mainCam.WorldToScreenPoint(newEnemy.transform.position).y > 0));

    }
    public void NewSpawnPoints() //call by reference :)
    {
        spawnPoints.Clear();
        spawnPoints = GameManager.currentRoom.Spawnpoints;
    }

    public void SpawnBoss()
    {
        List<GameObject> bosses = new List<GameObject>(LayerManager.GetBossListFromLayer());
        GameObject bossPrefab = bosses[Random.Range(0,bosses.Count)];
        GameObject newBoss = Instantiate(bossPrefab);
        aliveEnemies.Add(newBoss);
        doneSpawning = true;
        SetEnemyPos(newBoss);
        //for now a random position but we maybe change it later
    }

}
