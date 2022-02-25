using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Author: Kennan
* Date: Feb.24.2021
* Summary: A generalized melee attack behaviour. Provides an Attack() function, which when run will attempt a melee attack in the given direction. (WIP)
*/

//TODO: Make canDamagePlayer, canDamageTower, canDamageEnemy automatically filled based on the tag of this object (Player, Tower, Enemy).

public class MeleeAttackBehaviour : MonoBehaviour
{
    public bool canAttack = true;
    public bool canDamagePlayer = false;
    public bool canDamageTower = false;
    public bool canDamageEnemy = false;
    [Tooltip("How much damage the attack should do")]
    public int attackDamage = 25; // This is used by whatever is getting attacked to see how much damage to take
    [Tooltip("(Roughly) The number of Unity units away the attack can reach")]
    public float attackRange = 0.5f;
    [Tooltip("The number of seconds the attack should do damage for")]
    public float attackTime = 0.2f;
    [Tooltip("The number of seconds after an attack before can attack again")]
    public float attackCooldown = 0.3f;
    [Tooltip("The knockback multiplier of the attack")]
    public float attackKnockback = 1.0f;
    [Tooltip("The attack prefab to use")]
    public GameObject meleeAttack; //Reference to player melee attack prefab (TODO: POTENTIALLY JUST HAVE A FIELD FOR THE ATTACK ANIMATION INSTEAD, AS THE ATTACK OBJECT ITSELF COULD BE GENERATED ON THE SPOT, AS ITS JUST A GAMEOBJECT WITH A COLLIDER)
    [Tooltip("The sprite to use for the attack")]
    public Sprite attackImage; //TODO: THIS IS GONNA END UP BEING SOME KIND OF ANIMATION IN THE FUTURE. NOT SURE HOW THAT WORKS OUT. SAME ANIMATION FOR ALL SPRITES? OR JUST A UNIVERSAL ANIMATION THAT WE CHANGE THE COLOUR OF?

    private GameObject attackObj; //Internal ref to the attack collider gameobject
    private GameObject pivotObj;
    private bool attacking = false;
    private float sustainTime = 0.0f; //Internal; counter for how long until the attack should be removed again
    private float cooldownTime = 0.0f; //Internal; counter for how long until an attack can happen again
    private float startRot;
    private Vector2 startPos;
    private Vector2 pointDir;

    // Update is called once per frame
    void Update()
    {
        //Count down timers
        if (cooldownTime > 0.00f) cooldownTime -= Time.deltaTime;
        if (sustainTime > 0.00f) sustainTime -= Time.deltaTime;

        UpdateAnimation();

        CheckEndAttack();
    }//Update()

    
    private void UpdateAnimation()
    {//MANUALLY enacts the animation. Its disgusting but it was the quickest way I could think of

        if (attacking && attackObj != null)
        {
            var curPos = pivotObj.transform.position;
            var curRot = pivotObj.transform.eulerAngles;
            float animProg = Mathf.Clamp(2*(sustainTime/attackTime)-1, 0.0f, 1.0f);
            
            curRot.z = startRot-45f + 90f*animProg; //90° swing range

            //curPos.x = startPos.x - ((0.5f*animProg-0.25f)*pointDir.x);
            //curPos.y = startPos.y - ((0.5f*animProg-0.25f)*pointDir.y);

            pivotObj.transform.eulerAngles = curRot;
            pivotObj.transform.position = curPos;
            //print(Mathf.Max(0f, sustainTime/attackTime));
        }
    }//UpdateAnimation()


    private void CheckEndAttack()
    {// Terminates the attack if its sustain time is up
        if (sustainTime <= 0.00f && attacking) {
            Destroy(pivotObj);
            Destroy(attackObj);
            cooldownTime = attackCooldown;
            attacking = false;
        }//if
    }//CheckEndAttack()

    public void Attack(Vector2 direction)
    {//Attempts to make a melee attack.
        bool isKnockback = false;
        try {
            isKnockback = gameObject.GetComponent<DamageableBehaviour>().isKnockback;
        } catch {
            isKnockback = false;
        }
        if (canAttack && !attacking && cooldownTime <= 0.00f && !isKnockback)
        {
            //Begin attack
            attacking = true;
            sustainTime = attackTime;
            
            //Figure out location to place attack collider
            Vector3 playerPos = this.gameObject.transform.position;
            Vector2 attackPos = new Vector2(playerPos.x, playerPos.y);
            Vector2 scaling = new Vector2(attackRange, attackRange);
            Vector3 pointingDirection = new Vector3(0.0f, 0.0f, Vector2.SignedAngle(Vector2.up, direction.normalized));
            pointDir = direction.normalized;

            //Instantiate and scale collider object
            pivotObj = new GameObject("pivot");
            pivotObj.transform.SetParent(this.transform);
            pivotObj.transform.position = attackPos;
            pivotObj.AddComponent<SpriteRenderer>();
            attackObj = Instantiate(meleeAttack, attackPos + direction.normalized*scaling*2, Quaternion.identity, pivotObj.transform); //Spawn as child to self
            pivotObj.transform.localScale = scaling;
            attackObj.transform.eulerAngles = pointingDirection;
            attackObj.GetComponent<SpriteRenderer>().sprite = attackImage;
            startRot = pivotObj.transform.eulerAngles.z;
            startPos = attackPos;
            //Tell the attack what it can do
            Attack attack = attackObj.GetComponent<Attack>();
            attack.canDamageEnemy = canDamageEnemy;
            attack.canDamageTower = canDamageTower;
            attack.canDamagePlayer = canDamagePlayer;
            attack.damage = attackDamage;
            attack.knockback = attackKnockback;

            //Immediately update the animation
            UpdateAnimation();
        }//if
    }//Attack(Vector2)

    public void LoadMeleeInfo(MeleeInfo info)
    {//Loads the values from a given MeleeInfo into this behaviour
        attackDamage = info.attackDamage;
        attackRange = info.attackRange;
        attackTime = info.attackTime;
        attackCooldown = info.attackCooldown;
        attackImage = info.attackImage;
        attackKnockback = info.attackKnockback;
    }//LoadMeleeInfo(MeleeInfo)

}//MeleeAttackBehaviour