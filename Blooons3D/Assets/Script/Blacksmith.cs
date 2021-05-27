using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : Tower
{
    public AnvilTargeting Atargeting;


    void Start()
    {
        StartCoroutine(shoot());
    }
    public override IEnumerator shoot()
    {
        yield return new WaitUntil(() => Atargeting.targetT != null);
        yield return new WaitForSeconds(.01f);
        GameObject bullet = Instantiate(projectileDart, Atargeting.transform.position, Atargeting.transform.rotation) as GameObject;
        bullet.GetComponent<AnvilProjectile>().pierce = pierce;
        bullet.GetComponent<AnvilProjectile>().power = power;
        if (camoDetection)
        {
            bullet.layer = 14;
        }
        bullet.GetComponent<Projectile>().thrower = this;
        yield return new WaitForSeconds(attackSpeed);
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
