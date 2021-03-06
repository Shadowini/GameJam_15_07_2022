using System;
using System.Collections;
using System.Collections.Generic;
using Codetox.Variables;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave
{
    public int numEnemies;
    public float waveDuration;

    public Wave(int numEnemies, float waveDuration)
    {
        this.numEnemies = numEnemies;
        this.waveDuration = waveDuration;
    }
}

public class WaveManager : MonoBehaviour
{
    public Transform[] groundPoints;
    public Transform[] shootingPoints;
    public Transform[] skyPoints;
    public Wave wave;
    public GameObject[] typesEnemies;
    public GameObject[] spikes;
    public int totalSpikes;
    public float spikeTime;
    public Variable<float> timeIncrease;
    public Variable<int> enemiesIncrease;
    public Variable<float> totalTime;
    public Variable<int> totalWaves;

    void Awake()
    {
        timeIncrease.Value = 15f;
        enemiesIncrease.Value = 5;
        totalTime.Value = 0.0f;
        totalWaves.Value = 0;
        SpawnWave();
    }

    void Update()
    {
        totalTime.Value += Time.deltaTime;
        
        if (spikeTime > 0)
        {
            spikeTime -= Time.deltaTime;
        }
        else
        {
            foreach (GameObject spike in spikes)
            {
                spike.SetActive(false);
                
            }
            totalSpikes = Random.Range(0, spikes.Length);
            for (int i = 0; i < totalSpikes; i++)
            {
                spikes[i].SetActive(true);
            }
            spikeTime = Random.Range(0, 10);
        }
        
        if (wave.waveDuration <= 0.0f)
        {
            SpawnWave();
        }
        else
        {
            wave.waveDuration -= Time.deltaTime;
        }
    }

    void SpawnWave()
    {
        wave = new Wave(enemiesIncrease.Value, timeIncrease.Value);
        enemiesIncrease.Value += 5;
        totalWaves.Value += 1;
        if (totalTime.Value < 90)
        { 
            var randomPoint3 = skyPoints[Random.Range(0, skyPoints.Length)];
            Instantiate(typesEnemies[2], randomPoint3.position, randomPoint3.rotation);
            StartCoroutine(SpawnEnemiesBefore3());
        }
        else
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemiesBefore3()
    {
        for (int i = 0; i < wave.numEnemies-1; i++)
        {
            GameObject enemy = typesEnemies[Random.Range(0, typesEnemies.Length-1)];
            switch (enemy.tag)
            {
                case "melee":
                    var randomPoint = groundPoints[Random.Range(0, groundPoints.Length)];
                    Instantiate(enemy, randomPoint.position, randomPoint.rotation);
                    break;
                case "shooter":
                    var randomPoint2 = shootingPoints[Random.Range(0, shootingPoints.Length)];
                    Instantiate(enemy, randomPoint2.position, randomPoint2.rotation);
                    break;
            }
            yield return new WaitForSeconds(wave.waveDuration / (2 * wave.numEnemies));
        }
    }
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < wave.numEnemies; i++)
        {
            GameObject enemy = typesEnemies[Random.Range(0, typesEnemies.Length)];
            switch (enemy.tag)
            {
                case "melee":
                    var randomPoint = groundPoints[Random.Range(0, groundPoints.Length)];
                    Instantiate(enemy, randomPoint.position, randomPoint.rotation);
                    break;
                case "shooter":
                    var randomPoint2 = shootingPoints[Random.Range(0, shootingPoints.Length)];
                    Instantiate(enemy, randomPoint2.position, randomPoint2.rotation);
                    break;
                case "flying":
                    var randomPoint3 = skyPoints[Random.Range(0, skyPoints.Length)];
                    Instantiate(enemy, randomPoint3.position, randomPoint3.rotation);
                    break;
            }
            yield return new WaitForSeconds(wave.waveDuration / (2 * wave.numEnemies));
        }
    }
}