/*
* Author: Joshua
* Date: Mar.17.2021
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackBehaviour : MonoBehaviour
{
    public bool canAttack = true;
    public bool canDamagePlayer = false;
    public bool canDamageTower = false;
    public bool canDamageEnemy = false;
    [Tooltip("How much damage the attack should do")]
    public int attackDamage = 25; // This is used by whatever is getting attacked to see how much damage to take
    [Tooltip("The number of seconds after an attack before can attack again")]
    public float attackCooldown = 0.3f;
    [Tooltip("The projectile prefab to use")]

    public AudioClip shootSound;
    public GameObject projectile; //Reference to projectile
    public float attackKnockback = 1.0f;
    private GameObject attackObj;
    private float cooldownTime = 0.0f; //Internal; counter for how long until an attack can happen again
    // Start is called before the first frame update
    private bool isInventoryItem = false;
    private string itemName;
    private InventoryManager inventory;
    private SoundManager sound;
    private bool throwsItself = false;
    private Sprite itemSprite;
    void Start()
    {
        try {
            inventory = InventoryManager.instance;
        } catch {
            Debug.LogWarning("WARNING: RangedAttackBehaviour could not find reference to InventoryManager. Ensure the InventoryManager exists in this scene.");
        }
        try {
            sound = SoundManager.instance;
        } catch {
            Debug.LogWarning("WARNING: RangedAttackBehaviour could not find reference to SoundManager. Ensure the SoundManager exists in this scene.");
        }
    }

    void Update()
    {
        //Count down timers
        if (cooldownTime > 0.00f) cooldownTime -= Time.deltaTime;
    }//Update()

    public bool Attack(Quaternion direction)
    {//Attempts to make a melee attack.
        bool isKnockback = false;
        try {
            isKnockback = gameObject.GetComponent<DamageableBehaviour>().isKnockback;
        } catch {
            isKnockback = false;
        }
        Vector3 playerPos = this.gameObject.transform.position;
        Vector2 attackPos = new Vector2(playerPos.x, playerPos.y);
        if (canAttack && cooldownTime <= 0.00f && !isKnockback) {
            if (isInventoryItem) {
                if (inventory.HasItem(itemName) > 0) {
                    // make a projectile
                    projectile = new GameObject("rangedProj");
                    projectile.AddComponent<SpriteRenderer>();
                    projectile.GetComponent<SpriteRenderer>().sprite = itemSprite;
                    projectile.AddComponent<Rigidbody2D>();
                    projectile.GetComponent<Rigidbody2D>().isKinematic = true;
                    projectile.AddComponent<BoxCollider2D>();
                    projectile.GetComponent<BoxCollider2D>().isTrigger = true;
                    projectile.AddComponent<Bullet>();
                    projectile.AddComponent<Attack>();
                    projectile.GetComponent<Attack>().canDamageEnemy = true;
                    projectile.GetComponent<Attack>().damage = attackDamage;
                    projectile.GetComponent<Attack>().knockback = attackKnockback;
                    projectile.tag = "Projectile";
                    projectile.transform.position = attackPos;
                    projectile.transform.rotation = direction;
                    //sound.PlayOneShot(shootSound, 1.0f);
                    sound.PlaySpatialOnObject(shootSound, this.gameObject);

                    cooldownTime = attackCooldown; //Need to reset cooldownTime before next attack
                    // Remove from inventory
                    inventory.RemItem(itemName);
                }
            } else {
                //Figure out location to place attack collider-
                //Instantiate and scale collider object
                attackObj = Instantiate(projectile, attackPos, direction, this.transform); //Spawn as child to self
                //Tell the attack what it can do
                Attack attack = attackObj.GetComponent<Attack>();
                attack.canDamageEnemy = canDamageEnemy;
                attack.canDamageTower = canDamageTower;
                attack.canDamagePlayer = canDamagePlayer;
                attack.damage = attackDamage;
                attack.knockback = attackKnockback;
                sound.PlaySpatialOnObject(shootSound, this.gameObject);

                cooldownTime = attackCooldown; //Need to reset cooldownTime before next attack
            }//if
            return true;
        } else {
            return false;
        }
    }//Attack(Vector2)

    public void LoadRangedInfo(RangedInfo rangedInfo, ItemInfo itemInfo)
    {//Loads the values from a given RangedInfo into this behaviour
        attackDamage = rangedInfo.attackDamage;
        attackCooldown = rangedInfo.attackCooldown;
        attackKnockback = rangedInfo.attackKnockback;
        isInventoryItem = true;
        throwsItself = rangedInfo.throwable;
        if (throwsItself) {
            itemName = itemInfo.itemName;
            itemSprite = rangedInfo.itemSprite;
        } else {
            itemName = rangedInfo.projectile.GetComponent<ItemInfo>().itemName;
            itemSprite = rangedInfo.projectile.GetComponent<SpriteRenderer>().sprite;
        }
    }//LoadRangedInfo(RangedInfo)
}
