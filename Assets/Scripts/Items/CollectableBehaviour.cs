using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Author: Joshua (Kennan Modified)
    Date: Apr.7.2021
    Summary: Hover animation on object, moves towards player if in range, adds self to inventory and destroys self
*/

public class CollectableBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float hoverStrength = 0.1f;
    public float pickupDistance = 1.0f;
    public AudioClip pickupClip;
    private Vector2 hoverY;
    private float initialY;
    private GameObject player;
    private ItemInfo itemInfo;

    private Vector2 scatterVector; //unused?
    private int objectId; //unused?
    private int objectCount; //unused?


    void Start()
    {

        //Get reference to item's prefab
        try {
            itemInfo = this.GetComponent<ItemInfo>();
        } catch {
            Debug.LogWarning("WARNING: Object " + this.name + " is lacking an ItemInfo script.");
        }//try-catch

        initialY = transform.position.y;
    }//Start()
    
    // Update is called once per frame
    void Update()
    {
        player = PlayerController.instance.gameObject;
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= pickupDistance) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.1f);
            if (distance < 0.2f) {
                //Only destroy item/play sound if pickup was successful
                if (InventoryManager.instance.AddItem(itemInfo))
                {
                    Destroy(this.gameObject);
                    SoundManager.instance.PlayGlobal(pickupClip, 0.7f, 1.3f);
                }//if
            }//if
        } else {
            hoverY = transform.position;
            hoverY.y = initialY + (Mathf.Sin(Time.time*3.0f) * hoverStrength);
            transform.position = hoverY;
        }//if
    }//Update()
}//CollectableBehaviour