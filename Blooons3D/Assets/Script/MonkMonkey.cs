using System.Collections;
using UnityEngine;

public class MonkMonkey : Tower
{
    public GameObject fist;
    void Start()
    {
        StartCoroutine(shoot());
    }

    public override IEnumerator shoot()
    {
        yield return new WaitUntil(() => targeting.targetT != null);
        yield return new WaitForSeconds(.01f);
        targeting.target.GetComponent<Bloon>().TakeDamage(power, fist, Projectile.damageType.sharp);


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