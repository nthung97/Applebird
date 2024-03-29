﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private float xLimit;
    [SerializeField]
    private float[] xPositions;
    [SerializeField]
    private Wave[] wave;

    private float currentTime;

    List<float> remainingPositions = new List<float>();
    private int waveIndex;
    float xPos = 0;
    int rand;

	// Use this for initialization
	void Start ()
    {
        currentTime = 0;
        remainingPositions.AddRange(xPositions);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GameValues.Difficulty == GameValues.Difficulties.Normal
            || GameValues.Difficulty == GameValues.Difficulties.Challenge)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                SelectWave();
            }
        } 
	}

    void SpawnEnemy(float xPos)
    {
        int r = Random.Range(0, 3); //2 types of enemies
        string enemyName = "";
        if (r == 0) enemyName = "Enemy1";
        else if (r == 1) enemyName = "Enemy2";
        else if (r == 2) enemyName = "Enemy3";

        GameObject enemy = ObjectPooling.instance.GetPooledObject(enemyName);
        enemy.transform.position = new Vector3(xPos, transform.position.y, 0);
        enemy.SetActive(true);
    }

    void SelectWave()
    {
        remainingPositions = new List<float>();
        remainingPositions.AddRange(xPositions);

        waveIndex = Random.Range(0, wave.Length);

        currentTime = wave[waveIndex].delayTime;
        
        if (wave[waveIndex].spawnAmount == 1)
            xPos = Random.Range(-xLimit, xLimit);
        else if (wave[waveIndex].spawnAmount > 1)
        {
            rand = Random.Range(0, remainingPositions.Count);
            xPos = remainingPositions[rand];
            remainingPositions.RemoveAt(rand);
        }

        Debug.Log(remainingPositions.Count);
        Debug.Log(wave[waveIndex].spawnAmount);
        
        for (int i = 0; i < wave[waveIndex].spawnAmount; i++)
        {
            SpawnEnemy(xPos);
            rand = Random.Range(0, remainingPositions.Count);
            xPos = remainingPositions[rand];
            //remainingPositions.RemoveAt(rand);
        }
    }

}

[System.Serializable]
public class Wave
{
    public float delayTime;
    public float spawnAmount;
}