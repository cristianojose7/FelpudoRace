using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss: MonoBehaviour {
    public int health;
    public float blinkCycleSeconds;
    public float maxInvulTime;
    public Player player;
    public float jumpSpeed;
    public BossProjectile projectilePrefab;
    public float shootTime;
    public float jumpTime;
    public float initialShootTime;
    public float initialJumpTime;
    public ParticleSystem deathParticleSystem;
    private float timer;
    private float invisibilityTimer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        invisibilityTimer = 0;
        timer = 0.0f;
        InvokeRepeating("Shoot", initialShootTime, shootTime);
        InvokeRepeating("Jump", initialJumpTime, jumpTime);
    }

    void Update() {         
        timer += Time.deltaTime;
        if (invisibilityTimer > 0.0f) {
            invisibilityTimer -= Time.deltaTime;
        }
    }

    void Jump() {
        rigidBody.velocity = Vector3.up * jumpSpeed;    
    }

    void Shoot() {
        if (!player.gameObject.activeSelf) {
            return;
        }

        BossProjectile proj = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        proj.SendMessage("SetPlayer", player);
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
        deathParticleSystem.gameObject.SetActive(true);
        deathParticleSystem.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        deathParticleSystem.Play();
        Destroy(this.gameObject);
        player.OnBossDestroy();
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
