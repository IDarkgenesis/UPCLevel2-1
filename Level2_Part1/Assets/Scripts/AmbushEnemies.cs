using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushEnemies : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] bool isAmbushed;
    private void OnTriggerEnter(Collider other)
    {
        if (!isAmbushed)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                    isAmbushed = true;
                }
            }
        }
        

       

        
    }
}
