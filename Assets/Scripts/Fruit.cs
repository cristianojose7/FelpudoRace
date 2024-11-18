using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Fruit types
 0 = Cure
 1 = Default 
 2 = Dream
 3 = Plasma
 */

public class Fruit: MonoBehaviour {
    public float speed;
    public int powerUpType;
    public Sprite[] spriteImages = new Sprite[5];
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 10);
        powerUpType = Random.Range(0, spriteImages.Length);
        spriteRenderer.sprite = spriteImages[powerUpType];
    }

    void Update() {         
        transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
    }

    void OnPlayerContact(Player player) {
        player.Heal(1);
        switch (powerUpType) {
            case 0: // Cure
                player.Heal(1);
                break;
            default:
                if (powerUpType <= 3) {
                    player.Upgrade(powerUpType);    
                } else {
                    player.Heal(2);
                }
                break;
        }
    }
}
