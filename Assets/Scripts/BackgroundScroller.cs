using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller: MonoBehaviour {
    public float speed;
    public Player player;
    private float length;

    void Start() {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {
        if (player.started) {
            transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= -length * 2.0f) {
                transform.position = new Vector3(transform.position.x + length * 4.0f, transform.position.y, 0.0f);        
            }
        }
    }
}