using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kevin
//Apr.10.2021

public class MouseCursor : MonoBehaviour
{
    public Color canPlace = Color.green;
    public Color cantPlace = Color.red;
    [Space]
    public GameObject rangeCircle; 
    
    private SpriteRenderer rendTower;
    private SpriteRenderer rendRange;
    private Controls controls;
    private Camera cam;
    private Vector3 cursorPos;
    private Vector2 mouseRaw;
    private bool shown;

    #region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static MouseCursor instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MouseCursor found.");
            //Destroy(this.gameObject);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

    void Awake()
    {
        SetSingleton(); //Set as singleton
        controls = new Controls();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }//Awake()

    private void OnEnable()
    {
        if (controls != null) controls.Enable();
    }//OnEnable()

    private void OnDisable()
    {
        if (controls != null) controls.Disable();
    }//OnDisable()

    void Start()
    {// Start is called before the first frame update
        HotbarManager.instance.onTowerSelectCallback += OnTowerSelect;
        HotbarManager.instance.onDeselectCallback += OnTowerDeselect;
        rendTower = GetComponent<SpriteRenderer>();
        rendRange = rangeCircle.GetComponent<SpriteRenderer>();
        //mouseRaw = controls.Interact.Aim.ReadValue<Vector2>();
        //cursorPos = cam.ScreenToWorldPoint(mouseRaw); 
        rendTower.enabled = false; //Don't show the sprite initially
        rendRange.enabled = false;
    }//Start()

    void OnTowerSelect()
    {//Callback event
        //print("Tower Selected!");
        //Enter anything needed for OnTowerSelect
        rendTower.enabled = true;
        rendRange.enabled = true;
        shown = true;
        //Grab the info for the tower
        var curInfo = HotbarManager.instance.GetCurInvSlot().itemInfo;
        var towerRange = curInfo.MyPrefab().GetComponent<Tower>().startRange;
        //Set up the icon/range circle
        rendTower.sprite = curInfo.icon;
        rendRange.transform.localScale = new Vector3(towerRange*2, towerRange*2, 1f );
    }//OnTowerSelect()

    void OnTowerDeselect()
    {
        rendTower.enabled = false;
        rendRange.enabled = false;
        shown = false;
    }//OnTowerDeselect()

    public Vector3 getMouseLocation()
    {
        return cursorPos;
    }//getMouseLocation()

    // Update is called once per frame
    void Update()
    {
        if (shown)
        {//Only move/raycast when cursor is visible
            mouseRaw = controls.Interact.Aim.ReadValue<Vector2>();
            cursorPos = cam.ScreenToWorldPoint(mouseRaw)+new Vector3(0,0,9f); //Makes Z of MouseCursor -1.
            Debug.DrawLine(cursorPos, new Vector3(0,0,100f)); //THIS FOR SOME REASON MAKES IT WORK??
            
            RaycastHit2D hit = Physics2D.Raycast(cursorPos, new Vector3(0,0,100f), Mathf.Infinity);
            //RaycastHit2D hit = Physics2D.Raycast(cursorPos, -Vector2.up);

            try {
                if (hit.collider != null){
                    //Debug.Log(hit.collider.gameObject.tag);
                    if(hit.collider.gameObject.CompareTag("TileMap")){
                        transform.position = cursorPos;
                        rendTower.color = canPlace;
                        rendRange.enabled = true;
                        rendRange.transform.position = cursorPos;
                    } else {
                        transform.position = cursorPos;
                        rendTower.color = cantPlace;
                        rendRange.enabled = false;

                    }//if
                } else {
                    transform.position = cursorPos;
                    rendTower.color = cantPlace;
                    rendRange.enabled = false;
                }//if
            } catch {
                Debug.Log("Error in MouseCursor");
            }//try-catch
        }//if
    }//Update()
}//MouseCursor