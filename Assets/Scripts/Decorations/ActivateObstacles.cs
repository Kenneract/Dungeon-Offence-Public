using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObstacles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Debug.Log(collider);
        collider.size = new Vector3(1.5f, 1.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //collider.size = new Vector3(1.5f, 1.5f, 0f);
    }
}
