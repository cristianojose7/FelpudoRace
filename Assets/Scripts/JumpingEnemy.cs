using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy: MonoBehaviour {
    public float horizontalSpeed;
    public int health;
    public float blinkCycleSeconds;
    public float maxInvulTime;
    public Player player;
    public float jumpSpeed;
    public float distanceToPlayer;
    private float distanceBetweenJumps;
    private float invisibilityTimer;
    private float timeBetweenJumps;
    private float jumpingTime;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 10);
        invisibilityTimer = 0;

        // Jumping enemy
        float gravity = 9.81f;
        distanceBetweenJumps = distanceToPlayer / 2.0f - (1.5f * horizontalSpeed * jumpSpeed) / gravity;

        timeBetweenJumps = distanceBetweenJumps / horizontalSpeed;
        jumpingTime = 2.0f * jumpSpeed / gravity;

        InvokeRepeating("Jump", timeBetweenJumps, jumpingTime + timeBetweenJumps);
        SetNormalSprite();
    }

    void Update() {         
        transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
        if (invisibilityTimer > 0.0f) {
            invisibilityTimer -= Time.deltaTime;
        }
        if (transform.position.y <= -3.25) {
            transform.position = new Vector3(transform.position.x, -3.25f, 0.0f);            
        }
    }

    void Jump() {
        rigidBody.velocity = Vector3.up * jumpSpeed;
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
        spriteRenderer.color = Color.green;
        if (invisibilityTimer > 0) {
            Invoke("SetHurtSprite", blinkCycleSeconds);
        }
    }
}
