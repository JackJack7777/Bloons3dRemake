using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSpot : MonoBehaviour
{
    public levelSpot Up,Down,Left,Right;
    public bool hasLevel = true;
    public MeshRenderer Square;
    void Start()
    {
        Grid grid = FindObjectOfType<Grid>();

        foreach(GameObject g in grid.points)
        {
            if(g.transform.position == new Vector3(transform.position.x+grid.spacing, 0, transform.position.z))
            {
                Up = g.GetComponent<levelSpot>();
            } 
            else if (g.transform.position == new Vector3(transform.position.x - grid.spacing, 0, transform.position.z))
            {
                Down = g.GetComponent<levelSpot>();
            }
            else if (g.transform.position == new Vector3(transform.position.x, 0, transform.position.z - grid.spacing))
            {
                Left = g.GetComponent<levelSpot>();
            }
            else if (g.transform.position == new Vector3(transform.position.x, 0, transform.position.z + grid.spacing))
            {
                Right = g.GetComponent<levelSpot>();
            }
        }

        int initialTile = Random.Range(0, grid.points.Count-1);
        Debug.Log(grid.points.Count);




    }

    // Update is called once per frame
    void Update()
    {
        if (hasLevel)
        {
            Square.enabled = true;
        }
        else { Square.enabled = false;
        }
    }
}
