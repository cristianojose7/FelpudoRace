using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Projectile types
 1 = Default 
 2 = Dream
 3 = Plasma
 */

public class Projectile : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public float bulletSpeed;
    public float plasmaSpeed;
    public Color defaultColor;
    public Color dreamColor;
    public Color plasmaColor;
    public int bulletType;
    public float direction;
    public float dreamHeight;
    private SpriteRenderer spriteRenderer;
    private float timer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>(); 
        Destroy(this.gameObject, 20);
        timer = 0.0f;
        switch (this.bulletType) {
            case 1:
                spriteRenderer.color = defaultColor;   
                break;
            case 2:
                spriteRenderer.color = dreamColor;   
                break;
            case 3:
                spriteRenderer.color = plasmaColor;   
                break;
            default:
                spriteRenderer.color = defaultColor;  
                break;
        }
    }

    void Update() {   
        timer += Time.deltaTime;      
        if (bulletType == 2) {
            rigidBody.velocity = new Vector2(bulletSpeed, 0.0f);
        } else if (bulletType == 3) { 
            rigidBody.velocity = new Vector2(plasmaSpeed, direction * dreamHeight * Mathf.Cos(20 * timer));
        } else {
            rigidBody.velocity = new Vector2(bulletSpeed, 0.0f);        
        }
    }

    public void SetPlasma(float dir) {
        direction = dir;
        bulletType = 3;
    }

    public void SetDream() {
        bulletType = 2;
    }

    public void SetDefault() {
        bulletType = 1;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.SendMessage("Hurt", bulletType);
            Destroy(this.gameObject);
        }
    }
}
