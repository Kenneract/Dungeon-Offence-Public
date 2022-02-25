using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kevin

public class Bullet : MonoBehaviour
{

    private void Start(){
        if (this.transform.eulerAngles.z > 90.0f && this.transform.eulerAngles.z < 270.0f) {
            this.gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
        Destroy(gameObject, 10f);
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Walls") {
            Destroy(gameObject);
        }
    }
    
    private void Update(){
        transform.position += transform.right * 0.25f; 
    }
}
