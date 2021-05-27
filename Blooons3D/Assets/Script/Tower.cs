using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public float attackSpeed = 3f;
    public float baseattackSpeed = 3f;
    public float attackRange = 1f;
    public float baseattackRange = 1f;

    public Targeting targeting;
    public GameObject projectileDart;
    public int pierce = 1;
    public int power = 1;
    public int cost = 200;
    public int popcount;
    public TowerRadius radius;
    public bool activated = false;
    public bool camoDetection;
    public int level = 1;


    void Start()
    {
     
    }
    void Update()
    {

    }

    public virtual IEnumerator shoot()
    {
        yield return new WaitForSeconds(1f);
    }
    public void Sell()
    {
        FindObjectOfType<WaveManager>().AddCash(cost / 2);
        Destroy(gameObject);
    }

    public virtual void Upgrade1()
    {

    }
    public virtual void Upgrade2()
    {

    }
    public virtual void Upgrade3()
    {

    }

    public void Upgrade()
    {
        switch (level)
        {
            case 1:
                Upgrade1();
                level++;
                break;
            case 2:
                Upgrade2();
                level++;
                break;
            case 3:
                Upgrade3();
                break;
        }

 
    }
    public void SwapTargeting()
    {
        targeting.SwitchFiringMode();
    }
}
