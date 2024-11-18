using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {
    public Boss bossPrefab;

    public void Activate() {
        Invoke("Spawn", 2.0f);
    }

    public void Spawn() {
        var boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        boss.gameObject.SetActive(true);    
    }
}
