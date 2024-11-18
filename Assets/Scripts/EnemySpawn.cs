using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public StandingEnemy standingPrefab;
    public FlyingEnemy flyingPrefab;
    public JumpingEnemy jumpingPrefab;
    public float intialDelay;
    public float enemyPeriod;
    public Player player;

    void CreateEnemy() {
        var spawnType = Random.Range(1, 4);
        switch (spawnType) {
            case 1:
                StandingEnemy standingEnemy = Instantiate(standingPrefab, transform.position, Quaternion.identity);
                standingEnemy.player = this.player;
                break;   
            case 2:
                FlyingEnemy flyingEnemy = Instantiate(flyingPrefab, transform.position, Quaternion.identity);
                flyingEnemy.player = this.player;
                break; 
            case 3:
                JumpingEnemy jumpingEnemy = Instantiate(jumpingPrefab, transform.position, Quaternion.identity);
                jumpingPrefab.player = this.player;
                break;  
            default:
                break;        
        }        
    }

    void Activate() {
        InvokeRepeating("CreateEnemy", intialDelay, enemyPeriod);   
    }
}
