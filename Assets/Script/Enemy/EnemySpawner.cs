using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("POOL CONFIG")]
    [SerializeField] int poolSize=1;
    GameObject[] pool;

    [Header("SPAWN CONFIG")]
    [SerializeField] GameObject enemyprefab;
    [SerializeField] float spawnTimer;
    int activeEnemies = 0;
    private void Awake()
    {
       
    }

    private void CreatePool()
    {
        pool = new GameObject[poolSize];
        for(int i=0;i< pool.Length; i++)
        {
            pool[i]=Instantiate(enemyprefab,transform);
            pool[i].SetActive(false);
        }
    }

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTimer);
            EnableObjectInPool();
        }
    }

    private void EnableObjectInPool()
    {
        for(int i=0;i<pool.Length;i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive (true);
                return;
            }
        }
    }
}
