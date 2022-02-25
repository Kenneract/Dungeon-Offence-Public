using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Apr.10

public class Enemy : DamageableBehaviour
{
    [SerializeField]
    private float movementSpeed;
    public float visibilityRange = 1.0f;
    public float attackRange = 1.5f;
    public float runAwayRange = 0.0f;
    public int friendDamage = 25;
    public bool isRanged = false;



    //private int killReward;     //reward for killing the enemy
    [HideInInspector] public int pathfinderIdx = 0;

    private MeleeAttackBehaviour meleeAttack;
    private RangedAttackBehaviour rangedAttack;
    private GameObject[] path;
    private GameObject target;
    private GameObject player;
    private GameObject targetTower;
    private float attackDelay = 0.3f;
    private bool attackState = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Vector3 lastPos;
    private Vector2 facing;
    private bool isMoving = true;


    private void Start()
    {
        path = GameObject.FindGameObjectsWithTag("Path");
        try {
            player = PlayerController.instance.gameObject;
        } catch {
            Debug.LogWarning("WARNING: Enemy could not find reference to Player. Ensure the Player exists in this scene.");
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }//Start()

    private void Awake() {
        //path = GameObject.FindGameObjectsWithTag("Path");
        if (isRanged) {
            rangedAttack = this.gameObject.GetComponent<RangedAttackBehaviour>();
        } else {
            meleeAttack = this.gameObject.GetComponent<MeleeAttackBehaviour>();
        }
    }//Awake()

    private GameObject FindTarget(int pathfinderIdx) {
        path = GameObject.FindGameObjectsWithTag("Path");
        foreach (GameObject marker in path) {
            if (marker.GetComponent<PathfinderMarker>().markerNumber == pathfinderIdx) {
                return marker;
            }
        }
        return GameObject.Find("EnemyGoal");
    }//FindTarget(int)

    private void Pathfinding(){
        target = FindTarget(pathfinderIdx);
        if (target) {
            isMoving = true;
            Vector3 targetPos = target.transform.position;
            float distance = (transform.position-targetPos).magnitude;
            /*if (distance < 0.001f) {
                pathfinderIdx++;
            }*/
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            facing = (targetPos - transform.position).normalized;
        } else {
            Debug.LogWarning("Coudn't find target for Pathfinding");
            isMoving = false;
        }
    }//Pathfinding()

    private bool FindPlayer() {
        float distance = (transform.position - player.transform.position).magnitude;
        if (distance < visibilityRange) {
            return true;
        } else {
            return false;
        }
    }//FindPlayer()

    private void FindTower() {
        List<GameObject> towers = TowerManager.instance.towerList;
        targetTower = null;
        GameObject nearestTower = null;
        float shortestDistance = 99999.9f;
        foreach (GameObject tower in towers) {
            float distance = (transform.position - tower.transform.position).magnitude;
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestTower = tower;
            }
        }
        if (nearestTower != null && shortestDistance < visibilityRange) {
            targetTower = nearestTower;
        }
    }//FindTower()

    void AttackPlayer() {
        Vector2 direction = new Vector2(
            player.transform.position.x - transform.position.x, 
            player.transform.position.y - transform.position.y
        );
        float distance = (transform.position - player.transform.position).magnitude;
        attackState = (distance < attackRange);
        if (distance < attackRange) {
            isMoving = false;
            if (attackDelay < 0) {
                if (isRanged) {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Vector3 newRotation = new Vector3(0,0,angle);
                    rangedAttack.Attack(Quaternion.Euler(newRotation));
                } else {
                    meleeAttack.Attack(direction);
                }
            }
        } else {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        facing = (player.transform.position - transform.position).normalized;
    }//AttackPlayer()

    void RunAwayFromPlayer() {
        attackState = false;
        transform.Translate((transform.position - player.transform.position).normalized * movementSpeed * Time.deltaTime);
        facing = (player.transform.position - transform.position).normalized;
        isMoving = true;
    }//RunAwayFromPlayer()

    void AttackTower() {
        Vector2 direction = new Vector2(
            targetTower.transform.position.x - transform.position.x,
            targetTower.transform.position.y - transform.position.y
        );
        float distance = (transform.position - targetTower.transform.position).magnitude;
        attackState = (distance < attackRange);
        if (distance < attackRange) {
            isMoving = false;
            if (attackDelay < 0) {
                if (isRanged) {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Vector3 newRotation = new Vector3(0,0,angle);
                    rangedAttack.Attack(Quaternion.Euler(newRotation));
                } else {
                    meleeAttack.Attack(direction);
                }
            }
        } else {
            transform.position = Vector3.MoveTowards(transform.position, targetTower.transform.position, movementSpeed * Time.deltaTime);
            isMoving = true;
        }
        facing = (targetTower.transform.position - transform.position).normalized;
    }//AttackTower()

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Path") {
            pathfinderIdx = other.gameObject.GetComponent<PathfinderMarker>().markerNumber + 1;
        }
    }//OnTriggerEnter2D(Collider2D)

    private void OnDestroy()
    {//Called when enemy is destroyed; report to enemy manager
        EnemyManager.instance.EnemyExpire(this.gameObject);       
    }//OnDestroy()

    private void Update()
    {
        float distance = (transform.position - player.transform.position).magnitude;
        FindTower();
        if (FindPlayer()) {
            if (distance > runAwayRange) {
                AttackPlayer();
            } else {
                RunAwayFromPlayer();
            }
        } else if (targetTower != null) {
            AttackTower();
        } else {
            attackState = false;
            Pathfinding();
        }

        if (attackState) {
            attackDelay -= Time.deltaTime;
        } else {
            attackDelay = 0.3f;
        }

        //Update animation
        if (animator != null)
        {
            //var velocity = (transform.position - lastPos).normalized;
            animator.SetFloat("Horizontal",facing.x);
            animator.SetFloat("Vertical",facing.y);
            animator.SetFloat("Speed",isMoving ? movementSpeed : 0.0f); //To tell if stationary
            spriteRenderer.flipX = (facing.x <= 0.0001f); //Flip sprite if moving left
        }//if

    }//Update()
}//Enemy
