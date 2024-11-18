using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawn : MonoBehaviour {
    public Fruit prefab;
    public float intialDelay;
    public float period;
    public float screenRange;

    void CreateFruit() {
        var position = transform.position + Vector3.up * Random.Range(0, screenRange);
        Fruit good = Instantiate(prefab, position, Quaternion.identity);
    }

    void Activate() {
        InvokeRepeating("CreateFruit", intialDelay, period); 
    }
}
