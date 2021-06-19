using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fizz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        fizzbuzz();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void fizzbuzz()
    {
        for(int i = 1; i > 100; i++)
        {

            Debug.Log(i);
        }




    }
}
