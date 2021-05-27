using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTargeting : Targeting
{
    public List<GameObject> bloonsInRange;
    //public GameObject target;
    //public Transform targetT;
    public float highestprogress;//
    //public int targetMode = 1;
    //private int speed = 99999;
    //public GameObject head;



    void Update()
    {

        if (bloonsInRange.Count == 0)
        {

        }
        if (targetT)
        {
            Debug.DrawLine(gameObject.transform.position, targetT.position, Color.red);
        }
        switch (targetMode)
        {
            case 1:
                targetT = TargetFirst();
                break;
            case 2:
                target = TargetLast();
                break;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bloon"))
        {
            bloonsInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bloon"))
        {
            bloonsInRange.Remove(other.gameObject);
        }
    }
    Transform TargetFirst()
    {
        float highest = 0;
        GameObject highestBloon = null;
        if (bloonsInRange.Count > 0)
        {
            foreach (GameObject b in bloonsInRange)
            {
                if (b != null)
                {
                    float progress = b.GetComponent<Bloon>().progress;
                    if (progress > highest)
                    {
                        highest = progress;
                        highestBloon = b;
                        highestprogress = highest;
                    }
                }
            }
            if (highestBloon)
            {
                return highestBloon.transform;

            }
            else { return null; }
        }
        else { return null; }
    }

    void TargetStrong() { }
    void TargetClose() { }
    GameObject TargetLast()
    {
        float lowest = 1;
        GameObject lowestBloon = null;
        if (bloonsInRange.Count == 0)
        {
            foreach (GameObject b in bloonsInRange)
            {
                float progress = b.GetComponent<Bloon>().progress;
                if (progress < lowest)
                {
                    lowest = progress;
                    lowestBloon = b;
                }
            }
            return lowestBloon;
        }
        else { return null; }
    }
}
