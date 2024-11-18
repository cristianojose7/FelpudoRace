using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingEnemy: MonoBehaviour {
    public float speed;
    public int health;
    public float blinkCycleSeconds;
    public float maxInvulTime;
    public Player player;
    private float invisibilityTimer;
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 10);
        invisibilityTimer = 0;
    }

    void Update() {         
        transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
        if (invisibilityTimer > 0.0f) {
            invisibilityTimer -= Time.deltaTime;
        }
    }

    public void Hurt(int v) {
        if (health < 0) {
            Kill();
            return;
        }
        if (invisibilityTimer > 0.0f) {
            return;        
        }
        health -= v;
        SetInvisible();
    }

    private void SetInvisible() {
        invisibilityTimer = maxInvulTime;
        SetHurtSprite();
    }

    public void Kill() {
        health = 0;
        Destroy(this.gameObject);
        player.OnEnemyDestroy();
    }

    void SetHurtSprite() {
        spriteRenderer.color = Color.red;
        Invoke("SetNormalSprite", blinkCycleSeconds);
    }

    void SetNormalSprite() {
        spriteRenderer.color = Color.white;
        if (invisibilityTimer > 0) {
            Invoke("SetHurtSprite", blinkCycleSeconds);
        }
    }
}
