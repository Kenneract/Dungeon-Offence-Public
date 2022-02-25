/*
* Author: Kennan
* Date: Apr.2.2021
* Summary: Contains information about a given piece of armour
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourInfo : MonoBehaviour {

	#region Variables
    [Tooltip("The percent of damage that this armour should absorb.")]
    public float defense = 0.00f;
	#endregion

}//ArmourInfo