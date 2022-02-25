using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : MonoBehaviour
{
    public GameObject explosion;
    public AudioClip explosionSound;
    private GameObject explosionInstance;
    private void Start(){
        Destroy(gameObject, 10f);
    }

    public void Explode() {
        Attack thisAttack = gameObject.GetComponent<Attack>();
        explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        Attack explosionAttack = explosionInstance.GetComponent<Attack>();
        explosionAttack.canDamageEnemy = thisAttack.canDamageEnemy;
        explosionAttack.canDamagePlayer = thisAttack.canDamagePlayer;
        explosionAttack.canDamageTower = thisAttack.canDamageTower;
        explosionAttack.damage = thisAttack.damage;
        SoundManager.instance.PlaySpatial(explosionSound, this.gameObject.transform.position);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Walls") {
            //spawn an explosion
            Destroy(gameObject);
        } 
    }
    
    private void Update(){
        transform.position += transform.right * 0.25f; 
    }
}
