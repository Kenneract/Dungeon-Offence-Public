/*
* Author: Kennan
* Date: Mar.16.2021
* Summary: Contains information about the capabilities of a melee weapon; damage, range, cooldown, duration, attack image
*/

using UnityEngine;

public class MeleeInfo : MonoBehaviour {

	#region Variables
    [Tooltip("How much damage the attack should do")]
    public int attackDamage = 25;
    [Tooltip("(Roughly) The number of Unity units away the attack can reach")]
    public float attackRange = 0.5f;
    [Tooltip("The number of seconds the attack should do damage for")]
    public float attackTime = 0.2f;
    [Tooltip("The number of seconds after an attack before can attack again")]
    public float attackCooldown = 0.3f;
    public float attackKnockback = 1.0f;
    [Tooltip("The sprite to use for the attack.")]
	public Sprite attackImage;
	#endregion
	
}//MeleeInfo