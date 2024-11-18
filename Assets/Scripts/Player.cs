using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Projectile types
 1 = Default 
 2 = Dream
 3 = Plasma
 */

/**
 * Animation types
 1 = Hurt
 2 = Cure
 */

public class Player : MonoBehaviour {
    public bool started;
    public bool win;
    public int health;
    public int maxHealth;
    public Projectile projectilePrefab;
    public float blinkCycleSeconds;
    public ParticleSystem deathParticleSystem;
    public int projectileType;
    public float timeBetweenDefaultBullets;
    public EnemySpawn enemySpawn;
    public FruitSpawn fruitSpawn;
    public BossSpawn bossSpawn;
    public int points;
    public int phase;
    public Image[] hearts;
    public float winTimer;
    public float jumpSpeed;
    public GameObject winText;
    public GameObject loseText;
    private int animationType;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float invisibilityTimer;
    private float animationTimer;
    private float launchTimer;
    private bool canJump;
    private bool noMoreEnemies;

    void Start () {
        projectileType = 1;
        invisibilityTimer = 0;
        animationTimer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        canJump = true;
        winTimer = 0.0f;
    }

    void StartAllEvents() {
        enemySpawn.SendMessage("Activate");
        fruitSpawn.SendMessage("Activate");
        started = true;
    }

    void Update() {
        for (int i = 0; i < hearts.Length; ++i) {
            if (i < health) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;            
            }
        }   

        if (win) {
            winTimer += Time.deltaTime;
            if (winTimer > 2.0f) {
                rigidBody.AddForce(Vector2.right * 1.5f, ForceMode2D.Impulse);  
            }
            return;
        }

        if (canJump && Input.GetKeyDown("z")) { 
            canJump = false;
            if (!started) {
                StartAllEvents();
            }

            if (rigidBody.velocity.y <= 0.0f) {
                rigidBody.velocity = Vector3.up * jumpSpeed;  
            } 
        }

        // Variable jump height.
        if (!Input.GetKey("z") && rigidBody.velocity.y > jumpSpeed / 2.0f) {
            rigidBody.velocity = Vector3.up * jumpSpeed / 2.0f;  
        }

        // Handle projectiles
        if (projectileType != 1 && Input.GetKeyDown("x")) { 
            if (launchTimer <= 0.0f) {
                launchTimer = timeBetweenDefaultBullets / 2.0f;       
                LaunchProjectile();
            }
        }

        if (projectileType == 1 && Input.GetKey("x")) { 
            if (launchTimer <= 0.0f) {
                launchTimer = timeBetweenDefaultBullets;            
                LaunchProjectile();
            }
        }

        // Handle animations
        if (launchTimer > 0.0f) {
            launchTimer -= Time.deltaTime;
        }

        if (invisibilityTimer > 0.0f) {
            invisibilityTimer -= Time.deltaTime;
        }

        if (animationTimer > 0.0f) {
            animationTimer -= Time.deltaTime;
            if (animationTimer <= 0.0f) {
                animationType = 0;
            }
        }
    }

    void StopAllTimers() {
        animationType = 0;
        animationTimer = 0.0f;
        launchTimer = 0.0f;
        invisibilityTimer = 0.0f;
    }

    void LaunchProjectile() {
        Projectile first = null;
        Projectile second = null;
        switch (this.projectileType) {
            case 2: // Dream 
                first = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                second = Instantiate(projectilePrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
                first.SetDream();
                second.SetDream();   
                break;   
            case 3: // Plasma
                first = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                second = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                first.SetPlasma(1.0f); // Inverted direction
                second.SetPlasma(-1.0f);
                break;   
            default:
            case 1: // Default
                first = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                first.SetDefault();
                break;     
        }
    }

    public void Hurt(int damageAmount) {
        if (invisibilityTimer > 0.0f) {
            return;        
        }
        health -= damageAmount;
        if (health <= 0) {
            Kill();
            return;
        }
        SetInvisible();
    }
    
    void SetCureSprite() {
        spriteRenderer.color = Color.green;
        Invoke("SetNormalSprite", blinkCycleSeconds);
    }

    void SetHurtSprite() {
        spriteRenderer.color = Color.red;
        Invoke("SetNormalSprite", blinkCycleSeconds);
    }

    void SetNormalSprite() {
        spriteRenderer.color = Color.white;
        if (animationTimer > 0) {
            switch (animationType) {
                case 1:
                    Invoke("SetHurtSprite", blinkCycleSeconds);
                    break;
                case 2:
                    Invoke("SetCureSprite", blinkCycleSeconds);
                    break;
                default:
                    break;
            }
        }
    }
    
    public void SetInvisible() {
        invisibilityTimer = 1.0f / 10.0f;
        animationTimer = 1.0f / 10.0f;
        animationType = 1;
        SetHurtSprite();
    }

    public void Kill() {
        if (win) {
            return;
        }
        health = 0;
        deathParticleSystem.gameObject.SetActive(true);
        deathParticleSystem.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        deathParticleSystem.Play(); 
        loseText.SetActive(true);
        if (!noMoreEnemies) {
            Destroy(enemySpawn.gameObject);      
            Destroy(fruitSpawn.gameObject);    
        }  
        this.gameObject.SetActive(false);
        for (int i = 0; i < hearts.Length; ++i) {
            hearts[i].enabled = false;            
        }   
    }

    public void Upgrade(int type) {
        this.projectileType = type;
        animationTimer = 1.0f / 20.0f;
        animationType = 2;
        SetCureSprite();
    }

    public void Heal(int amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        animationTimer = 1.0f / 20.0f;
        animationType = 2;
        SetCureSprite();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            canJump = true;        
        }    
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (win) {
            return;
        }

        if (other.gameObject.CompareTag("Hazard")) {
            Hurt(1);
        }

        if (other.gameObject.CompareTag("Enemy")) {
            Hurt(1);
        }

        if (other.gameObject.CompareTag("InstantDeath")) {
            Kill();
        }

        if (other.gameObject.CompareTag("Fruit")) {
            other.gameObject.SendMessage("OnPlayerContact", this);
            Destroy(other.gameObject);
        }
    }

    public void OnEnemyDestroy() {
        if (noMoreEnemies) {
            return;
        }
        points += 1;
        if (points >= 15) {
            bossSpawn.SendMessage("Activate");
            Destroy(enemySpawn.gameObject);      
            Destroy(fruitSpawn.gameObject);      
            //enemySpawn.SendMessage("Activate"); 
            animationTimer = 1.0f / 20.0f;
            animationType = 2;
            SetCureSprite();
            noMoreEnemies = true;
        }    
    }

    public void OnBossDestroy() {
        win = true;    
        winText.SetActive(true);
        SetNormalSprite();
    }
}
