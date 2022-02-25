using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    //int damage = 0; //might use later instead of accessing parent

    //public string[] DamageableTags = {"Enemy"};
    public bool canDamagePlayer;
    public bool canDamageTower;
    public bool canDamageEnemy;
    public int damage;

    //private Vector2 attackDirection;
    public float knockback;
    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    /*void OnTriggerEnter2D(Collider2D other) {
    }*/
}
