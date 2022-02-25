using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

/*
* Author: Joshua
* Date: Apr.7.2021 (Kennan edit)
*/
[System.Serializable] public struct LootTableItem {
    public GameObject prefab;
    public int amount;
    public float probability;
}
public class DamageableBehaviour : MonoBehaviour
{
    public int health = 50;
    [HideInInspector] public int maxHealth;
    [Tooltip("A percent of incoming damage to be ignored 100% makes this object invincible.")]
    public float defense = 0.00f;
    [Tooltip("MUST NOT BE 0. Determines how much this object gets knocked-back; higher is less. Negative values mean no knockback.")]
    public float weight = -1; //negative value means no knockback
    private Vector2 knockback;
    private float knockbackTime = 0.0f;
    public bool isKnockback = false;
    private GameManager gameManager;
    // Start is called before the first frame update
    public GameObject healthBar;
    private GameObject hbarInstance;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private float yOffset;
    private float xOffset;
    //public GameObject[] dropPrefabs;
    [Tooltip("Each element represents an item.")]
    public LootTableItem[] dropList;

    //Define callbacks
    public delegate void OnDamage();
    public OnDamage onDamageCallback; //Callback for when damaged

    
    void Start()
    {
    }//Start()

    void Awake() {
        maxHealth = health;
        yOffset = GetComponent<SpriteRenderer>().bounds.size.y;
        xOffset = GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
        Vector3 healthPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0.0f);
        hbarInstance = Instantiate(healthBar, healthPosition, Quaternion.identity, this.transform);
        hbarInstance.GetComponent<SpriteRenderer>().size = new Vector2(2.4f,0.32f);
        hbarInstance.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
    }//Awake()

    void dropItems() {
        foreach(LootTableItem drop in dropList) {
            GameObject dropPrefab = drop.prefab;
            for (int i = 0; i < drop.amount; i++) {
                float chance = Random.value;
                if (chance < drop.probability) {
                    Instantiate(dropPrefab, (Vector2)transform.position + Random.insideUnitCircle, Quaternion.identity);
                }
            }
        }
    }

    public void setHealth(int newHealth) {
        health = newHealth;
    }

    public void takeDamage(int damage) {
        health -= (int)(damage*(1.0-defense));
        if (onDamageCallback != null) onDamageCallback.Invoke();
        if (health <= 0) {
            dropItems();
            SoundManager.instance.PlaySpatial(deathSound, this.gameObject.transform.position);
            Destroy(this.gameObject);
        } else {
            SoundManager.instance.PlaySpatial(hurtSound, this.gameObject.transform.position);
        }
        hbarInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2((2.4f * ((float)health/(float)maxHealth)),0.32f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        bool takenDamage = false;
        int damage;
        Attack attack = other.gameObject.GetComponent<Attack>();
        if (attack != null) {
            //get the damage attrribute
            damage = attack.damage;
            if (attack.canDamagePlayer && this.tag == "Player") {
                takenDamage = true;
                takeDamage(damage);
            } else if (attack.canDamageEnemy && this.tag == "Enemy") {
                takenDamage = true;
                takeDamage(damage);
            } else if (attack.canDamageTower && this.tag == "Tower") {
                takenDamage = true;
                takeDamage(damage);
            } else if ((attack.canDamagePlayer || attack.canDamageEnemy || attack.canDamageTower) && (this.tag == "Decoration")) {
                takenDamage = true;
                takeDamage(damage);
            }
            // knockback
            if (takenDamage && weight > 0) {
                if (other.gameObject.tag == "Projectile") {
                    knockback = ((Vector2)other.transform.right).normalized * (1.0f / weight) * attack.knockback;
                } else {
                    knockback = -(other.transform.position - this.transform.position).normalized * (1.0f / weight) * attack.knockback;
                }
                knockbackTime = 0.2f;
            }
            // destroy if projectile, 
            if (takenDamage) {
                if (other.gameObject.tag == "Projectile") {
                    Destroy(other.gameObject);
                } else if(other.gameObject.tag == "ExplodingProjectile") {
                    other.gameObject.GetComponent<ExplodingBullet>().Explode();
                } else {
                    Destroy(other);
                }
            }
        }
    }

    void FlashColour() {
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        if (rend.color == Color.white) {
            rend.color = Color.red;
        } else {
            rend.color = Color.white;
        }
    }

    void Update() {
        if (knockbackTime > 0) {
            isKnockback = true;
            knockbackTime -= Time.deltaTime;
            transform.Translate(knockback * 0.05f);
            FlashColour();
        } else if (isKnockback) {
            isKnockback = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
