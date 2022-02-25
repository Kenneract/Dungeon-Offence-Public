using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Author: Kennan
* Date: Apr.9.2021
* Summary: Placed on player. Manages movement, taking damage, melee/ranged attacks, and tower placement controls FOR THE PLAYER.
*/

// NOTE: USES THE NEW UNITY INPUT SYSTEM

/*
TODO: MAKE THIS SCRIPT JUST HAVE A REFERENCE TO THE PLAYER SPRITE OBJECT, AND THEN AUTOMATICALLY POPULATE THE SPRITERENDERER AND ANIMATOR FIELDS FROM IT.
TODO: RENAME ALL THE "RIGHT" ANIMATIONS TO JUST "SIDE", AS THEYRE NOW MIRRORED TO BE USED FOR BOTH DIRECTIONS
TODO: DIAGONAL MOVEMENT IS SLOWER (OR FASTER RN) THAN IT SHOULD BE
*/



public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public bool frozen = false;
    [Header("Movement")]
    public float speed = 9.0f;
    public float accelerationTime = 0.1f;
    public float decelerationTime = 0.1f;
    public Camera cam; //For where mouse is pointing
    public Animator animator;
    public SpriteRenderer spriteRenderer; //Used for animation

    [Header("Damage")]
    public bool canBeDamaged = true;
    public float invincibilityTime = 0.6f;
    [Tooltip("A percent of incoming damage to be ignored 100% makes player invincible.")]
    [Range(0.0f, 1.0f)]
    public float defense = 0.00f;
    
    [Header("Sound")]
    public AudioClip playerHurt;
    public AudioClip playerStep;
    public float stepInterval = 0.25f;
    private float stepWait = 0.00f;

    private float invincible = 0.00f; //Internal; counter for how long until the player can take damage again
    private MeleeAttackBehaviour meleeAttack;
    private RangedAttackBehaviour rangedAttack;
    private Rigidbody2D rigidBody; //Automatically filled
    private Vector2 moveDirection;
    private Vector2 facing; //Used for animation
    private float velAdd; //The amount of velocity to add per FixedUpdate when moving. Determined by accelerationTime.
    private float velRem; //The amount of velocity to remove per FixedUpdate when no movement given. Determined by decelerationTime.
    private const int FIXED_UPDATES_PER_SECOND = 50; //Assumed 60FPS TODO: FRAMERATE ISNT CONSTANT; CALCULATE THIS SOMEHOW USING TIME.DELTATIME
    private Controls controls; //The player controls (USING NEW UNITY INPUT SYSTEM)
    private GameManager gameManager;
    private Vector2 mousePos;

    private enum ActionTypes {None, Melee, Ranged, Tower}
    private ActionTypes actionType;


    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static PlayerController instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("A PlayerController already exists; deleting self.");
            Destroy(this.gameObject);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

    private void Awake()
    {// Run before Start()
        SetSingleton();
        controls = new Controls();
        rigidBody = GetComponent<Rigidbody2D>();
    }//Awake()

    private void OnEnable()
    {
        controls.Enable();
        controls.Interact.Attack.started += context => OnMainAction(); //Hook into attack button press

        //Grab a reference to self MeleeAttackBehaviour
        try {
            meleeAttack = this.gameObject.GetComponent<MeleeAttackBehaviour>();
        } catch {
            Debug.LogWarning("WARNING: PlayerController could not find reference to own MeleeAttackBehaviour. Ensure the player has a MeleeAttackBehaviour.");
        }//try-catch

        try {
            rangedAttack = this.gameObject.GetComponent<RangedAttackBehaviour>();
        } catch {
            Debug.LogWarning("WARNING: PlayerController could not find reference to own RangedAttackBehaviour. Ensure the player has a MeleeAttackBehaviour.");
        }//try-catch

        try
        {
            //TRY TO Subscribe to hotbar callbacks
            HotbarManager.instance.onDeselectCallback += OnItemDeselect; //Hook into item 
            HotbarManager.instance.onRangedSelectCallback += OnRangedSelect; //Hook into item 
            HotbarManager.instance.onMeleeSelectCallback += OnMeleeSelect; //Hook into item 
            HotbarManager.instance.onTowerSelectCallback += OnTowerSelect; //Hook into item desellect event
        } catch {
        }

    }//OnEnable()
   
    private void Start()
    {// Start is called before the first frame update
        velAdd = speed / (accelerationTime / (1f/FIXED_UPDATES_PER_SECOND)); //Max speed divided across the number of cycles that equal length accelerationTime
        velRem = speed / (decelerationTime / (1f/FIXED_UPDATES_PER_SECOND)); //Max speed divided across the number of cycles that equal length decelerationTime

        //Grab a reference to the GameManager
        try {
            gameManager = GameManager.instance;
        } catch {
            Debug.LogWarning("WARNING: PlayerController could not find reference to GameManager. Ensure the GameManager exists in this scene.");
        }//try-catch

        //Subscribe to armour change callbacks
        InventoryManager.instance.onEquipChangedCallback += OnEquipmentChange; //Hook into armour change event

        //Subscribe to gamemanager menu callbacks
        GameManager.instance.onMenuOpenedCallback += Freeze;
        GameManager.instance.onMenusClosedCallback += Unfreeze;

        //Subscribe to hotbar callbacks if havent already
        HotbarManager.instance.onDeselectCallback += OnItemDeselect; //Hook into item 
	    HotbarManager.instance.onRangedSelectCallback += OnRangedSelect; //Hook into item 
	    HotbarManager.instance.onMeleeSelectCallback += OnMeleeSelect; //Hook into item 
	    HotbarManager.instance.onTowerSelectCallback += OnTowerSelect; //Hook into item desellect event

        HotbarManager.instance.SelectSlot(1); //This really shouldnt need to be done

    }//Start()

    private void OnDisable()
    {
        if (controls != null) controls.Disable();
    }//OnDisable()

    void OnEquipmentChange()
    {//Hooked into callback; run when armour configuration changes
        defense = InventoryManager.instance.GetEquipDefense();
    }//OnEquipmentChange()

    void Freeze()
    {//Hooked into callback; run when any menu is opened
        frozen = true;
    }//InvOpen()
    void Unfreeze()
    {//Hooked into callback; run when all menus are closed
        frozen = false;
    }//InvClose()

    private void TryAttack()
    {
        if (actionType == ActionTypes.Melee && !frozen)
        {
            float mouseClick = controls.Interact.Attack.ReadValue<float>();
            if (mouseClick == 1f) meleeAttack.Attack(facing);
        } else if (actionType == ActionTypes.Ranged && !frozen) {
            float mouseClick = controls.Interact.Attack.ReadValue<float>();
            //RangedAttackBehaviour uses quaternions for some reason
            if (mouseClick == 1f)  {
                float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
                Vector3 newRotation = new Vector3(0,0,angle);
                rangedAttack.Attack(Quaternion.Euler(newRotation));
            }
        }//if
    }//TryAttack()

    private void OnTriggerEnter2D(Collider2D other) 
    {
        //TODO: Could have this set up with the OnTriggerExit2D thing, keep track of if in exit goal with a boolean (still call the GameManager if you enter it, but it would allow for the GameManager to check if you're already standinng in it when the level ends.) 
        //If we just collided with the player exit, notify the GameManager.
        if (other.gameObject.CompareTag("PlayerExit"))
        {
            GameManager.instance.OnPlayerInExit();
        }//if
        
        //If can take damage
        if (canBeDamaged && invincible <= 0.00f) {
            
            //Attempt to get damage object
            Attack attack = other.gameObject.GetComponent<Attack>();
            if (attack != null) {
                // Get attack info
                int damage = attack.damage;

                // If we've been hit
                if (attack.canDamagePlayer) {
                    
                    // Destroy / incapacitate attack
                    if (other.CompareTag("Projectile")) {
                        Destroy(other.gameObject);
                    } else {
                        Destroy(other);
                    }//if (is projectile)

                    // Enact damage
                    invincible = invincibilityTime;
                    gameManager.OnPlayerDamage((int)(damage*(1-defense)));
                    SoundManager.instance.PlayGlobal(playerHurt); //TODO: THIS MAYBE SHOULD BE ON THE GAMEMANAGER INSTEAD

                }//if (damage)
            }//if (attack defined)
        }//if (can be damaged)
    }//OnTriggerEnter2D()

    void OnMainAction()
    {//Hooked into callback; run when player clicks mouse (or whatever the main action button is)

        if (actionType == ActionTypes.Tower && !frozen)
        {//Attempt to place tower

            //Gather current tower info
            var towerPrefab = HotbarManager.instance.GetCurInvSlot().GetPrefab();
            var towerName = HotbarManager.instance.GetCurInvSlot().GetName();
            //Try to place tower
            bool placed = TowerManager.instance.SpawnTower(towerPrefab, mousePos);
            //If placement worked, remove one item from inventory
            if (placed) InventoryManager.instance.RemItem(towerName);
        }//if
    }//OnMainAction()


    private void GetMoveInput()
    {
        //Load movement
        Vector2 moveInput = controls.Movement.Move.ReadValue<Vector2>();
        if (!frozen) {
             moveDirection = moveInput.normalized; //Ceil vector to magnitude 1 to prevent diagonal movements being faster
        } else {
            moveDirection = new Vector2(0,0);
        }

        //Get mouse position
        Vector2 mouseRaw = controls.Interact.Aim.ReadValue<Vector2>();
        mousePos = cam.ScreenToWorldPoint(mouseRaw);
       
        //If we're moving, note down the direction
        if (moveDirection.sqrMagnitude > 0) {
            facing = moveDirection;
        } else {
            //Not moving; point in direction of mouse
            Vector2 lookDir = mousePos - rigidBody.position;
            facing = lookDir;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        }

        //Update animation
        animator.SetFloat("Horizontal",facing.x);
        animator.SetFloat("Vertical",facing.y);
        animator.SetFloat("Speed",moveDirection.sqrMagnitude); //speed needs to be current
        
        spriteRenderer.flipX = (facing.x < 0); //Flip sprite if we're moving left
    }//GetMoveInput()

    private void Update()
    {// Update is called once per frame
        //Reduce times (invincibility, step sound delay, etc)
        if (invincible > 0.00f) invincible -= Time.deltaTime;
        if (stepWait > 0.00f) stepWait -= Time.deltaTime;
        GetMoveInput();
        TryAttack(); //Attack if clicking
    }//Update()

    void MovePlayer()
    {
        // Get current velocity
        Vector2 curVelocity = rigidBody.velocity;

        //PLAY FOOTSTEP SOUNDS
        if (curVelocity.normalized.magnitude > 0.00f)
        {//we're moving
            if (stepWait <= 0.00f)
            {
                SoundManager.instance.PlayGlobal(playerStep, 0.8f, 1.2f);
                stepWait = stepInterval;
            }
        } else {
            stepWait = 0.00f; //Reset if we're not moving
        }//if

        // Determine new velocity
        if (moveDirection.x > 0) {
            //Moving right
            curVelocity.x += velAdd;
        } else if (moveDirection.x < 0) {
            //Moving left
            curVelocity.x -= velAdd;
        } else {
            //Not moving; slow down
            if (curVelocity.x > 0) {
                curVelocity.x -= velRem;
                if (curVelocity.x < 0) curVelocity.x = 0; //Stop the player once velocity swaps directions
            }
            else if (curVelocity.x < 0) {
                curVelocity.x += velRem;
                if (curVelocity.x > 0) curVelocity.x = 0; //Stop the player once velocity swaps directions
            }
        }//end if


        if (moveDirection.y > 0) {
            //Moving up
            curVelocity.y += velAdd; 
        } else if (moveDirection.y < 0) {
            //Moving down
            curVelocity.y -= velAdd; 
        } else {
            //Not moving; slow down
            if (curVelocity.y > 0) {
                curVelocity.y -= velRem;
                if (curVelocity.y < 0) curVelocity.y = 0; //Stop the player once velocity swaps directions
            }
            else if (curVelocity.y < 0) {
                curVelocity.y += velRem;
                if (curVelocity.y > 0) curVelocity.y = 0; //Stop the player once velocity swaps directions
            }
        }//end if

        //Bound velocity to speed (TODO: THIS SHOULD BE CLEANED UP)
        if (curVelocity.x < -speed) curVelocity.x = -speed; //Bound to max speed
        if (curVelocity.x > speed) curVelocity.x = speed; //Bound to max speed
        if (curVelocity.y < -speed) curVelocity.y = -speed; //Bound to max speed
        if (curVelocity.y > speed) curVelocity.y = speed; //Bound to max speed

        // Set new velocity
        //rigidBody.velocity = new Vector2(Mathf.Abs(moveDirection.x) * curVelocity.x, Mathf.Abs(moveDirection.y) * curVelocity.y); //Normalize a second time; would turn to percents. Multiply by speed again.
        rigidBody.velocity = curVelocity;
    }//MovePlayer()

    void FixedUpdate()
    {// Called fixed ammount of times per second (50?) (Physics calculations)
        MovePlayer();
    }//FixedUpdate()

    void OnMeleeSelect()
    {//Callback event; when a melee weapon is selected in the hotbar, load its stats into the players attack and enable it
        //Get current item's data
        var info = HotbarManager.instance.GetCurInvSlot().GetPrefab().GetComponent<MeleeInfo>();

        //Load melee data from it
        meleeAttack.LoadMeleeInfo(info);

        //Enable melee attack
        meleeAttack.canAttack = true;
        rangedAttack.canAttack = false;
        actionType = ActionTypes.Melee;
    }//OnMeleeSelect()

    void OnItemDeselect()
    {//Callback event; disable melee attack, ranged attack, and tower placement.
        //Disable melee attack
        meleeAttack.canAttack = false;
        rangedAttack.canAttack = false;

        actionType = ActionTypes.None;
    }//OnItemDeselect()

    void OnRangedSelect()
    {//Callback event; 
        var rangedInfo = HotbarManager.instance.GetCurInvSlot().GetPrefab().GetComponent<RangedInfo>();
        var itemInfo = HotbarManager.instance.GetCurInvSlot().GetPrefab().GetComponent<ItemInfo>();
        actionType = ActionTypes.Ranged;

        rangedAttack.LoadRangedInfo(rangedInfo, itemInfo);

        meleeAttack.canAttack = false;
        rangedAttack.canAttack = true;
        actionType = ActionTypes.Ranged;
    }//OnRangedSelect()

    void OnTowerSelect()
    {//Callback event;
        actionType = ActionTypes.Tower;
    }//OnTowerSelect()

}//PlayerController
