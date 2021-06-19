using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject block;
    public int Width = 10;
    public uint Height = 10;
    public uint Depth = 10;
    public int spacing = 10;
    public int amount = 5;
    public List<GameObject> points = new List<GameObject>();

    void Start()
    {
        int amount2 = spacing * amount;

        for (int x = 0; x < amount2; x=x + spacing)
        {

            GameObject thing = Instantiate(block, new Vector3(x, 0, 0), Quaternion.identity);
            points.Add(thing);

            for (int z = 0; z < amount2-amount; z=z+ spacing) {
                GameObject thing2 = Instantiate(block, new Vector3(x, 0, z), Quaternion.identity);
                points.Add(thing2);
            }
        }




    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
