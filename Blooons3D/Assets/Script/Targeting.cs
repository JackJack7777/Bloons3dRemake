using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private List<GameObject> bloonsInRange = new List<GameObject>();
    public GameObject target;
    public Transform targetT;
    public int targetMode = 1;
    public GameObject head;
    public Tower tower;
    public bool playerControl;



    void Update()
    {
        gameObject.GetComponent<SphereCollider>().radius = tower.attackRange;
        if (bloonsInRange.Count == 0)
        {
        }
        if (targetT)
        {
            Debug.DrawLine(gameObject.transform.position, targetT.position, Color.red);
        }
        if (tower.activated)
        {
            targetT=Target();
        }
    }
    public void SwitchFiringMode()
    {
        targetMode++;
        if (targetMode > 4)
        {
            targetMode = 1;
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
    public void Aim()
    {
        if(tower.GetComponent<MinigunTower>() != null)
        {
            if (tower.GetComponent<MinigunTower>().playerControl)
            {
                head.transform.rotation = FindObjectOfType<Camera>().gameObject.transform.rotation;

            }
        }
        else
        {
            if (targetT != null && bloonsInRange.Count > 0)
            {
                {
                    Vector3 targetDirection = targetT.transform.position - transform.position;
                    float singleStep = 99999 * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                    gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
                    //TURN MONKY HEAD
                    head.transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
    }
    Transform Target()
    {   
            Aim();
            float lowest = 99;
            float highest = 0;
            GameObject highestBloon = null;
            if (bloonsInRange.Count > 0)
            {
                foreach (GameObject b in bloonsInRange)
                {
                    if (b != null)//checks if bloon alive
                    {
                        if (b.GetComponent<Bloon>().camo && !tower.camoDetection)
                        {
                        //makes camo bloons untargetable if no camo detection
                    }
                    else
                        {
                        float progress = b.GetComponent<Bloon>().progress;
                        switch (targetMode)
                        {
                            case 1://FIRST
                                if (progress > highest)
                                {
                                    highest = progress;
                                    highestBloon = b;
                                }
                                break;
                            case 2://LAST
                                if (progress < lowest)
                                {
                                    lowest = progress;
                                    highestBloon = b;
                                }
                                break;

                            case 3://CLOSE
                                float distance = Vector3.Distance(transform.position, b.transform.position);
                                if (distance < lowest)
                                {
                                    lowest = distance;
                                    highestBloon = b;
                                }
                                break;
                            case 4://STRONG
                                int RBE = b.GetComponent<Bloon>().RBE;
                                if (RBE > highest)
                                {
                                    highest = RBE;
                                    highestBloon = b;
                                }
                                break;
                            }             
                        }
                    }
                }
                if (highestBloon)
                {
                    target = highestBloon;
                    return highestBloon.transform;
                }
                else { return null; }
            }
            else { return null; }
    }
}