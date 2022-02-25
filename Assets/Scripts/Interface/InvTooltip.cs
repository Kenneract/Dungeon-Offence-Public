/*
* Author: Kennan
* Date: Mar.17.2021
* Summary: Manages the global tooltip; displays the window on screen, moves to locations, hides self when dragging starts, etc.
*/

using UnityEngine;
using UnityEngine.UI;

public class InvTooltip : MonoBehaviour {

	#region Variables
	public Text nameText;
	public Text descText;
	private bool isDrag;
	#endregion

	#region Singleton
    // Set up (semi-)singleton instance of self to allow global references
    public static InvTooltip instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.Log("Another InvTooltip found; taking over for it.");
			Destroy(instance.gameObject);
		}//if
		instance = this;      
    }//SetSingleton()
    #endregion

	void Awake()
	{
		SetSingleton();
	}//Awake()

	void Start()
	{
		Hide();
	}//Start()

	public void StartDrag()
	{//Manually called when ANY item starts being dragged
		isDrag = true;
		Hide();
	}//StartDrag()

	public void EndDrag()
	{//Manually called when ANY item ends being dragged (slightly before, on drop)
		isDrag = false;
	}//EndDrag()

	public void SetInfo(ItemInfo info)
	{//Sets the target ItemInfo to be used on this tooltip

		nameText.text = string.Format("{0}x {1}", info.quantity, info.itemName);
		descText.text = info.description;
	}//SetInfo(ItemInfo)

	public void Show(Vector2 pos)
	{
		if (!isDrag)
		{
			gameObject.SetActive(true);
			var rectTransform = this.gameObject.GetComponent<RectTransform>();
			float xAdd = Mathf.Abs(0.15f*(rectTransform.offsetMax.x - rectTransform.offsetMin.x));
			float yAdd = Mathf.Abs(0.30f*(rectTransform.offsetMax.y - rectTransform.offsetMin.y));
			gameObject.transform.position = pos + new Vector2(xAdd,-yAdd); //TODO: MAKE THESE VALUES SOME FIXED PORTION OF THE SIZE OF THE TOOLTIP OR SOMETHING, SO RESIZING WORKS RIGHT
		}
	}//Show(Vector2)

	public void Hide()
	{
		gameObject.SetActive(false);
	}//Hide()



}//InvTooltip