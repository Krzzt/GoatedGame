using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<List<GameObject>> enemiesToSpawn = new List<List<GameObject>>();
    private int currentWave = 0;
    private int waveAmount;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool doneSpawning = false;
    private GameObject skipButton;
    private TMP_Text waveText;
    private TMP_Text enemiesAliveText;
    private List<GameObject> spawnPoints = new List<GameObject>();

    private void Awake()
    {
        skipButton = GameObject.FindWithTag("SkipWaveButton");
        skipButton.SetActive(false);

        waveText = GameObject.FindWithTag("WaveText").GetComponent<TMP_Text>(); //i prefer tags since they dont change as often as names
        waveText.gameObject.SetActive(false);
        enemiesAliveText = GameObject.FindWithTag("EnemiesAliveText").GetComponent<TMP_Text>();
        enemiesAliveText.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        RoomScript.SendEnemyList += GenerateWaves;
        Enemy.enemyDies += RemoveEnemy;
        GameManager.currRoomChanged += NewSpawnPoints;
    }
    private void OnDisable()
    {
        RoomScript.SendEnemyList -= GenerateWaves;
        Enemy.enemyDies -= RemoveEnemy;
        GameManager.currRoomChanged -= NewSpawnPoints;
    }

    private void FixedUpdate()
    {
        if (GameManager.currentRoom.State == Enums.RoomState.Uncleared && enemiesToSpawn[currentWave].Count <= 0 && aliveEnemies.Count <= 0) //if there are no enemies to spawn and none are alive
        {
            NextWave(); //start the next Wave
        }
        if (aliveEnemies.Count == 0 && GameManager.currentRoom.State == Enums.RoomState.Uncleared && doneSpawning) //and the room ur currently in is not cleared and the spawning is finished
        {
           GameManager.currentRoom.ClearRoom();
            waveText.gameObject.SetActive(false);
            enemiesAliveText.gameObject.SetActive(false);
        }
    }
    public void RemoveEnemy(GameObject enemyToRemove) //gets called as an event every time an enemy dies
    {
        aliveEnemies.Remove(enemyToRemove);
        enemiesAliveText.SetText("Enemies Remaining: " + aliveEnemies.Count);
    }
    public void GenerateWaves(List<GameObject> enemyList, int budget)
    {
            skipButton.SetActive(true);
            enemiesAliveText.gameObject.SetActive(true);
            enemiesAliveText.SetText("Enemies Remaining: 0");
            waveText.gameObject.SetActive(true);
            //Set everything active
            currentWave = 0; //reset waves as this gets called every new room via an event (0 because nextWave does currentwave++)
            enemiesToSpawn.Clear(); //clear previous possible EnemySpawns
            List<Enemy> enemyScriptList = new List<Enemy>();
            foreach (GameObject enemy in enemyList)
            {
                enemyScriptList.Add(enemy.GetComponent<Enemy>()); //get every enemy component from the GameObjects
            }
            waveAmount = Random.Range(1, 4); //between 1 and 3 waves (currently placeholder)
            budget = (int)(budget * (0.8f + (waveAmount / 5f)));
            //1 Wave = budget * 1,
            //2 Waves = budget * 1.2
            //3 Waves = budget * 1.4
            //...
            enemiesToSpawn.Add(new List<GameObject>());
            for (int i = 1; i <= waveAmount; i++) //for every wave
            {
                enemiesToSpawn.Add(new List<GameObject>());
                List<GameObject> possibleEnemies = new List<GameObject>(enemyList); //fuck you call by reference (we need to set this as a new list with the old one as a parameter to prevent call by reference)
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
            waveText.SetText("Wave " + currentWave + "/" + waveAmount); //only set this now because we want the new number (after the ++)
            StartWaveSpawn();
        }
        else //if we are done spawning
        {
            doneSpawning = true;
            skipButton.SetActive(false);
            //Hide the button to skip waves
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
        enemiesAliveText.SetText("Enemies Remaining: " + aliveEnemies.Count);
        newEnemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
    }

    public void NewSpawnPoints(RoomScript currRoom) //call by reference :)
    {
        spawnPoints.Clear();
        spawnPoints = currRoom.Spawnpoints;
        //the existence of currRoom is theoretically not necessary here, but an event has to give a variable (as far as i know) so why not
    }

}
