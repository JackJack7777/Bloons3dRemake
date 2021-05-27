using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRadius : MonoBehaviour
{
    MeshRenderer renderer;
    public List<TowerRadius> radiuses = new List<TowerRadius>();
    public bool beingTouched;
    public bool selected;
    public bool hovered;
    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.enabled = false;
        beingTouched = false;
    }
    private void Update()
    {
        updatecolor();

    }

    public IEnumerator PlaceTower()
    {
        yield return new WaitForSeconds(.1f);
        radiuses.Clear();

    }
    void updatecolor()
    {
        if (radiuses.Count > 0)
        {
            renderer.material.color = new Color(.8f, 0f, 0f, .5f);
            renderer.enabled = true;
            beingTouched = true;

        }
        else
        {
            beingTouched = false;
            if (hovered)
            {
                renderer.material.color = new Color(0f, 0f, .8f, .5f);
                renderer.enabled = true;
                if (selected)
                {
                    renderer.material.color = new Color(0f, .8f, 0f, .5f);
                }
            }
            else
            {
                renderer.enabled = false;

            }

        }




        if (radiuses.Count<0 && !selected && !hovered)
        {
        }
        





    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TowerRadius"))
        {
            radiuses.Add(other.GetComponent<TowerRadius>());
            updatecolor();

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TowerRadius"))
        {
            radiuses.Remove(other.GetComponent<TowerRadius>());
            updatecolor();
        }
    }





}
