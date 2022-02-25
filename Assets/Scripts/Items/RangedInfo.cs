using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedInfo : MonoBehaviour
{
    #region Variables
    [Tooltip("How much damage the attack should do")]
    public int attackDamage = 25;
    //[Tooltip("(Roughly) The number of Unity units away the attack can reach")]
    //public float attackRange = 5.0f;
    [Tooltip("The number of seconds after an attack before can attack again")]
    public float attackCooldown = 0.3f;

    public bool throwable;
    //public GameObject projectile;
    [Header("Use this if it uses ammo")]
    public GameObject projectile;
    public float attackKnockback = 1.0f;
    [Header("Use this if it throws itself.")]
    public Sprite itemSprite;
	#endregion

    void Start() {
        itemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}