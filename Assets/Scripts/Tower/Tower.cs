using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Author: Kevin (Co-Author: Kennan)
//Date: Apr.7.2021
//Purpose: Aims and fires at nearby enemies. Shows HUD on mouse-over for range and remaining ammo. Manages all tower-related stats (health, damage, speed, range)

public class Tower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("MouseOver")]
    public GameObject rangeCircle; //A reference to the gameobject for the range-preview circle
    public float minPlayerDistance = 3.0f; //The min number of unity-units the player has to be from this tower to intereact with it
    public Color hoverColour = Color.red;
    public GameObject ammoBar;
    
    [Space]
    [Header("Firing")]
    public int ammoCapacity = 30;
    [HideInInspector] public int curAmmo;
    [Tooltip("The item prefab for the item to be used as ammo. Doesn't have to be related to the actual ranged attack object.")]
    public GameObject reloadItem;
    [Space]
    [Header("Upgrade Stats")]
    //[Tooltip("How much damage the attack should do")]
    public int startHP;
    public int maxHP;
    public float startRange;
    public float maxRange;
    public int startDamage;
    public int maxDamage;
    public float startCooldown;
    public float minCooldown;
    [Space]
    [Tooltip("The number of increments (levels) to go from startXX to maxXX values.")]
    public int upgradeSteps = 5;
    [Tooltip("The resource required to upgrade stats")]
    public GameObject upgradeResource;
    [Tooltip("How many of the resource is required to upgrade health one level")]
    public int hpResourceReq = 4;
    [Tooltip("How many of the resource is required to upgrade damage one level")]
    public int rangeResourceReq = 4;
    [Tooltip("How many of the resource is required to upgrade range one level")]
    public int damageResourceReq = 4;
    [Tooltip("How many of the resource is required to upgrade speed one level")]
    public int speedResourceReq = 4;
    [Space]
    [HideInInspector] public int hpLvl = 0; //The current health upgrade level.
    [HideInInspector] public int rangeLvl = 0; //The current range upgrade level.
    [HideInInspector] public int damageLvl = 0; //The current damage upgrade level.
    [HideInInspector] public int cooldownLvl = 0; //The current cooldown (speed) upgrade level.
    [HideInInspector] public DamageableBehaviour damageableBehaviour;
    [HideInInspector] public RangedAttackBehaviour rangedAttackBehaviour;
    [HideInInspector] public float curRange;
    [HideInInspector] public GameObject currentTarget;
    private bool hovered = false;
    //private bool hoverInRange = false;
    private Controls controls;
    private MouseCursor mouseCursor;
    private Vector2 mouseLocation;
    private SpriteRenderer rend;

    //Callbacks (only for the TowerMenuManager)
    public delegate void OnInfoChange();
    public OnInfoChange onInfoChangeback; //For when the health/ammo quantity is changed
    public delegate void OnDeath();
    public OnDeath onDeathCallback; //For when the tower is destroyed

    public void SetHealthLvl(int lvl)
    {//ASSUMES LVL IS BETWEEN 0 - upgradeSteps
        int chng = (int)((maxHP - startHP)/upgradeSteps);
        damageableBehaviour.health += chng;
        damageableBehaviour.maxHealth = startHP + (int)((maxHP-startHP) * ((float)lvl/upgradeSteps));
        hpLvl = lvl;
    }//SetHealthLvl(int)
    public void SetRangeLvl(int lvl)
    {//ASSUMES LVL IS BETWEEN 0 - upgradeSteps
        curRange = startRange + (maxRange-startRange) * ((float)lvl/upgradeSteps);
        rangeLvl = lvl;
    }//SetRangeLvl(int)
    public void SetDamageLvl(int lvl)
    {//ASSUMES LVL IS BETWEEN 0 - upgradeSteps
        rangedAttackBehaviour.attackDamage = startDamage + (int)((maxDamage-startDamage) * ((float)lvl/upgradeSteps));
        damageLvl = lvl;
    }//SetDamageLvl(int)
    public void SetSpeedLvl(int lvl)
    {//ASSUMES LVL IS BETWEEN 0 - upgradeSteps
        rangedAttackBehaviour.attackCooldown = startCooldown - ((startCooldown-minCooldown) * ((float)lvl/upgradeSteps));
        cooldownLvl = lvl;
    }//SetSpeedLvl(int)


    
    
    
    



    public void OnPointerEnter(PointerEventData eventData)
	{//Run when the mouse enters this tower
        hovered = true;
    }//OnPointerEnter(PointerEventData)
   
    public void OnPointerExit(PointerEventData eventData)
	{//Run when mouse leavs this tower
        hovered = false;
    }//OnPointerExit(PointerEventData)


    //////////////////////////////////////////////////////
    //                      :/::::::--..`               //
    //                   `.:/:::::::-..`                //
    //                 `-////:::::::-..`                //
    //               `-///////::::::--.                 //
    //             `-://///++/::::::--.                 //
    //           .:/://///++o//:::::--`                 //
    //         `://++//+++++o///::::-.`                 //
    //       .:/+++oso+++oo++///:::::.`                 //
    //       :///o+-` `-+o+++//::::::-`                 //
    //       `////       :////:///:::-`                 //
    //        -/+.      .//////////:::-.                //
    //         -/+/. `-:+/////////////:-.`              //
    //          :++++ss++o//////////--::--`             //
    //          `-//++o+/. `/+/++///-.`:/:-`            //
    //             `.-.`    ://+/ :/:-. .::-`           //
    //                     `://+: `///-  ./:-`          //
    //                     `///+.  -/:-   `:/:`         //
    //                      ///+   `/:-     `.`         //
    //                      ://-    //:`                //
    //                      ///.    ://.                //
    //                     `///`    .//.                //
    //                      //:`     `.`                //
    //                      .:-                         //
    //////////////////////////////////////////////////////
    // HAH GOTCHA

    public void Awake()
    {
        controls = new Controls();
        controls.Interact.Action.started += context => OnInteract(); //Hook into interact button press
        rend = GetComponent<SpriteRenderer>();
    }//Awake()

    private void OnInteract()
    {//Callback event for when user right-clicks.
        if (hovered)
        {
            TowerMenuManager.instance.TryOpen(this.gameObject);
        }//if
    }//OnInteract

    public void Start()
    {
        controls.Enable();
        try {
            mouseCursor = GameObject.Find("MouseCursor").GetComponent<MouseCursor>();
        } catch {
            Debug.LogWarning("WARNING: Enemy could not find reference to MouseCursor. Ensure the MouseCursor exists in this scene.");
        }
        //mouseLocation = mouseCursor.getMouseLocation();

        //Setup starting values
        damageableBehaviour = GetComponent<DamageableBehaviour>();
        rangedAttackBehaviour = GetComponent<RangedAttackBehaviour>();

        //health
        damageableBehaviour.health = startHP;
        damageableBehaviour.maxHealth = startHP;
        //damage & cooldown
        rangedAttackBehaviour.attackDamage = startDamage;
        rangedAttackBehaviour.attackCooldown = startCooldown; 
        //range
        curRange = startRange;
        //ammo
        curAmmo = ammoCapacity;

        //Hook into damage callback
        damageableBehaviour.onDamageCallback += OnDamage;

    }//start

    private void OnDamage()
    {//Hooked into the DamageableBehaviour callback
        if (onInfoChangeback != null) onInfoChangeback.Invoke();
    }//OnDamage

    private void OnDestroy()
    {//Called when tower is destroyed; report to tower manager
        if (onDeathCallback != null) onDeathCallback.Invoke(); //TODO: Make the tower manager just subscribe to all these callbacks innstead of the below line
        TowerManager.instance.TowerExpire(this.gameObject);       
    }//OnDestroy()

    private void UpdateNearestEnemy()
    {
        GameObject currentNearestEnemy = null;
        float distance = Mathf.Infinity;

        foreach (GameObject enemy in EnemyManager.enemyList){
            if (enemy != null){
                float _distance = (transform.position - enemy.transform.position).magnitude;

                if (_distance < distance){
                    distance = _distance;
                    currentNearestEnemy = enemy;
                }//if
            }//if
            
        }//foreach

        if (distance <= curRange){
            currentTarget = currentNearestEnemy;
        }
        else{
            currentTarget = null;
        }//if
    }//UpdateNearestEnemy()

    public bool HaveAmmo()
    {
        return (curAmmo > 0);
    }//HaveAmmo()

    public bool UseAmmo()
    {// Attempts to consume a unit of ammunition. Returns true if successful, false otherwise.
        if (HaveAmmo())
        {
            curAmmo--;
            if (onInfoChangeback != null) onInfoChangeback.Invoke();
            return true;
        } else {
            return false;
        }//if
    }//UseAmmo()

    protected virtual void Shoot(){
        //to be overwritten by children to allow for different types of towers
    }//shoot()

    private void Update(){
        //Target enemies
        UpdateNearestEnemy();
        if (currentTarget != null){
            Shoot();
        }//if

        //Check if mouse is hovering and in range
        if (hovered)
        {
            rend.color = hoverColour;
            rangeCircle.transform.localScale = new Vector3(curRange*2, curRange*2, 1f );
            float ammoBarScale = (float)curAmmo/((float)ammoCapacity*2);
            ammoBar.transform.localScale = new Vector3(ammoBarScale, 0.5f, 1f );
            //Kennan's stuff
            float distance = (transform.position - PlayerController.instance.gameObject.transform.position).magnitude;
            //if (distance <= minPlayerDistance)
            //{
            //    hoverInRange = true;
            //} else {
            //    hoverInRange = false;
            //}//if
            
        } else {
            //hoverInRange = false;

            rend.color = Color.white;
            rangeCircle.transform.localScale = new Vector3(0, 0, 1f );
            ammoBar.transform.localScale = new Vector3(0, 0, 1f );
        }//if

    }//Update()

}//Tower