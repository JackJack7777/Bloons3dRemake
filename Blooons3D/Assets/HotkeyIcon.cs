using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeyIcon : MonoBehaviour
{
    public Text buttonText;
    public Text Desc;
    public void UpdateInfo(string buttontext, string desctext)
    {
        buttonText.text = buttontext;
        Desc.text = desctext;
    }
    
}
