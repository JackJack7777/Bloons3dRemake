﻿using System.Collections;
using UnityEngine;

public class SniperMonkey : Tower
{
    void Start()


    {
        StartCoroutine(shoot());



    }

    private void Update()
    {
    }
    public override IEnumerator shoot()
    {
        yield return new WaitUntil(() => targeting.targetT != null);
        yield return new WaitForSeconds(.01f);

        GameObject bullet = Instantiate(projectileDart, targeting.transform.position, targeting.transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(targeting.transform.forward * 5000);
        bullet.GetComponent<DartProjectile>().pierce = pierce;
        bullet.GetComponent<DartProjectile>().power = power;
        if (camoDetection)
        {
            bullet.layer = 14;
        }
        bullet.GetComponent<Projectile>().damageMode = Projectile.damageType.sharp;
        bullet.GetComponent<Projectile>().thrower = this;
        yield return new WaitForSeconds(1 / attackSpeed);
        StartCoroutine(shoot());
    }
    public override void Upgrade1()
    {
        attackSpeed = baseattackSpeed * 2;

    }
    public override void Upgrade2()
    {
        attackRange = baseattackRange * 1.5f;
    }
    public override void Upgrade3()
    {
        camoDetection = true;
    }
}
