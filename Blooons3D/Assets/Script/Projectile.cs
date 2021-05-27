using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Tower thrower;
    public enum damageType {sharp, acidic, explosive, ice, energy}
    public damageType damageMode;
    public bool camo;
    private void Start()
    {
        if (camo)
        {
            gameObject.tag = "CamoBloon";
        }
    }



}
