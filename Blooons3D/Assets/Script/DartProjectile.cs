using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : Projectile
{
    public int power = 1;
    public int pierce;
    private void Awake()
    {
        StartCoroutine(Despawn());

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bloon"))
        {
            other.GetComponent<Bloon>().TakeDamage(power, this.gameObject, damageMode);
            other.GetComponent<Bloon>().lastHitBy = gameObject;
            pierce = pierce - 1;
            if(pierce <= 0)
            {
                Destroy(gameObject);
            }
        
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(.3f);
        if (gameObject)
        {
        Destroy(gameObject);

        }

    }
}
