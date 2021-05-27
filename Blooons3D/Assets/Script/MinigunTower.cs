using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunTower : Tower
{
    public Transform playerSpot;
    public GameObject shootSpot;
    public GameObject gunPart;
    public GameObject laser;
    public GameObject barrel;
    public bool playerControl;
    public bool shoooty = false;
    public bool Stalling;
    public bool canBeSelceted=true;
    private void Start()
    {
        StartCoroutine(shooting());
    }
    public IEnumerator stall()
    {
        Stalling = true;
        yield return new WaitForSeconds(.1f);
        Stalling = false;
    }
    private void Update()
    {
        if (playerControl)
        {
            canBeSelceted = false;
            targeting.playerControl = true;
            laser.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                shoooty = true;

            }
        if (Input.GetMouseButtonUp(0))
        {
            shoooty = false;
        }
        }
        else
        {
            canBeSelceted = true;
            laser.SetActive(false);
        }
    }
    IEnumerator shooting()
    {
        
        yield return new WaitUntil(() => shoooty == true);
        barrel.transform.Rotate(80, 0, 0);
        GameObject bullet = Instantiate(projectileDart, shootSpot.transform.position, shootSpot.transform.rotation) as GameObject;
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
        StartCoroutine(shooting());

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
