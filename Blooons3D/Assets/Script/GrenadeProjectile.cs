using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : Projectile
{
    public int power = 1;
    public int pierce;
    private SphereCollider collider;
    List<Collider> blosns = null;
    bool exploded;

    private void Awake()
    {
        StartCoroutine(Despawn());
        collider = gameObject.GetComponent<SphereCollider>();
        collider.radius = 0.43f;
        blosns = new List<Collider>();
    }
    private void Update()
    {
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bloon"))
        {

            blosns.Add(other);
            if (!exploded)
            {
            StartCoroutine(Explode());
            exploded = true;
            }
        }
    }
    IEnumerator Explode()
    {
        collider.radius = collider.radius = 5f;
        yield return new WaitForSeconds(.01f);
        if (gameObject)
        {
            foreach(Collider b in blosns)
            {
                if (b)
                {
                    b.GetComponent<Bloon>().TakeDamage(power, this.gameObject, damageType.explosive);
                    b.GetComponent<Bloon>().lastHitBy = gameObject;
                }
            }
        }
            collider.radius = 0.43f;
        Destroy(gameObject);




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
