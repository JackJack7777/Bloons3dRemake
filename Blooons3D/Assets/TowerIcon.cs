using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerIcon : MonoBehaviour
{
    public int ID;
    public void UpdateInfo(int selected)
    {
        if (selected == ID)
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }

    }
    
    

          
    
}
