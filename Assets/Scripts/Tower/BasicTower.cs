using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kevin
//Apr.6

public class BasicTower : Tower
{
    public Transform pivot;
    public Transform barrel;
    public RangedAttackBehaviour rangeAttack;

    new void Awake()
    {
        base.Awake(); //Just making sure this doesnt accidentally get overridden
        rangeAttack = this.gameObject.GetComponent<RangedAttackBehaviour>();
    }//Awake()

    //new void OnEnable()
    //{
    //    base.OnEnable(); //Just making sure this doesnt accidentally get overridden
    //}//OnEnable()
    new void Start()
    {
        base.Start(); //Just making sure this doesnt accidentally get overridden
    }//Start()

    protected override void Shoot(){
        BarrelRotation barrelRotation = GetComponent<BarrelRotation>();
        if(barrelRotation.adjusted){
            if (base.HaveAmmo()) //Only shoot if have ammo
            {
                base.Shoot();
                bool did = rangeAttack.Attack(pivot.rotation);
                if (did) base.UseAmmo(); //If shot, use ammo
            }//if
        }//if
    }//Shoot()
}//BasicTower