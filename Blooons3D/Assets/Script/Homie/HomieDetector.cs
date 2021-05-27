using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomieDetector : MonoBehaviour
{
    public List<GameObject> Objectsinrange;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && Objectsinrange[0]!=null)
        {
            Interact();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
           

        }
    }
    private void OnTriggerExit(Collider other)
    {
        Objectsinrange.Remove(other.gameObject);
    }

    void Interact()
    {

     
    }

}
